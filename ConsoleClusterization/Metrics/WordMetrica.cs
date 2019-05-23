using Clusters.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Clusters.Metrica
{
    // метрика без приоритетов
    // Штрафы:
    //  несоответствие типов - 15, 
    //  отсутствие соответствующего поля - 10, 
    //  несоответствие значения поля - 5
    public class WordMetrica : Metrica<Word>
    {
        double typePenalty;
        double propertyPenalty;
        double propertyValuePenalty;

        public WordMetrica(double typePenalty=1.0, double propertyPenalty=0.8, double propertyValuePenalty=0.5)
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
    }
}
