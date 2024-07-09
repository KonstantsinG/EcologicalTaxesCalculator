using CalculatorTools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTools.Items
{
    public class ReportsCollection
    {
        private string _year;

        public string Year
        {
            get => _year;
            set => _year = value;
        }

        private List<string> _monthReports = new List<string>();

        public List<string> MonthReports
        {
            get => _monthReports;
            set => _monthReports = value;
        }

        private List<string> _quartalReports = new List<string>();

        public List<string> QuartalReports
        {
            get => _quartalReports;
            set => _quartalReports = value;
        }

        private bool _yearReport;

        public bool YearReport
        {
            get => _yearReport;
            set => _yearReport = value;
        }

        public List<string> MonthReportsString
        {
            get
            {
                List<string> monthNames = new List<string>();

                foreach (string num in _monthReports)
                    monthNames.Add(EcologicalTaxesHandler.GetMonthString(num));

                return monthNames;
            }
        }

        public List<string> QuartalReportsString
        {
            get
            {
                List<string> quartalNames = new List<string>();

                foreach (string num in _quartalReports)
                    quartalNames.Add(EcologicalTaxesHandler.GetQuartalString(num));

                return quartalNames;
            }
        }

        
        public ReportsCollection(string year)
        {
            _year = year;
        }


        public void SortReports()
        {
            _monthReports.Sort((r1, r2) => int.Parse(r1).CompareTo(int.Parse(r2)));
            _quartalReports.Sort((r1, r2) => int.Parse(r1).CompareTo(int.Parse(r2)));
        }
    }
}
