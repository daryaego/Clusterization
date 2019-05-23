using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Clusters.Words
{
    public class Particle : Word
    {
        public Particle() : base()
        {
        }

        public Particle(string parametrs)
        {
            var args = parametrs.Split(' ');
            if (args[0] != "part") throw new Exception("Method CreatePART recieved wrong data");
            var other = args.ToList();
            other.RemoveAt(0);
            foreach (string arg in other)
            {
                switch (arg)
                {
                    //неправильное употребление слова
                    case "неправ":
                        _wrong = true;
                        break;

                    //нестандартное написание слова
                    case "нестанд":
                        _nonStandart = true;
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
