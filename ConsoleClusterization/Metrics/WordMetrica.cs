using Clusters.Words;
using ConsoleClusterization.Additional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClusterization.Metrics
{
    // метрика без приоритетов
    // Штрафы:
    //  несоответствие типов - typePenalty, 
    //  отсутствие соответствующего поля - propertyPenalty, 
    //  несоответствие значения поля - propertyValuePenalty
    public class WordMetrica : Metrica<Word>
    {
        private double typePenalty;
        private double propertyPenalty;
        private double propertyValuePenalty;

        public WordMetrica(double typePenalty = 1.0, double propertyPenalty = 0.8, double propertyValuePenalty = 0.5)
        {
            this.typePenalty = typePenalty;
            this.propertyPenalty = propertyPenalty;
            this.propertyValuePenalty = propertyValuePenalty;
        }

        public double distance(Word a, Word b)
        {
            double maximumDifference = typePenalty;
            double result = 0;
            if (a.GetType() != b.GetType())
                result += typePenalty;

            var aFields = a.GetType().GetFields().ToList();
            var bFields = b.GetType().GetFields().ToList();
            foreach (FieldInfo info in aFields)
            {
                var temp = from field in bFields
                           where field.Name == info.Name
                           select field;
                if (temp.Count() == 0)
                {
                    result += propertyPenalty;
                    continue;
                }
                maximumDifference += propertyPenalty;
                var bField = temp.First();
                var bFieldValue = bField.GetValue(b);
                var aFieldValue = info.GetValue(a);
                if (!object.Equals(aFieldValue, bFieldValue))
                    result += propertyValuePenalty;
                maximumDifference += propertyValuePenalty;
            }

            foreach(var info in bFields)
            {
                var temp = from field in aFields
                           where field.Name == info.Name
                           select field;
                if(temp.Count()==0)
                {
                    result += propertyPenalty;
                    maximumDifference += propertyPenalty;
                }
            }

            return result/maximumDifference;
        }

        public double distance(BaseFuzzyObject bFuzzy, Word straight)
        {
            var fuzzy = bFuzzy as FuzzyObject;
            var res = 0.0;
            var type = straight.GetType();
            if (!fuzzy.Belongings["type"].ContainsKey(type.Name))
                res += typePenalty;
            else res += Math.Abs(fuzzy.Belongings["type"][type.Name] * typePenalty - typePenalty);
            foreach (var field in type.GetFields())
            {
                if (!fuzzy.Belongings.ContainsKey(field.Name))
                    res += propertyPenalty;
                else if (!fuzzy.Belongings[field.Name].ContainsKey(field.GetValue(straight).ToString()))
                    res += propertyValuePenalty;
                else res += (1 - fuzzy.Belongings[field.Name][field.GetValue(straight).ToString()]) * propertyValuePenalty;
            }
            return res;
        }

        public double distance(Word straight, BaseFuzzyObject fuzzy)
        {
            return this.distance(fuzzy, straight);
        }

        public BaseFuzzyObject multiply(Word item, double mult)
        {
            var fuzzy = new FuzzyObject();
            var typeDictionary = new Dictionary<string, double>();
            typeDictionary.Add(item.GetType().Name, mult);
            fuzzy.Belongings.Add("type", typeDictionary);

            var fields = item.GetType().GetFields().ToList();

            foreach (var field in fields)
            {
                var temp = new Dictionary<string, double>();
                temp.Add(field.GetValue(item).ToString(), mult);
                fuzzy.Belongings.Add(field.Name, temp);
            }

            return fuzzy;
        }
    }
}
