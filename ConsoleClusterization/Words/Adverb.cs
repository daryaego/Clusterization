using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Clusters.Words
{
    public class Adverb : Word
    {
        [XmlAttribute]
        public Comparability _comparability;

        public Adverb() : base()
        {
        }

        public Adverb(string parametrs)
        {
            var args = parametrs.Split(' ');
            if (args[0] != "adv") throw new Exception("Method CreateS recieved wrong data");
            var other = args.ToList();
            other.RemoveAt(0);
            foreach (string arg in other)
            {
                switch (arg)
                {
                    //степень сравнения
                    case "срав":
                        _comparability = Comparability.Сomparative;
                        break;
                    case "прев":
                        _comparability = Comparability.Superior;
                        break;

                    //смягченная сравнительная степень
                    case "смяг":
                        _softened = true;
                        break;

                    //нестандартное написание слова
                    case "нестанд":
                        _nonStandart = true;
                        break;

                    //неправильное употребление слова
                    case "неправ":
                        _wrong = true;
                        break;

                    //не уверенна, что у меня полная памятка, ПОПОЗЖЕ ПОИСКАТЬ НА САЙТЕ СИНТАГРУСА
                    case "мета":
                        break;

                    default:
                        throw new Exception("New parametr has arrived: " + arg);
                        break;
                }
            }
        }
    }
}
