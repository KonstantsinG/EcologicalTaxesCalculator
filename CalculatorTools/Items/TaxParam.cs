using CalculatorTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CalculatorTools.Items
{
    public class TaxParam
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private double _limit;

        public double Limit
        {
            get => _limit;
            set => _limit = value;
        }

        private double _rate;

        public double Rate
        {
            get => _rate;
            set => _rate = value;
        }


        public TaxParam() { }

        public TaxParam(string name, double limit, double rate)
        {
            Name = name;
            Limit = limit;
            Rate = rate;
        }

        public XElement ToXElement()
        {
            return new XElement("TaxParam",
                                new XElement("Name", Name),
                                new XElement("Rate", Rate),
                                new XElement("Limit", Limit));
        }
    }
}
