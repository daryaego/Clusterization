using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClusterization.Additional
{
    public class FuzzyTriplet : BaseFuzzyObject
    {
        public Triplet<BaseFuzzyObject> triplet;

        public FuzzyTriplet()
        {
            triplet = new Triplet<BaseFuzzyObject>();
        }

        public FuzzyTriplet(BaseFuzzyObject f, BaseFuzzyObject s, BaseFuzzyObject t)
        {
            triplet = new Triplet<BaseFuzzyObject>(f, s, t);
        }

        protected override BaseFuzzyObject plus(BaseFuzzyObject fuzzy)
        {
            var item = fuzzy as FuzzyTriplet;
            return new FuzzyTriplet(this.triplet.first + item.triplet.first,
                this.triplet.second + item.triplet.second,
                this.triplet.third + item.triplet.third);
        }

        public override BaseFuzzyObject divide(double div)
        {
            return new FuzzyTriplet(this.triplet.first / div,
                this.triplet.second / div,
                this.triplet.third / div);
        }
    }
}
