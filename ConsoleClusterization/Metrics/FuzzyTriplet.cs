using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClusterization.Metrics
{
    public class FuzzyTriplet : FuzzyObject
    {
        public Triplet<FuzzyObject> triplet;

        public FuzzyTriplet()
        {
            triplet = new Triplet<FuzzyObject>();
        }

        public FuzzyTriplet(FuzzyObject f, FuzzyObject s, FuzzyObject t)
        {
            triplet = new Triplet<FuzzyObject>(f, s, t);
        }

        public static FuzzyObject operator +(FuzzyTriplet first, FuzzyTriplet second)
        {
            return new FuzzyTriplet(first.triplet.first + second.triplet.first,
                first.triplet.second + second.triplet.second,
                first.triplet.third + second.triplet.third);
        }

        public static FuzzyObject operator /(FuzzyTriplet first, double div)
        {
            return new FuzzyTriplet(first.triplet.first / div,
                first.triplet.second / div,
                first.triplet.third / div);
        }
    }
}
