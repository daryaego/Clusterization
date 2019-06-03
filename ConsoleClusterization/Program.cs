using Clusters.Words;
using ConsoleClusterization.Additional;
using ConsoleClusterization.Clusterization;
using ConsoleClusterization.Metrics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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

            var methods = new List<ClusteringMethod<Triplet<Word>>>();
            methods.Add(new NearestNaighbor<Triplet<Word>>());
            methods.Add(new FarestNeighbor<Triplet<Word>>());
            methods.Add(new CMeans<Triplet<Word>>(0.95f));

            var standartBaseMetrica = new WordMetrica();

            var metrics = new List<Metrica<Triplet<Word>>>();
            metrics.Add(new SumTripletMetrica<Word>(standartBaseMetrica));
            metrics.Add(new MultTripletMetrica<Word>(standartBaseMetrica));
            metrics.Add(new MaxTripletMetrica<Word>(standartBaseMetrica));

            foreach (var method in methods)
                foreach (var metrica in metrics)
                    clusterizeAndSave(method, metrica, 11);


            Console.WriteLine("Data count: " + data.Count + "\nResult count: " + resultItemsCount);
            Console.Read();
        }

        private static void clusterizeAndSave(ClusteringMethod<Triplet<Word>> method, Metrica<Triplet<Word>> metrica, int count)
        {
            Console.WriteLine("Clustering method " + method.ToString());
            Console.WriteLine("Metrica " + metrica.ToString());
            var path = "..\\" + method.ToString() + "\\" + metrica.ToString() + "\\";
            Directory.CreateDirectory(path);
            var result = method.clusterize(data, metrica, count);

            var format = new XmlSerializer(typeof(List<Triplet<Word>>));
            var statFormat = new BinaryFormatter();
            resultItemsCount = 0;
            FileStream stream;
            for (int i = 0; i < result.Count; i++)
            {
                resultItemsCount += result[i].Count;
                var fileName = path + "clustersResult" + i + ".xml";
                stream = File.Create(fileName);
                format.Serialize(stream, result[i]);
                stream.Close();

                var fs = File.OpenWrite(path + "clusterStat" + i + ".txt");
                var stat = getStatForCluster(result[i], fs);
                fs.Close();
            }

            var dataStat = getStatForCluster(data, File.OpenWrite(path + "dataStat.txt"));
        }

        private static Dictionary<string, int> getStatForCluster(List<Triplet<Word>> list, FileStream fs)
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
            foreach (var key in stat.Keys)
                fs.Write(new UTF8Encoding(true).GetBytes(key + ": " + (100 * (double)stat[key] / (double)list.Count) + "\n"));
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
                                        temp._link = reader.GetAttribute("LINK");
                                        temp._lemma = reader.GetAttribute("LEMMA").ToLower(CultureInfo.CurrentCulture);
                                        if (temp._link == null)
                                            temp._link = "root";
                                        wordFlag = true;
                                    }
                                    break;
                                case XmlNodeType.Text:
                                    if (wordFlag)
                                    {
                                        temp.Value = reader.Value.ToLower(CultureInfo.CurrentCulture);
                                        if (first == null)
                                        {
                                            first = temp.Copy();
                                        }
                                        else if (second == null)
                                        {
                                            second = temp.Copy();
                                        }
                                        else if (third == null)
                                        {
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
                                    {
                                        first = second = third = null;
                                    }
                                    else if (reader.Name == "W")
                                        wordFlag = false;
                                    break;
                                default:
                                    break;
                            }
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
