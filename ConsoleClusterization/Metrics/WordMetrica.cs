using Clusters.Words;
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
                var bField = temp.First();
                var bFieldValue = bField.GetValue(b);
                var aFieldValue = info.GetValue(a);
                var fine = 1.0;
                //switch (info.Name)
                //{
                //    case "Value":
                //        fine = 0;
                //        break;
                //    default:
                //        break;
                //}
                if (!object.Equals(aFieldValue, bFieldValue))
                    result += propertyValuePenalty * fine;
            }

            return (double)result;
        }

        public double distance(FuzzyObject fuzzy, Word straight)
        {
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
                else res += fuzzy.Belongings[field.Name][field.GetValue(straight).ToString()] * propertyValuePenalty + propertyValuePenalty;
            }
            return res;
        }

        public double distance(Word straight, FuzzyObject fuzzy)
        {
            return this.distance(fuzzy, straight);
        }

        public FuzzyObject multiply(Word item, double mult)
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
