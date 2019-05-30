using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Clusters.Words
{
    public class Verb : Word //глогол
    {
        [XmlAttribute]
        public bool _plural;
        [XmlAttribute]
        public Gender _gender;
        [XmlAttribute]
        public bool _shortness = false;
        [XmlAttribute]
        public bool _perfection;
        [XmlAttribute]
        public bool _active = true; //действительный залог
        [XmlAttribute]
        public Time _time;
        [XmlAttribute]
        public Case _case;
        [XmlAttribute]
        public Representation _representation = Representation.Personal;
        [XmlAttribute]
        public bool _passive = false;
        [XmlAttribute]
        public Mood _mood = Mood.Subjunctive;
        [XmlAttribute]
        public bool _animation;


        public Verb() : base()
        {
        }

        public enum Mood //наклонение
        {
            Subjunctive /*сослагательное*/, Insicative /*изъявительное*/, Imperative /*повелительное*/
        }

        public Verb(string pars)
        {
            var args = pars.Split(' ');
            if (args[0] != "v") throw new Exception("Method CreateS recieved wrong data");
            var other = args.ToList();
            other.RemoveAt(0);
            foreach (string arg in other)
            {
                switch (arg)
                {
                    //совершенность
                    case "сов":
                        _perfection = true;
                        break;
                    case "несов":
                        _perfection = false;
                        break;

                    //страдательный залог
                    case "страд":
                        _passive = true;
                        break;

                    //репрезентация
                    case "инф":
                        _representation = Representation.Infinitive;
                        break;
                    case "прич":
                        _representation = Representation.Participle;
                        break;
                    case "деепр":
                        _representation = Representation.Adverbal;
                        break;

                    //время
                    case "непрош":
                        _time = Time.NotPast;
                        break;
                    case "прош":
                        _time = Time.Past;
                        break;
                    case "наст":
                        _time = Time.Present;
                        break;

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

                    //краткость
                    case "кр":
                        _shortness = true;
                        break;

                    //число
                    case "изъяв":
                        _mood = Mood.Insicative;
                        break;
                    case "пов":
                        _mood = Mood.Imperative;
                        break;

                    //одушевленность
                    case "од":
                        _animation = true;
                        break;
                    case "неод":
                        _animation = false;
                        break;

                    //лицо
                    case "1-л":
                        _person = Person.First;
                        break;
                    case "2-л":
                        _person = Person.Second;
                        break;
                    case "3-л":
                        _person = Person.Third;
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
