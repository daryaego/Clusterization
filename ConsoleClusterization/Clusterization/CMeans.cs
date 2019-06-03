using ConsoleClusterization.Additional;
using ConsoleClusterization.Metrics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClusterization.Clusterization
{
    public class CMeans<T> : FuzzyMethod<T>
    {
        private BaseFuzzyObject[] centers;
        private NormalDistribution distribution = new NormalDistribution(dispersion: 0.5);
        private double loss = double.MaxValue;

        public CMeans(float trustLevel = 0.9f)
        {
            this.trustLevel = trustLevel;
        }

        public override List<List<T>> clusterize(List<T> set, Metrica<T> metrica, int count)
        {
            initialize(set, metrica, count);
            do
            {
                calculateAssiciationMatrix();
                moveCenters();
            } while (ifLoss());
            return prepareResult();
        }

        private void initialize(List<T> set, Metrica<T> metrica, int count)
        {
            centers = new BaseFuzzyObject[count];
            clustersCount = count;
            this.set = set;
            this.metrica = metrica;
            associationMatrix = new double[set.Count][];
            for (int i = 0; i < set.Count; i++)
            {
                associationMatrix[i] = new double[count];
            }

            var rand = new Random();
            for (int i = 0; i < count; i++)
            {
                centers[i] = metrica.multiply(set[rand.Next(set.Count)], 1);
            }
        }

        private bool ifLoss()
        {
            Console.Write("Loss fnction calculating... ");

            Task<double>[] tasks = new Task<double>[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                Tuple<int, int> coordinates = new Tuple<int, int>(i, threadCount);
                tasks[i] = Task.Factory.StartNew((tuple) =>
                {
                    return lossFunctionPatly(((Tuple<int, int>)tuple).Item1, ((Tuple<int, int>)tuple).Item2);
                }, coordinates);
            }
            Task.WaitAll(tasks);
            var lossFunction = 0.0;

            foreach (var task in tasks)
                lossFunction += task.Result;

            for (int i = 0; i < threadCount; i++)
                tasks[i].Dispose();

            Console.WriteLine(lossFunction);

            if (lossFunction < loss)
            {
                loss = lossFunction;
                return true;
            }
            else return false;
        }

        private double lossFunctionPatly(int start, int step)
        {
            var lossFunction = 0.0;

            for (int i = start; i < set.Count; i+=step)
            {
                for (int j = 0; j < clustersCount; j++)
                {
                    var dist = metrica.distance(set[i], centers[j]);
                    lossFunction += dist * dist * associationMatrix[i][j];
                }
            }
            return lossFunction;
        }

        private void moveCenters()
        {
            Console.WriteLine("Moving clusters centers");

            Task[] tasks = new Task[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                Tuple<int, int> coordinates = new Tuple<int, int>(i, threadCount);
                tasks[i] = Task.Factory.StartNew((tuple) =>
                {
                    movingCentersInThread(((Tuple<int, int>)tuple).Item1, ((Tuple<int, int>)tuple).Item2);
                    return;
                }, coordinates);
            }
            Task.WaitAll(tasks);

            for (int i = 0; i < threadCount; i++)
                tasks[i].Dispose();

        }

        private void movingCentersInThread(int startPosition, int step)
        {
            for (int k = startPosition; k < clustersCount; k += step)
            {
                var divider = 0.0;
                var dividend = metrica.multiply(set[0], associationMatrix[0][k]);
                for (int i = 1; i < set.Count; i++)
                {
                    dividend = dividend + metrica.multiply(set[i], associationMatrix[i][k]);
                    divider += associationMatrix[i][k];
                }
                centers[k] = dividend / divider;
            }
        }

        private void calculateAssiciationMatrix()
        {
            Console.WriteLine("Association matrix calculating");
            Task[] tasks = new Task[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                Tuple<int, int> coordinates = new Tuple<int, int>(i, threadCount);
                tasks[i] = Task.Factory.StartNew((tuple) =>
                {
                    initializeAssociationMatrix(((Tuple<int, int>)tuple).Item1, ((Tuple<int, int>)tuple).Item2);
                    return;
                }, coordinates);
            }
            Task.WaitAll(tasks);

            for (int i = 0; i < threadCount; i++)
                tasks[i].Dispose();
        }

        private void initializeAssociationMatrix(int startPosition, int step)
        {
            for (int i = startPosition; i < associationMatrix.Length; i += step)
            {
                var sum = 0.0;
                for (int j = 0; j < clustersCount; j++)
                {
                    var distance = metrica.distance(set[i], centers[j]);
                    distance = distribution.Distribution(distance);
                    associationMatrix[i][j] = distance;
                    sum += distance;
                }
                for (int j = 0; j < clustersCount; j++)
                {
                    associationMatrix[i][j] = associationMatrix[i][j] / sum;
                }
            }
        }
    }
}
