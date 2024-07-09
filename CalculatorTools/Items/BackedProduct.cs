using CalculatorTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CalculatorTools.Items
{
    public class BackedProduct : ISerializableItem
    {
        private static int _currentId = 0;

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                if (_id >= _currentId) _currentId = _id + 1;
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private double _ryeFlourRatio;

        public double RyeFlourRatio
        {
            get { return _ryeFlourRatio; }
            set { _ryeFlourRatio = value; }
        }

        private double _wheatFlourRatio;

        public double WheatFlourRatio
        {
            get { return _wheatFlourRatio; }
            set { _wheatFlourRatio = value; }
        }

        private double _ryeFlourCount;

        [XmlIgnore]
        public double RyeFlourCount
        {
            get { return _ryeFlourCount; }
            set { _ryeFlourCount = value; }
        }

        private double _wheatFlourCount;

        [XmlIgnore]
        public double WheatFlourCount
        {
            get { return _wheatFlourCount; }
            set { _wheatFlourCount = value; }
        }

        public double FlourCount
        {
            get => RyeFlourCount + WheatFlourCount;
        }

        public double ExtraRyeFlour = 0.0d;
        public double ExtraWheatFlour = 0.0d;

        public static double EthanolWheatCoeff => 1.11;
        public static double EthanolRyeCoeff => 0.98;

        public static double AceticAcidWheatCoeff => 0.1;
        public static double AceticAcidRyeCoeff => 0.2;

        public static double FlourDustWheatCoeff => 0.024;
        public static double FlourDustRyeCoeff => 0.024;



        public BackedProduct()
        {
            _id = _currentId;
            _currentId++;
        }

        public BackedProduct(string name, double ryeFlourRatio, double wheatFlourRatio)
        {
            if (ryeFlourRatio + wheatFlourRatio != 100)
                throw new ArgumentOutOfRangeException("Неверное значение соотношения ржаной и пшеничной муки.");

            _name = name;
            _ryeFlourRatio = ryeFlourRatio;
            _wheatFlourRatio = wheatFlourRatio;
            _id = _currentId;
            _currentId++;
        }

        public void SetProducedCount(double count, double ryeExtra, double wheatExtra)
        {
            _ryeFlourCount = _ryeFlourRatio * count / 100;
            _wheatFlourCount = _wheatFlourRatio * count / 100;
            ExtraRyeFlour = ryeExtra;
            ExtraWheatFlour = wheatExtra;
        }

        public XElement ToXElement()
        {
            return new XElement("BackedProduct",
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("RyeFlourRatio", RyeFlourRatio),
                new XElement("WheatFlourRatio", WheatFlourRatio)
            );
        }

        public XElement ToResultsXElement()
        {
            XElement el = ToXElement();
            el.Add(new XElement("RyeFlourCount", RyeFlourCount));
            el.Add(new XElement("WheatFlourCount", WheatFlourCount));

            return el;
        }

        public override int GetHashCode()
        {
            int nameHash = Encoding.UTF8.GetBytes(Name).Sum(b => (int)b);
            return (int)(3 * nameHash + 24 * _ryeFlourRatio * _ryeFlourRatio + 3 * _wheatFlourRatio);
        }

        public static int GetHashCode(string name, double rye, double wheat)
        {
            int nameHash = Encoding.UTF8.GetBytes(name).Sum(b => (int)b);
            return (int)(3 * nameHash + 24 * rye * rye + 3 * wheat);
        }

        public override bool Equals(object obj)
        {
            BackedProduct prod = obj as BackedProduct;
            if (prod == null) return false;

            if (GetHashCode() == prod.GetHashCode()) return true;
            else return false;
        }
    }
}
