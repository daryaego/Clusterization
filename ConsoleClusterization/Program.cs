using Clusters.Words;
using ConsoleClusterization.Clusterization;
using ConsoleClusterization.Metrics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Clusters
{
    internal class Program
    {
        private static List<Triplet<Word>> data;
        private static int resultItemsCount;

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            data = new List<Triplet<Word>>();
            readClusters("..\\SynTagRus2017\\2003");

            var method = new FarestNeighbor<Triplet<Word>>();
            var result = method.clusterize(data, new MultTripletMetrica<Word>(new WordMetrica(0.8, 0.5, 0.3)), 11);

            var format = new XmlSerializer(typeof(List<Triplet<Word>>));
            var statFormat = new BinaryFormatter();//new XmlSerializer(typeof(Dictionary<Type, int>));
            resultItemsCount = 0;
            FileStream stream;
            for (int i = 0; i < result.Count; i++)
            {
                resultItemsCount += result[i].Count;
                var fileName = "clustersResult" + i + ".xml";
                stream = File.Create(fileName);
                format.Serialize(stream, result[i]);
                stream.Close();

                var stat = getStatForCluster(result[i]);
                fileName = "stat" + fileName;
                stream = File.Create(fileName);
                statFormat.Serialize(stream, stat);
                stream.Close();
            }

            var dataStat = getStatForCluster(data);
            stream = File.Create("dataStat.xml");
            statFormat.Serialize(stream, dataStat);
            stream.Close();

            Console.WriteLine("Data count: " + data.Count + "\nResult count: " + resultItemsCount);
            Console.Read();
        }

        private static Dictionary<string, int> getStatForCluster(List<Triplet<Word>> list)
        {
            var stat = new Dictionary<string, int>();
            foreach (var item in list)
            {
                var type = item.second.GetType().Name;
                try
                {
                    stat[type] = stat[type] + 1;
                }
                catch
                {
                    stat.Add(type, 1);
                }
            }
            Console.WriteLine("____________________________________________________");
            foreach (var key in stat.Keys)
                Console.WriteLine(key + ": " + stat[key]);
            return stat;
        }

        private static void readClusters(string path)
        {
            var directory = new DirectoryInfo(path);
            var temp = new Word();
            var wordFlag = false;
            Word first = null, second = null, third = null;
            foreach (var dir in directory.GetDirectories())
                foreach (var file in dir.GetFiles("*.tgt"))
                {
                    Console.WriteLine(dir.Name + file.Name);
                    var stream = file.OpenText();
                    using (var reader = XmlReader.Create(stream))
                    {
                        while (reader.Read())
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    if (reader.Name == "W")
                                    {
                                        var type = reader.GetAttribute("FEAT");
                                        temp = Word.CreateWord(type.ToLower(CultureInfo.CurrentCulture));
                                        wordFlag = true;
                                    }
                                    break;
                                case XmlNodeType.Text:
                                    if (wordFlag)
                                    {
                                        temp.Value = reader.Value.ToLower(CultureInfo.CurrentCulture);

                                        if (first == null)
                                        {
                                            //first = new Word();
                                            first = temp.Copy();
                                        }
                                        else if (second == null)
                                        {
                                            //second = new Word();
                                            second = temp.Copy();
                                        }
                                        else if (third == null)
                                        {
                                            //third = new Word();
                                            third = temp.Copy();
                                            data.Add(new Triplet<Word>(first, second, third));
                                        }
                                        else
                                        {
                                            first = second.Copy();
                                            second = third.Copy();
                                            third = temp.Copy();
                                            var triplet = new Triplet<Word>(first, second, third);
                                            data.Add(triplet);
                                        }
                                    }
                                    break;
                                case XmlNodeType.EndElement:
                                    if (reader.Name == "S")
                                        first = second = third = null;
                                    else if (reader.Name == "W")
                                        wordFlag = false;
                                    break;
                                default:
                                    //Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
                                    break;
                            }
                        //Console.WriteLine(data[data.Count - 1]);
                    }
                    stream.Close();
                    Console.WriteLine(data.Count);
                }
            Console.WriteLine("Files had been readed. Triplets found:" + data.Count);

        }

        private static void trimData()
        {
            for (int i = 0; i < data.Count; i++)
            {
                var anotherPosition = maxIndexOf(data[i], i + 1);
                while (anotherPosition > i)
                {
                    data.RemoveAt(anotherPosition);
                    anotherPosition = data.LastIndexOf(data[i]);
                    Console.WriteLine(data.Count);
                }

            }

        }

        private static int maxIndexOf(Triplet<Word> triplet, int from)
        {
            var threadCount = 8;
            Task<int>[] tasks = new Task<int>[threadCount];
            int step = (int)Math.Ceiling((double)(data.Count - from) / (double)threadCount);
            for (int i = 0; i < threadCount; i++)
            {
                var coordinates = new Tuple<int, int>(from + i * step, step);
                tasks[i] = Task<int>.Factory.StartNew((tuple) =>
                {
                    var size = ((Tuple<int, int>)tuple).Item1 + ((Tuple<int, int>)tuple).Item2;
                    var count = size < data.Count ? ((Tuple<int, int>)tuple).Item2 : ((Tuple<int, int>)tuple).Item2 - (size - data.Count) - 1;
                    return data.IndexOf(triplet, ((Tuple<int, int>)tuple).Item1, count);
                }, coordinates);
            }
            Task.WaitAll(tasks);
            var max = tasks[0].Result;
            foreach (var task in tasks)
                max = Math.Max(max, task.Result);
            return max;
        }
    }
}
