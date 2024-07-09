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
    /// Логика взаимодействия для InputProductionEquipmentTable.xaml
    /// </summary>
    public partial class InputProductionEquipmentTable : UserControl, IThreatOfDataLoss
    {
        private Action _onBackClick;
        private Action _onSaveClick;
        private List<ProductionEquipment> _productionEquipment;
        private List<TableCell> _inputCells = new List<TableCell>();
        private bool _saved = false;

        public InputProductionEquipmentTable(Action onBackClick, Action onSaveClick)
        {
            InitializeComponent();
            _onBackClick = onBackClick;
            _onSaveClick = onSaveClick;

            _productionEquipment = new List<ProductionEquipment>();
            _productionEquipment.AddRange(DataCollection.Instance.Furnaces);
            _productionEquipment.AddRange(DataCollection.Instance.SteamBoilers);
            _productionEquipment.AddRange(DataCollection.Instance.HotWaterBoilers);

            foreach (TableRow row in ConstructTableRows(_productionEquipment))
                instancesContainer.Rows.Add(row);
        }

        private List<TableRow> ConstructTableRows(List<ProductionEquipment> props)
        {
            List<TableRow> rows = new List<TableRow>();
            TableRow row;
            TableCell cell;
            string eqName;

            foreach (ProductionEquipment p in props)
            {
                row = new TableRow();
                eqName = p.Place + ": " + p.TypeName + " «" + p.Name + "»";
                row.Cells.Add(TableInstancesFactory.ConstructCell(eqName, new string[] { "padding", "borders" }));

                cell = TableInstancesFactory.ConstructCell("0", new string[] { "padding", "borders" }, true);
                row.Cells.Add(cell); _inputCells.Add(cell);

                cell = TableInstancesFactory.ConstructCell("0", new string[] { "padding", "borders" }, true);
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
                _productionEquipment[i / 2].SetParams(values[i], values[i + 1]);
        }

        public bool IsReady()
        {
            if (_saved) return false;

            if (_inputCells.Any(c => ((c.Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text != "0")) return true;
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
