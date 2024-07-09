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
    /// Логика взаимодействия для InputTaxParamsTable.xaml
    /// </summary>
    public partial class InputTaxParamsTable : UserControl, IThreatOfDataLoss
    {
        private Action _onBackClick;
        private Action _onSaveClick;
        private List<TaxParam> _taxParams;
        private List<TableCell> _inputCells = new List<TableCell>();
        private bool _saved = false;

        public InputTaxParamsTable(Action onBackClick, Action onSaveClick)
        {
            InitializeComponent();
            _onBackClick = onBackClick;
            _onSaveClick = onSaveClick;

            _taxParams = DataCollection.Instance.TaxParams;

            foreach (TableRow row in ConstructTableRows(_taxParams))
                instancesContainer.Rows.Add(row);
        }

        private List<TableRow> ConstructTableRows(List<TaxParam> props)
        {
            List<TableRow> rows = new List<TableRow>();
            TableRow row;
            TableCell cell;

            foreach (TaxParam p in props)
            {
                row = new TableRow();
                row.Cells.Add(TableInstancesFactory.ConstructCell(p.Name, new string[] { "padding", "borders" }));

                cell = TableInstancesFactory.ConstructCell(p.Rate.ToString(), new string[] { "padding", "borders" }, true);
                row.Cells.Add(cell); _inputCells.Add(cell);

                cell = TableInstancesFactory.ConstructCell(p.Limit.ToString(), new string[] { "padding", "borders" }, true);
                row.Cells.Add(cell); _inputCells.Add(cell);
                rows.Add(row);
            }

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
            string data;

            foreach (TableCell cell in _inputCells)
            {
                data = ((cell.Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text;
                if (data.Contains(".")) data = data.Replace(".", ",");
                values.Add(double.Parse(data));
            }

            for (int i = 0; i < values.Count; i += 2)
            {
                _taxParams[i / 2].Rate = values[i];
                _taxParams[i / 2].Limit = values[i + 1];
            }
            XMLWriter.UpdateTaxParams(_taxParams);
        }

        public bool IsReady()
        {
            if (_saved) return false;

            List<double> taxData = new List<double>();
            for (int i = 0; i < DataCollection.Instance.TaxParams.Count; i++)
            {
                taxData.Add(DataCollection.Instance.TaxRates[i]);
                taxData.Add(DataCollection.Instance.TaxLimits[i]);
            }

            for (int i = 0; i < taxData.Count; i++)
            {
                if (((_inputCells[i].Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text != taxData[i].ToString())
                    return true; 
            }

            return false;
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
