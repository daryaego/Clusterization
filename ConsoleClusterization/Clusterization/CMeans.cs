using ConsoleClusterization.Metrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClusterization.Clusterization
{
    public class CMeans<T> : FuzzyMethod<T>
    {
        public CMeans(float trustLevel=0.9f)
        {
            this.trustLevel = trustLevel;
        }

        public override List<List<T>> clusterize(List<T> set, Metrica<T> metrica, int count)
        {

            return prepareResult();
        }
    }
}
