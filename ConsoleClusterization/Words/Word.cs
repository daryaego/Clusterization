using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace Clusters.Words
{

    [XmlInclude(typeof(Adjective))]
    [XmlInclude(typeof(Adverb))]
    [XmlInclude(typeof(Composite))]
    [XmlInclude(typeof(Conjunction))]
    [XmlInclude(typeof(Interjection))]
    [XmlInclude(typeof(NID))]
    [XmlInclude(typeof(Noun))]
    [XmlInclude(typeof(Numeral))]
    [XmlInclude(typeof(P))]
    [XmlInclude(typeof(Particle))]
    [XmlInclude(typeof(Preposition))]
    [XmlInclude(typeof(Verb))]
    public class Word
    {
        public string Value;

        [XmlAttribute]
        public bool _composite = false;

        [XmlAttribute]
        public bool _softened = false;

        [XmlAttribute]
        public bool _nonStandart = false;

        [XmlAttribute]
        public bool _wrong = false;

        [XmlAttribute]
        public Person _person;

        [XmlAttribute]
        public string _link;

        [XmlAttribute]
        public string _lemma;

        //public Word(Word image)
        //{
        //    this.Value = image.Value;
        //    this._composite = image._composite;
        //    this._softened = image._softened;
        //    this._nonStandart = image._nonStandart;
        //    this._wrong = image._wrong;
        //    this._person = image._person;
        //}

        public override string ToString()
        {
            return Value;
        }

        public virtual Word Copy()
        {
            Word result = (Word)this.MemberwiseClone();
            result.Value = String.Copy(this.Value);
            return result;
            //var res = new Word();
            //res.Value = w.Value;
            //res._composite = w._composite;
            //res._person = w._person;
            //res._softened = w._softened;
            //return res;
        }
        public enum Case
        {
            Nominative/*Именительный*/, Genetive/*Родительный*/, Partitive/*Партитивный*/, Dative/*Дательный*/, Accusative/*Винителный*/, Instrumentalis/*Творительный*/, Praepositionalis/*Предложный*/, Locative /*Местный*/
        }

        public enum Gender
        {
            M, W, N
        }

        public enum Comparability
        {
            None, Сomparative, Superior
        }

        public enum Representation
        {
            Personal, Infinitive, Participle, Adverbal
        }

        public enum Inclination
        {
            Indicative/*Изъявительное*/, Imperative /*Повелительное*/, Subjunctive /*Сослагательное*/
        }

        public enum Time
        {
            Past, Present, NotPast
        }

        public enum Person
        {
            First, Second, Third
        }

        public static Word CreateWord(string par)
        {
            par = par.TrimEnd().TrimStart();
            var type = par.Split(' ')[0];
            Word res;
            switch (type)
            {
                case "s":
                    res = CreateS(par);
                    break;
                case "a":
                    res = CreateA(par);
                    break;
                case "v":
                    res = CreateV(par);
                    break;
                case "adv":
                    res = CreateADV(par);
                    break;
                case "num":
                    res = CreateNUM(par);
                    break;
                case "pr":
                    res = CreatePR(par);
                    break;
                case "com":
                    res = CreateCOM(par);
                    break;
                case "conj":
                    res = CreateCONJ(par);
                    break;
                case "part":
                    res = CreatePART(par);
                    break;
                case "p":
                    res = CreateP(par);
                    break;
                case "nid":
                    res = CreateNID(par);
                    break;
                case "intj":
                    res = CreateINTJ(par);
                    break;
                default:
                    throw new Exception("Something went wrong. Parsing words");
                    break;
            }
            return res;
        }

        public static Noun CreateS(string parametrs)
        {
            return new Noun(parametrs);
        }

        public static Adjective CreateA(string parametrs)
        {
            return new Adjective(parametrs);
        }

        public static Verb CreateV(string parametrs)
        {
            return new Verb(parametrs);
        }

        public static Adverb CreateADV(string parametrs)
        {
            return new Adverb(parametrs);
        }

        public static Numeral CreateNUM(string parametrs)
        {
            return new Numeral(parametrs);
        }

        public static Preposition CreatePR(string parametrs)
        {
            return new Preposition(parametrs);
        }

        public static Composite CreateCOM(string parametrs)
        {
            return new Composite(parametrs);
        }

        public static Conjunction CreateCONJ(string parametrs)
        {
            return new Conjunction(parametrs);
        }

        public static Particle CreatePART(string parametrs)
        {
            return new Particle(parametrs);
        }

        public static P CreateP(string parametrs)
        {
            return new P(parametrs);
        }

        public static Interjection CreateINTJ(string parametrs)
        {
            return new Interjection(parametrs);
        }

        public static NID CreateNID(string parametrs)
        {
            return new NID(parametrs);
        }
    }
}