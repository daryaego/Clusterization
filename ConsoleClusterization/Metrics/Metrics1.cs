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
    public class Metrics1 : Metrica<Word>
    {
        public double distance(Word a, Word b)
        {
            int result = 0;
            if (a.GetType() != b.GetType())
                result += 15;

            var aFields = a.GetType().GetFields().ToList();
            var bFields = b.GetType().GetFields().ToList();
            foreach (FieldInfo info in aFields)
            {
                var temp = from field in bFields
                           where field.Name == info.Name
                           select field;
                if (temp.Count() == 0)
                {
                    result += 10;
                    continue;
                }
                var bField = temp.First();
                var bFieldValue = bField.GetValue(b);
                var aFieldValue = info.GetValue(a);
                var fine = 1;
                //switch (info.Name)
                //{
                //    case "Value":
                //        fine = 0;
                //        break;
                //    default:
                //        break;
                //}
                if (!object.Equals(aFieldValue, bFieldValue))
                    result += 5 * fine;
            }

            return (double)result;
        }
    }
}
