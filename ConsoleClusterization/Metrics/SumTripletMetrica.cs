﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClusterization.Metrics
{
    internal class SumTripletMetrica<T> : Metrica<Triplet<T>>
    {
        private Metrica<T> baseMetrica;
        public SumTripletMetrica(Metrica<T> baseMetrica)
        {
            this.baseMetrica = baseMetrica;
        }

        public double distance(Triplet<T> a, Triplet<T> b)
        {
            return baseMetrica.distance(a.first, b.first) + baseMetrica.distance(a.third, b.third);
        }

        public double distance(FuzzyObject fuzzy, Triplet<T> straight)
        {
            var triplet = fuzzy as FuzzyTriplet;
            return baseMetrica.distance(triplet.triplet.first, straight.first) + baseMetrica.distance(triplet.triplet.third, straight.third);
        }

        public double distance(Triplet<T> straight, FuzzyObject fuzzy)
        {
            return this.distance(fuzzy, straight);
        }

        public FuzzyObject multiply(Triplet<T> item, double mult)
        {
            return new FuzzyTriplet(baseMetrica.multiply(item.first, mult),
                baseMetrica.multiply(item.second, mult),
                baseMetrica.multiply(item.third, mult));
        }
    }
}
