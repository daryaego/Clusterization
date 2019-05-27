using ConsoleClusterization.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClusterization.Clusterization
{
    public abstract class HierarchicalMethod<T> : ClusteringMethod<T>
    {
        protected List<int>[] clusters;
        protected double[][] clustersDistances;
        protected List<int> operatingClusters;

        public override List<List<T>> clusterize(List<T> set, Metrica<T> metrica, int count)
        {
            this.initialize(set, metrica, count);

            while (operatingClusters.Count > this.clustersCount)
            {
                int firstCluster = 0; int secondCluster = 1;
                this.getMinPositions(out firstCluster, out secondCluster);
                this.uniteClusters(firstCluster, secondCluster);
                if (operatingClusters.Count % 100 == 0) Console.WriteLine(operatingClusters.Count);
            }

            return toListList();
        }

        private List<List<T>> toListList()
        {
            Console.WriteLine("Converting into List<List<T>>");
            var res = new List<List<T>>();
            for (int i = 0; i < operatingClusters.Count; i++)
            {
                res.Add(new List<T>());
                for (int j = 0; j < clusters[operatingClusters[i]].Count; j++)
                {
                    res[i].Add(set[clusters[operatingClusters[i]][j]]);
                }
            }

            operatingClusters.Clear();
            return res;
        }

        private void uniteClusters(int firstCluster, int secondCluster)
        {
            clusters[operatingClusters[firstCluster]] = clusters[operatingClusters[firstCluster]].Concat(clusters[operatingClusters[secondCluster]]).ToList();
            operatingClusters.RemoveAt(secondCluster);

            //пересчитать расстояния для firstCluster
            Task[] tasks = new Task[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                tasks[i] = Task.Factory.StartNew((value) =>
                {
                    var start = (int)value;
                    for (int j = start; j < operatingClusters.Count; j += threadCount)
                        if (firstCluster != j)
                            setDistance(firstCluster, j, distance(firstCluster, j));
                    return;
                }, i);
            }
            Task.WaitAll(tasks);
            for (int i = 0; i < threadCount; i++)
                tasks[i].Dispose();
        }

        private void initialize(List<T> set, Metrica<T> metrica, int count)
        {
            clustersCount = count;
            this.set = set;
            this.metrica = metrica;
            clusters = new List<int>[set.Count];
            clustersDistances = new double[set.Count][];
            objectsDistances = new double[set.Count][];
            operatingClusters = new List<int>();
            for (int i = 0; i < set.Count; i++)
            {
                //сначала каждый кластер содержит один элемент
                clusters[i] = new List<int>();
                clusters[i].Add(i);
                clustersDistances[i] = new double[i];
                objectsDistances[i] = new double[i];
                operatingClusters.Add(i);
            }

            Task[] tasks = new Task[threadCount];
            int step = (int)Math.Ceiling((double)clustersDistances.Length / threadCount);
            for (int i = 0; i < threadCount; i++)
            {
                Tuple<int, int> coordinates = new Tuple<int, int>(i, threadCount);
                tasks[i] = Task.Factory.StartNew((tuple) =>
                {
                    initializeDistances(((Tuple<int, int>)tuple).Item1, ((Tuple<int, int>)tuple).Item2);
                    return;
                }, coordinates);
            }
            Task.WaitAll(tasks);
            for (int i = 0; i < threadCount; i++)
                tasks[i].Dispose();
        }

        private void initializeDistances(int startPosition, int step)
        {
            for (int i = startPosition; i < clustersDistances.Length; i += step)
            {
                for (int j = 0; j < i; j++)
                {
                    clustersDistances[i][j] = (metrica.distance(set[i], set[j]));
                    objectsDistances[i][j] = clustersDistances[i][j];
                }
                if (i % 1000 == 0) Console.WriteLine(i);
            }
        }

        private void getMinPositions(out int firstPosition, out int secondPosition)
        {
            firstPosition = 0;
            secondPosition = 1;
            Task<Tuple<double, int, int>>[] tasks = new Task<Tuple<double, int, int>>[threadCount];
            int step = (int)Math.Ceiling((double)operatingClusters.Count / (double)threadCount);
            for (int i = 0; i < threadCount; i++)
            {
                Tuple<int, int> coordinates = new Tuple<int, int>(i, threadCount);
                tasks[i] = Task.Factory.StartNew((tuple) =>
                {
                    return Count(((Tuple<int, int>)tuple).Item1, ((Tuple<int, int>)tuple).Item2);
                }, coordinates);
            }
            Task.WaitAll(tasks);

            int minPosition = 0;
            for (int i = 0; i < threadCount; i++)
                if ((tasks[i].Result).Item1 < (tasks[minPosition].Result).Item1)
                    minPosition = i;
            firstPosition = Math.Min((tasks[minPosition].Result).Item2, (tasks[minPosition].Result).Item3);
            secondPosition = Math.Max((tasks[minPosition].Result).Item2, (tasks[minPosition].Result).Item3);

            for (int i = 0; i < threadCount; i++)
                tasks[i].Dispose();
        }

        private Tuple<double, int, int> Count(int startPosition, int step)
        {
            if (startPosition > operatingClusters.Count) return new Tuple<double, int, int>(double.MaxValue, -1, -1);

            int firstCluster = startPosition;
            int secondCluster = 0 == firstCluster ? 1 : 0;
            double min = getDistance(firstCluster, secondCluster);

            for (int i = startPosition; i < operatingClusters.Count; i += step)
                for (int j = 0; j < i; j++) //distances[i].Count = i as initialized
                {
                    var temp = getDistance(i, j);
                    if (temp < min)
                    {
                        min = temp;
                        firstCluster = i;
                        secondCluster = j;
                    }
                }
            return new Tuple<double, int, int>(min, firstCluster, secondCluster);
        }

        private void setDistance(int i, int j, double value)
        {
            var posi = operatingClusters[i];
            var posj = operatingClusters[j];
            if (i == j) return;
            clustersDistances[Math.Max(posi, posj)][Math.Min(posi, posj)] = value;
        }

        private double getDistance(int i, int j)
        {
            var posi = operatingClusters[i];
            var posj = operatingClusters[j];
            if (i == j) return 0;
            else return clustersDistances[Math.Max(posi, posj)][Math.Min(posi, posj)];
        }

        public abstract double distance(int firstCluster, int secondCluster);
    }
}
