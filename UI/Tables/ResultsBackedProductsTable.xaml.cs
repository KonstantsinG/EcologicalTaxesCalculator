using CalculatorTools.Items;
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
    /// Логика взаимодействия для ResultsBackedProductsTable.xaml
    /// </summary>
    public partial class ResultsBackedProductsTable : UserControl
    {
        public ResultsBackedProductsTable(bool stored = false)
        {
            InitializeComponent();

            if (stored)
            {
                string path = XMLReader.GetPath(DataCollection.DataPath, "month", EcologicalTaxesHandler.CurrentDate, "backedProducts.xml");
                foreach (TableRow row in ConstructRows(XMLReader.ReadBackedProducts(path)))
                    instancesContainer.Rows.Add(row);
            }
            else
            {
                foreach (TableRow row in ConstructRows(DataCollection.Instance.BackedProducts))
                    instancesContainer.Rows.Add(row);
            }
        }

        private List<TableRow> ConstructRows(List<BackedProduct> prods)
        {
            List<TableRow> rows = new List<TableRow>();
            TableRow row;
            double ryeSum = 0, wheatSum = 0, allSum = 0; 
            
            foreach (BackedProduct bp in prods)
            {
                row = new TableRow();
                row.Cells.Add(TableInstancesFactory.ConstructCell(bp.Name, new string[] { "padding", "borders" }));
                row.Cells.Add(TableInstancesFactory.ConstructCell(bp.RyeFlourRatio.ToString(), new string[] { "padding", "borders" }));
                row.Cells.Add(TableInstancesFactory.ConstructCell(bp.WheatFlourRatio.ToString(), new string[] { "padding", "borders" }));
                row.Cells.Add(TableInstancesFactory.ConstructCell(bp.RyeFlourCount.ToString(), new string[] { "padding", "borders" }));
                row.Cells.Add(TableInstancesFactory.ConstructCell(bp.WheatFlourCount.ToString(), new string[] { "padding", "borders" }));
                row.Cells.Add(TableInstancesFactory.ConstructCell(bp.FlourCount.ToString(), new string[] { "padding", "borders" }));
                rows.Add(row);

                ryeSum += bp.RyeFlourCount;
                wheatSum += bp.WheatFlourCount;
                allSum += bp.FlourCount;
            }

            row = new TableRow();
            row.Cells.Add(TableInstancesFactory.ConstructCell("Прочие изделия", new string[] { "padding", "borders" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "padding", "borders" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell(prods[0].ExtraWheatFlour.ToString(), new string[] { "padding", "borders" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell(prods[0].ExtraRyeFlour.ToString(), new string[] { "padding", "borders" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell((prods[0].ExtraRyeFlour + prods[0].ExtraWheatFlour).ToString(), new string[] { "padding", "borders" }));
            rows.Add(row);
            ryeSum += prods[0].ExtraRyeFlour; wheatSum += prods[0].ExtraWheatFlour; allSum += prods[0].ExtraRyeFlour + prods[0].ExtraWheatFlour;

            rows.Add(TableInstancesFactory.ConstructEmptyRow(6));
            rows.Last().Cells.Last().BorderThickness = new Thickness(0, 1, 1, 0);
            row = new TableRow();
            row.Cells.Add(TableInstancesFactory.ConstructCell("Итого", new string[] { "padding", "borders", "bold", "bgAttention" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "borders", "bgAttention" } ));
            row.Cells.Add(TableInstancesFactory.ConstructCell("", new string[] { "borders", "bgAttention" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell(ryeSum.ToString(), new string[] { "padding", "borders", "bold", "bgAttention" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell(wheatSum.ToString(), new string[] { "padding", "borders", "bold", "bgAttention" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell(allSum.ToString(), new string[] { "padding", "borders", "bold", "bgAttention" }));
            rows.Add(row);

            return rows;
        }
    }
}
