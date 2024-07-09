using CalculatorTools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.Tables
{
    /// <summary>
    /// Логика взаимодействия для ResultsEcologicalTaxesTable.xaml
    /// </summary>
    public partial class ResultsEcologicalTaxesTable : UserControl
    {
        public ResultsEcologicalTaxesTable(EcologicalTaxesHandler.ReportTypes type, string title, bool stored = false)
        {
            InitializeComponent();
            ConstructTable(type, stored);
            tableTitle.Text = "Экологический налог за " + title;
        }

        private void ConstructTable(EcologicalTaxesHandler.ReportTypes type, bool stored)
        {
            double[] limits = stored ? EcologicalTaxesHandler.GetStoredTaxLimits() : DataCollection.Instance.TaxLimits;
            double[] taxes = stored ? EcologicalTaxesHandler.GetStoredTaxRates() : DataCollection.Instance.TaxRates;
            double[] taxesCoeffs = EcologicalTaxesHandler.TaxesCoeffs;
            double[] reducedTaxes = EcologicalTaxesHandler.ReducedTaxes;
            double[] currentOuts = null; // --- (non calculable)
            double[] yearOuts = null; // --------- (non calculable)

            if (type == EcologicalTaxesHandler.ReportTypes.Month)
            {
                if(stored) currentOuts = EcologicalTaxesHandler.GetStoredMonthOut();
                else currentOuts = EcologicalTaxesHandler.GetCurrentOut();
                yearOuts = EcologicalTaxesHandler.GetYearOut();
            }
            else if (type == EcologicalTaxesHandler.ReportTypes.Quartal)
            {
                currentOuts = EcologicalTaxesHandler.GetQuartalOut();
                yearOuts = currentOuts;
            }
            else if (type == EcologicalTaxesHandler.ReportTypes.Year)
            {
                currentOuts = EcologicalTaxesHandler.GetYearOut();
                yearOuts = currentOuts;
            }

            double[] inLimits = new double[7]; // ---------------------------
            double[] overLimits = new double[7]; // -------------------------
            double[] taxSumInLimit = new double[7]; // ---------------------- 
            double[] taxSumOverLimit = new double[7]; // --------------------
            double[] resTaxes = new double[7];

            double[] classesSums;
            double[] shortTaxes;
            double[] shortSumInLimit;

            for (int i = 0; i < 7; i++)
            {
                if (i == 3) // for Manganese and etc...
                {
                    table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(limits[i].ToString(), new string[] { "padding", "borders" }));
                    for (int j = 0; j < 5; j++) table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
                    table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(taxes[i].ToString(), new string[] { "padding", "borders" }));
                    for (int j = 0; j < 5; j++) table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
                    continue;
                }

                table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(limits[i].ToString(), new string[] { "padding", "borders" }));
                // --------------------
                table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(yearOuts[i].ToString(), new string[] { "padding", "borders" }));
                table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell((yearOuts[i] > limits[i] ? limits[i] : yearOuts[i]).ToString(), new string[] { "padding", "borders" }));
                table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell((yearOuts[i] > limits[i] ? yearOuts[i] - limits[i] : 0).ToString(), new string[] { "padding", "borders" }));
                // --------------------

                if (type == EcologicalTaxesHandler.ReportTypes.Month)
                {
                    inLimits[i] = currentOuts[i] > limits[i] ? limits[i] : currentOuts[i];
                    table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(inLimits[i].ToString(), new string[] { "padding", "borders" }));
                    overLimits[i] = currentOuts[i] > limits[i] ? currentOuts[i] - limits[i] : 0;
                    table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(overLimits[i].ToString(), new string[] { "padding", "borders" }));
                }
                else if (type == EcologicalTaxesHandler.ReportTypes.Quartal || type == EcologicalTaxesHandler.ReportTypes.Year)
                {
                    inLimits[i] = yearOuts[i] > limits[i] ? limits[i] : yearOuts[i];
                    table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(" - ", new string[] { "padding", "borders" }));
                    overLimits[i] = yearOuts[i] > limits[i] ? yearOuts[i] - limits[i] : 0;
                    table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(" - ", new string[] { "padding", "borders" }));
                }

                table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(taxes[i].ToString(), new string[] { "padding", "borders" }));
                table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(taxesCoeffs[i].ToString(), new string[] { "padding", "borders" }));

                taxSumInLimit[i] = Math.Round(inLimits[i] * taxes[i] * taxesCoeffs[i], 2);
                table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(taxSumInLimit[i].ToString(), new string[] { "padding", "borders" }));
                table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(reducedTaxes[i].ToString(), new string[] { "padding", "borders" }));
                taxSumOverLimit[i] = Math.Round(overLimits[i] * taxes[i] * taxesCoeffs[i] * 15, 2);
                table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(taxSumOverLimit[i].ToString(), new string[] { "padding", "borders" }));
                resTaxes[i] = Math.Round(taxSumInLimit[i] - reducedTaxes[i] + taxSumOverLimit[i], 2);
                table.RowGroups[1].Rows.ElementAt(i).Cells.Add(TableInstancesFactory.ConstructCell(resTaxes[i].ToString(), new string[] { "padding", "borders" }));
            }

            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell(limits.Sum().ToString(), new string[] { "padding", "borders", "bold" }));
            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell(yearOuts.Sum().ToString(), new string[] { "padding", "borders", "bold" }));

            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell("X", new string[] { "padding", "borders", "bold", "center" }));
            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell("X", new string[] { "padding", "borders", "bold", "center" }));

            if (type == EcologicalTaxesHandler.ReportTypes.Month)
                table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell(currentOuts.Sum().ToString(), new string[] { "padding", "borders", "bold" }));
            else if (type == EcologicalTaxesHandler.ReportTypes.Quartal || type == EcologicalTaxesHandler.ReportTypes.Year)
                table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell("X", new string[] { "padding", "borders", "bold", "center" }));

            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell("X", new string[] { "padding", "borders", "bold", "center" }));
            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell("X", new string[] { "padding", "borders", "bold", "center" }));

            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell(taxesCoeffs.Sum().ToString(), new string[] { "padding", "borders", "bold" }));
            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell(taxSumInLimit.Sum().ToString(), new string[] { "padding", "borders", "bold" }));
            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell(reducedTaxes.Sum().ToString(), new string[] { "padding", "borders", "bold" }));
            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell(taxSumOverLimit.Sum().ToString(), new string[] { "padding", "borders", "bold" }));
            table.RowGroups[2].Rows.ElementAt(0).Cells.Add(TableInstancesFactory.ConstructCell(resTaxes.Sum().ToString(), new string[] { "padding", "borders", "bold" }));



            // ------- dow ln 3 -
            classesSums = new double[]
            {
                inLimits[0] + inLimits[3],
                inLimits[1] + inLimits[2] + inLimits[5],
                inLimits[4] + inLimits[6]
            };
            shortTaxes = new double[] { 1327.27, 438.77, 218.02 };
            shortSumInLimit = new double[]
            {
                Math.Round(classesSums[0] * shortTaxes[0], 2),
                Math.Round(classesSums[1] * shortTaxes[1], 2),
                Math.Round(classesSums[2] * shortTaxes[2], 2)
            };
            ConstructResultsRows(classesSums, shortTaxes, shortSumInLimit);
        }

        private void ConstructResultsRows(double[] classesSums, double[] shortTaxes, double[] shortSumsInLimit)
        {
            for (int i = 0; i < 3; i++)
            {
                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));

                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell(classesSums[i].ToString(), new string[] { "padding", "borders" }));
                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell(shortTaxes[i].ToString(), new string[] { "padding", "borders" }));
                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell(shortSumsInLimit[i].ToString(), new string[] { "padding", "borders" }));

                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
                table.RowGroups[2].Rows.ElementAt(i + 3).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
            }

            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));

            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell(classesSums.Sum().ToString(), new string[] { "padding", "borders", "bold" }));

            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));

            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell(shortSumsInLimit.Sum().ToString(), new string[] { "padding", "borders", "bold" }));

            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
            table.RowGroups[2].Rows.ElementAt(6).Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
        }
    }
}
