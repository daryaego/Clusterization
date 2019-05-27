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

        FuzzyObject multiply(T item, double mult);

        double distance(FuzzyObject fuzzy, T straight);
        
        double distance(T straight, FuzzyObject fuzzy);

    }
}
