using CalculatorTools.Interfaces;
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
    /// Логика взаимодействия для InputBackedProductsTable.xaml
    /// </summary>
    public partial class InputBackedProductsTable : UserControl, IThreatOfDataLoss
    {
        private Action _onBackClick;
        private Action _onSaveClick;
        private List<TableCell> _inputCells = new List<TableCell>();
        private TableCell[] _extraCells = new TableCell[2];
        private bool _saved = false;

        public InputBackedProductsTable(Action onBackClick, Action onSaveClick)
        {
            InitializeComponent();
            _onBackClick = onBackClick;
            _onSaveClick = onSaveClick;

            foreach (TableRow row in ConstructTableRows(DataCollection.Instance.BackedProducts))
                instancesContainer.Rows.Add(row);
        }

        private List<TableRow> ConstructTableRows(List<BackedProduct> props)
        {
            List<TableRow> rows = new List<TableRow>();
            TableRow row;
            TableCell cell;

            foreach (BackedProduct p in props)
            {
                row = new TableRow();
                row.Cells.Add(TableInstancesFactory.ConstructCell(p.Name, new string[] { "padding", "borders" }));
                cell = TableInstancesFactory.ConstructCell("0", new string[] { "padding", "borders" }, true);
                row.Cells.Add(cell); _inputCells.Add(cell);
                rows.Add(row);
            }

            rows.Add(TableInstancesFactory.ConstructEmptyRow(2)); row = new TableRow();

            row.Cells.Add(TableInstancesFactory.ConstructCell("Изделия из ржаной муки", new string[] { "padding", "borders" }));
            cell = TableInstancesFactory.ConstructCell("0", new string[] { "padding", "borders" }, true);
            row.Cells.Add(cell); _extraCells[0] = cell;
            rows.Add(row); row = new TableRow();

            row.Cells.Add(TableInstancesFactory.ConstructCell("Изделия из пшеничной муки", new string[] { "padding", "borders" }));
            cell = TableInstancesFactory.ConstructCell("0", new string[] { "padding", "borders" }, true);
            row.Cells.Add(cell); _extraCells[1] = cell;
            rows.Add(row);

            return rows;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            _onBackClick();
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _saved = true;
                SaveResults();
                _onSaveClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка ввода данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveResults()
        {
            List<double> values = new List<double>();
            List<BackedProduct> prods = DataCollection.Instance.BackedProducts;
            string data;

            string ryeExtra = ((_extraCells[0].Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text;
            if (ryeExtra.Contains(".")) ryeExtra = ryeExtra.Replace(".", ",");

            string wheatExtra = ((_extraCells[1].Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text;
            if (wheatExtra.Contains(".")) wheatExtra = wheatExtra.Replace(".", ",");

            foreach (TableCell cell in _inputCells)
            {
                data = ((cell.Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text;
                if (data.Contains(".")) data = data.Replace(".", ",");
                values.Add(double.Parse(data));
            }

            for (int i = 0; i < values.Count; i++)
                prods[i].SetProducedCount(values[i], double.Parse(ryeExtra), double.Parse(wheatExtra));
        }

        public bool IsReady()
        {
            if (_saved) return false;

            if (_extraCells.Any(c => ((c.Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text != "0")) return true;
            else if (_inputCells.Any(c => ((c.Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text != "0")) return true;
            else return false;
        }

        public void Save()
        {
            try
            {
                if (IsReady())
                {
                    SaveResults();
                    _onSaveClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка ввода данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
