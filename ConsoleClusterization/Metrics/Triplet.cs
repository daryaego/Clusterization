using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusters.Metrica
{
    public class Triplet<T>
    {
        public T first;
        public T second;
        public T third;

        public Triplet(){}
        public Triplet (T f, T s, T t)
        {
            first = f;
            second = s;
            third = t;
        }

        public override bool Equals(object obj)
        {
            var triplet = obj as Triplet<T>;
            return triplet != null &&
                   EqualityComparer<T>.Default.Equals(first, triplet.first) &&
                   EqualityComparer<T>.Default.Equals(second, triplet.second) &&
                   EqualityComparer<T>.Default.Equals(third, triplet.third);
        }

        public override string ToString()
        {
            return "{" + first + ", " + second + ", " + third + "}";
        }
    }
}
