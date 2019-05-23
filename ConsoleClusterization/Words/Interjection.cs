using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Clusters.Words
{
    public class Interjection : Word
    {
        public Interjection() : base()
        { }

        public Interjection(string parametrs)
        {
            var args = parametrs.Split(' ');
            if (args[0] != "intj") throw new Exception("Method CreateINTJ recieved wrong data");
            var other = args.ToList();
            other.RemoveAt(0);
            foreach (string arg in other)
            {
                switch (arg)
                {
                    //нестандартное написание слова
                    case "нестанд":
                        _nonStandart = true;
                        break;
                    default:
                        throw new Exception("New parametr has arrived: " + arg);
                        break;
                }
            }
        }
    }
}
