using Clusters.Metrica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusters.Metrica
{
    class TripletMetrica<T> : Metrica<Triplet<T>>
    {
        Metrica<T> baseMetrica;
        public TripletMetrica(Metrica<T> baseMetrica)
        {
            this.baseMetrica = baseMetrica;
        }

        public double distance(Triplet<T> a, Triplet<T> b)
        {
            return baseMetrica.distance(a.first, b.first) + baseMetrica.distance(a.third, b.third);
        }
    }
}
