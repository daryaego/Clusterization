using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Clusters.Words
{
    public class Numeral : Word
    {
        [XmlAttribute]
        public bool Animation;
        [XmlAttribute]
        private bool _plural;
        [XmlAttribute]
        private Gender _gender;
        [XmlAttribute]
        private Case _case;
        [XmlAttribute]
        private bool _animation;

        public Numeral() : base()
        {
        }

        public Numeral(string pars)
        {
            var args = pars.Split(' ');
            if (args[0] != "num") throw new Exception("Method CreateNUM recieved wrong data");
            var other = args.ToList();
            other.RemoveAt(0);
            foreach (string arg in other)
            {
                switch (arg)
                {
                    //число
                    case "мн":
                        _plural = true;
                        break;
                    case "ед":
                        _plural = false;
                        break;

                    //род
                    case "муж":
                        _gender = Gender.M;
                        break;
                    case "жен":
                        _gender = Gender.W;
                        break;
                    case "сред":
                        _gender = Gender.N;
                        break;

                    //падеж
                    case "им":
                        _case = Case.Nominative;
                        break;
                    case "род":
                        _case = Case.Genetive;
                        break;
                    case "парт":
                        _case = Case.Partitive;
                        break;
                    case "дат":
                        _case = Case.Dative;
                        break;
                    case "вин":
                        _case = Case.Accusative;
                        break;
                    case "твор":
                        _case = Case.Instrumentalis;
                        break;
                    case "пр":
                        _case = Case.Praepositionalis;
                        break;
                    case "местн":
                        _case = Case.Locative;
                        break;

                    //одушевленность
                    case "од":
                        _animation = true;
                        break;
                    case "неод":
                        _animation = false;
                        break;

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
