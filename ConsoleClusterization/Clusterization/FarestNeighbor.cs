using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClusterization.Clusterization
{
    public class FarestNeighbor<T> : HierarchicalMethod<T>
    {
        public FarestNeighbor() : base()
        {
        }

        public override double distance(int firstClusterPosition, int secondClusterPosition)
        {
            var firstCluster = clusters[operatingClusters[firstClusterPosition]];
            var secondCluster = clusters[operatingClusters[secondClusterPosition]];
            double maxDistance = metrica.distance(set[firstCluster[0]], set[secondCluster[0]]);
            for (int i = 1; i < firstCluster.Count; i++)
            {
                for (int j = 0; j < secondCluster.Count; j++)
                {
                    var temp = metrica.distance(set[firstCluster[i]], set[secondCluster[j]]);
                    if (temp > maxDistance)
                        maxDistance = temp;
                }
            }
            return maxDistance;
        }
    }
}
