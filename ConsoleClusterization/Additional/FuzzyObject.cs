using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClusterization.Additional
{
    public class FuzzyObject : BaseFuzzyObject
    {
        public Dictionary<string, Dictionary<string, double>> Belongings;

        public FuzzyObject() : base()
        {
            Belongings = new Dictionary<string, Dictionary<string, double>>();
        }

        protected override BaseFuzzyObject plus(BaseFuzzyObject fuzzy)
        {
            var first = this;
            var second = fuzzy as FuzzyObject;

            var result = new FuzzyObject();
            foreach (var property in first.Belongings)
            {
                result.Belongings.Add(property.Key, new Dictionary<string, double>());
                if (second.Belongings.ContainsKey(property.Key))
                {
                    foreach (var value in property.Value)
                        if (second.Belongings[property.Key].ContainsKey(value.Key))
                            result.Belongings[property.Key].Add(value.Key, value.Value + second.Belongings[property.Key][value.Key]);
                        else
                            result.Belongings[property.Key].Add(value.Key, value.Value);
                }
            }

            foreach (var property in second.Belongings)
            {
                if (!result.Belongings.ContainsKey(property.Key))
                    result.Belongings.Add(property.Key, new Dictionary<string, double>());
                foreach (var value in second.Belongings[property.Key])
                    if (!result.Belongings[property.Key].ContainsKey(value.Key))
                        result.Belongings[property.Key].Add(value.Key, value.Value);
            }

            return result;
        }

        public override BaseFuzzyObject divide(double div)
        {
            var result = new FuzzyObject();
            foreach (var property in Belongings)
            {
                var values = new Dictionary<string, double>();
                foreach (var value in property.Value)
                {
                    //Belongings[property.Key][value.Key] = Belongings[property.Key][value.Key] / div;
                    values.Add(value.Key, Belongings[property.Key][value.Key] / div);
                }
                result.Belongings.Add(property.Key, values);

            }
            return result;
        }

    }
}
