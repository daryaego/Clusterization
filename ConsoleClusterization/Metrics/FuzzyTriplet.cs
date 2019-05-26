using Clusters.Metrica;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClusterization.Metrics
{
    public class FuzzyTriplet : FuzzyObject
    {
        public Triplet<FuzzyObject> triplet;

        public FuzzyTriplet()
        { }

        public FuzzyTriplet(FuzzyObject f, FuzzyObject s, FuzzyObject t)
        {
            triplet = new Triplet<FuzzyObject>(f, s, t);
        }
    }
}
