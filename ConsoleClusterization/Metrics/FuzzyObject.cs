using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClusterization.Metrics
{
    public class FuzzyObject
    {
        public Dictionary<string, Dictionary<string, double>> Belongings;

        public FuzzyObject()
        {
            Belongings = new Dictionary<string, Dictionary<string, double>>();
        }

        public static FuzzyObject operator +(FuzzyObject first, FuzzyObject second)
        {
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

        public static FuzzyObject operator/(FuzzyObject item, double div)
        {
            foreach (var property in item.Belongings)
                foreach (var value in property.Value)
                    item.Belongings[property.Key][value.Key] = item.Belongings[property.Key][value.Key] / div;
            return item;
        }


    }
}
