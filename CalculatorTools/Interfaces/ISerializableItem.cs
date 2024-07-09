using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CalculatorTools.Interfaces
{
    public interface ISerializableItem
    {
        int Id { get; set; }

        XElement ToXElement();

        XElement ToResultsXElement();
    }
}
