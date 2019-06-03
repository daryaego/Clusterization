using ConsoleClusterization.Metrics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleClusterization.Clusterization
{
    public abstract class ClusteringMethod<T>
    {
        protected static int threadCount = 6;
        protected int clustersCount = 10;
        protected Metrica<T> metrica;
        protected List<T> set;
        protected double[][] objectsDistances;

        public abstract List<List<T>> clusterize(List<T> set, Metrica<T> metrica, int count);

        protected double getObjectsDistance(int i, int j)
        {
            return objectsDistances[Math.Max(i, j)][Math.Min(i, j)];
        }
    }
}
