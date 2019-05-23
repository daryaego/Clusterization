using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Clusters.Words
{
    public class Conjunction : Word
    {
        public Conjunction() : base()
        {
        }

        public Conjunction(string parametrs)
        {
            var args = parametrs.Split(' ');
            if (args[0] != "conj") throw new Exception("Method CreateCONJ recieved wrong data");
            var other = args.ToList();
            other.RemoveAt(0);
            foreach (string arg in other)
            {
                switch (arg)
                {
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
