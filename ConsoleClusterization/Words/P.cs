using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Clusters.Words
{
    public class P: Word //слово-предложение
    {

        public P() : base()
        {
        }

        public P(string parametrs)
        {
            var args = parametrs.Split(' ');
            if (args[0] != "p") throw new Exception("Method CreateP recieved wrong data");
            var other = args.ToList();
            other.RemoveAt(0);
            foreach (string arg in other)
            {
                switch (arg)
                {
                    default:
                        throw new Exception("New parametr has arrived: " + arg);
                        break;
                }
            }
        }
    }
}
