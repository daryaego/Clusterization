using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClusterization.Additional
{
    public abstract class BaseFuzzyObject
    {
        public BaseFuzzyObject()
        { }

        public static BaseFuzzyObject operator +(BaseFuzzyObject first, BaseFuzzyObject second)
        {
            return first.plus(second);
        }

        protected abstract BaseFuzzyObject plus(BaseFuzzyObject fuzzy);


        public static BaseFuzzyObject operator /(BaseFuzzyObject item, double div)
        {
            return item.divide(div);
        }

        public abstract BaseFuzzyObject divide(double div);
    }
}
