using ConsoleClusterization.Additional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClusterization.Metrics
{
    public interface Metrica<T>
    {
        double distance(T a, T b);

        BaseFuzzyObject multiply(T item, double mult);

        double distance(BaseFuzzyObject fuzzy, T straight);
        
        double distance(T straight, BaseFuzzyObject fuzzy);

    }
}
