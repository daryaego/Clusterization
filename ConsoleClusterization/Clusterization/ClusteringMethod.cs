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
        private static int threadCount = 4;
        protected int clustersCount = 10;
        protected Metrica<T> metrica;
        protected List<T> set;

        public abstract List<List<T>> clusterize(List<T> set, Metrica<T> metrica, int count);

        //public abstract double distance(int firstCluster, int secondCluster);
    }
}
