using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Clusters.Words
{
    public class Noun : Word //существительное
    {
        [XmlAttribute]
        public bool _plural;
        [XmlAttribute]
        public Gender _gender;
        [XmlAttribute]
        public Case _case;
        [XmlAttribute]
        public bool _animation;

        public Noun() : base()
        {
        }

        public Noun(string pars)
        {
            var args = pars.Split(' ');
            if (args[0] != "s") throw new Exception("Method CreateS recieved wrong data");
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

                    //нестандартное написание слова
                    case "нестанд":
                        _nonStandart = true;
                        break;

                    //форма, используемая в словосложении
                    case "сл":
                        _composite = true;
                        break;

                    //понятия не имею, что ха параметр, в памятке к корпусу ничего про него нет
                    case "зв":
                        break;

                    //неправильное употребление слова
                    case "неправ":
                        _wrong = true;
                        break;

                    //не уверенна, что у меня полная памятка, ПОПОЗЖЕ ПОИСКАТЬ НА САЙТЕ СИНТАГРУСА
                    case "мета":
                        _wrong = true;
                        break;



                    default:
                        throw new Exception("New parametr has arrived: " + arg);
                        break;
                }
            }
        }
    }
}
