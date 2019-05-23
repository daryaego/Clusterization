using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Clusters.Words
{
    public class Composite : Word
    {
        public Composite() : base()
        {
        }

        public Composite(string parametrs)
        {
            var args = parametrs.Split(' ');
            if (args[0] != "com") throw new Exception("Method CreateCOM recieved wrong data");
            var other = args.ToList();
            other.RemoveAt(0);
            foreach (string arg in other)
            {
                switch (arg)
                {

                    //форма, используемая в словосложении
                    case "сл":
                        _composite = true;
                        break;
                    default:
                        throw new Exception("New parametr has arrived: " + arg);
                        break;
                }
            }
        }
    }
}
