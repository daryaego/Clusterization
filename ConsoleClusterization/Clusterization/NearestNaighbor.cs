using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClusterization.Clusterization
{
    public class NearestNaighbor<T> : HierarchicalMethod<T>
    {
        public NearestNaighbor() : base()
        {
        }

        public override double distance(int firstClusterPosition, int secondClusterPosition)
        {
            var firstCluster = clusters[operatingClusters[firstClusterPosition]];
            var secondCluster = clusters[operatingClusters[secondClusterPosition]];
            double minDistance = getObjectsDistance(firstCluster[0], secondCluster[0]);
            for (int i = 1; i < firstCluster.Count; i++)
            {
                for (int j = 0; j < secondCluster.Count; j++)
                {
                    var temp = getObjectsDistance(firstCluster[i], secondCluster[j]);
                    if (temp < minDistance)
                        minDistance = temp;
                }
            }
            return minDistance;
        }
    }
}
