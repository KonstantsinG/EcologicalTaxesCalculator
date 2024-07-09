using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTools.Interfaces
{
    public interface IThreatOfDataLoss
    {
        bool IsReady();

        void Save();
    }
}
