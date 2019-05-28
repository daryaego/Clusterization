using ConsoleClusterization.Metrics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClusterization.Clusterization
{
    public class CMeans<T> : FuzzyMethod<T>
    {
        private FuzzyObject[] centers;
        public CMeans(float trustLevel = 0.9f)
        {
            this.trustLevel = trustLevel;
        }

        public override List<List<T>> clusterize(List<T> set, Metrica<T> metrica, int count)
        {
            this.initialize(set, metrica, count);

            return prepareResult();
        }

        private void initialize(List<T> set, Metrica<T> metrica, int count)
        {
            centers = new FuzzyObject[count];
            clustersCount = count;
            this.set = set;
            this.metrica = metrica;
            objectsDistances = new double[set.Count][];
            associationMatrix = new double[set.Count][];
            for (int i = 0; i < set.Count; i++)
            {
                objectsDistances[i] = new double[i];
                associationMatrix[i] = new double[count];
            }

            var rand = new Random();
            for (int i = 0; i < count; i++)
                centers[i] = metrica.multiply(set[rand.Next(set.Count)], 1);
            
            Task[] tasks = new Task[threadCount];
            int step = (int)Math.Ceiling((double)objectsDistances.Length / threadCount);
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
            for (int i = startPosition; i < objectsDistances.Length; i += step)
            {
                for (int j = 0; j < clustersCount; j++)
                    associationMatrix[i][j] = metrica.distance(set[i], centers[j]);
                if (i % 1000 == 0) Console.WriteLine(i);
            }
        }
    }
}
