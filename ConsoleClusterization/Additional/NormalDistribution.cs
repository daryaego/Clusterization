using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClusterization.Additional
{
    public class NormalDistribution
    {
        private double expectedValue;
        private double dispersion;
        private double divider;
        private double multiplier;
        public NormalDistribution(double expected=0, double dispersion=1)
        {
            this.expectedValue = expected;
            this.dispersion = dispersion;

            divider = 2 * dispersion * dispersion;
            multiplier = 1 / (dispersion * Math.Sqrt(2 * Math.PI));
        }

        public double Distribution(double x)
        {
            return Math.Exp(-Math.Pow((x - expectedValue), 2) / divider) * multiplier;
        }
    }
}
