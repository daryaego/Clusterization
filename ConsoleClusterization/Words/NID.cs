using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Clusters.Words
{
    public class NID : Word
    {
        public NID() : base()
        { }

        public NID(string parametrs)
        {
            var args = parametrs.Split(' ');
            if (args[0] != "nid") throw new Exception("Method CreateNID recieved wrong data");
            var other = args.ToList();
            other.RemoveAt(0);
            foreach (string arg in other)
            {
                switch (arg)
                {
                    //очередной неизвестный науке параметр
                    case "s":
                        break;
                    default:
                        throw new Exception("New parametr has arrived: " + arg);
                        break;
                }
            }
        }
    }
}
