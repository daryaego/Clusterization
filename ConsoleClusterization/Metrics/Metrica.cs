using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusters.Metrica
{
    public interface Metrica<T>
    {
        double distance(T a, T b);
    }
}
