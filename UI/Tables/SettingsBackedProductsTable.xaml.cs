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
    /// Логика взаимодействия для SettingsBackedProductsTable.xaml
    /// </summary>
    public partial class SettingsBackedProductsTable : UserControl
    {
        private TableRow _selectedRow = null;
        private TableRow _editingRow = null;
        private TableRow[] _addedRows = null;
        private BackedProduct _editingProduct = null;

        public SettingsBackedProductsTable()
        {
            InitializeComponent();
            FillTable();
        }

        private void FillTable()
        {
            List<BackedProduct> instances = DataCollection.Instance.BackedProducts;
            List<TableRow> rows = ConstructSettingsBackedProductsRows(instances, OnTableRowClicked);

            foreach (TableRow row in rows)
                instancesContainer.Rows.Add(row);
        }

        // Highlight row
        public void OnTableRowClicked(object sender, MouseButtonEventArgs e)
        {
            if (_editingRow != null) return;
            if (_selectedRow != null) _selectedRow.Background = null;

            _selectedRow = sender as TableRow;
            _selectedRow.Background = new SolidColorBrush(Colors.LightSkyBlue);
        }

        // Add new row -------------------------------
        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (_addedRows != null) return;

            TableRow[] newRows = ConstructSettingsBackedProductsAddRow(OnSaveAddedRowClicked, OnCancelAddedRowClicked);
            _addedRows = newRows;
            instancesContainer.Rows.Add(newRows[0]);
            instancesContainer.Rows.Add(newRows[1]);
        }

        // Save added row
        public void OnSaveAddedRowClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = ((_addedRows[0].Cells[0].Blocks.ElementAtOrDefault(0) as BlockUIContainer).Child as TextBox).Text;
                string ryeRatioString = ((_addedRows[0].Cells[1].Blocks.ElementAtOrDefault(0) as BlockUIContainer).Child as TextBox).Text;
                string wheatRatioString = ((_addedRows[0].Cells[2].Blocks.ElementAtOrDefault(0) as BlockUIContainer).Child as TextBox).Text;
                string[] strData = TableInstancesFactory.CheckForDots(ryeRatioString, wheatRatioString);

                double ryeRatio = double.Parse(strData[0]);
                double wheatRatio = double.Parse(strData[1]);

                BackedProduct product = new BackedProduct(name, ryeRatio, wheatRatio);
                DataCollection.AddBackedProduct(product);

                instancesContainer.Rows.Remove(_addedRows[0]);
                instancesContainer.Rows.Remove(_addedRows[1]);
                _addedRows = null;
                instancesContainer.Rows.Add(ConstructSettingsBackedProductsRows(product, OnTableRowClicked));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Save edited row
        public void OnSaveEditedRowClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = ((_editingRow.Cells[0].Blocks.ElementAtOrDefault(0) as BlockUIContainer).Child as TextBox).Text;
                string ryeRatioString = ((_editingRow.Cells[1].Blocks.ElementAtOrDefault(0) as BlockUIContainer).Child as TextBox).Text;
                string wheatRatioString = ((_editingRow.Cells[2].Blocks.ElementAtOrDefault(0) as BlockUIContainer).Child as TextBox).Text;
                string[] strData = TableInstancesFactory.CheckForDots(ryeRatioString, wheatRatioString);

                if (double.Parse(strData[0]) + double.Parse(strData[1]) != 100)
                    throw new ArgumentException("Соотношение пшеничной и ржаной муки должны в сумме давать 100%");

                DataCollection.UpdateBackedProduct(_editingProduct, new string[] { name, strData[0], strData[1] });
                TableInstancesFactory.ReplaceTextBoxToParagraph(ref _selectedRow);
                
                _editingRow.Background = null;
                if (_selectedRow != null) _selectedRow.Background = null;

                instancesContainer.Rows.Remove(_addedRows[1]);
                _selectedRow = null;
                _addedRows = null;
                _editingRow = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка изменения данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Cancel added row
        public void OnCancelAddedRowClicked(object sender, RoutedEventArgs e)
        {
            instancesContainer.Rows.Remove(_addedRows[0]);
            instancesContainer.Rows.Remove(_addedRows[1]);
            _addedRows = null;
        }

        // Cancel edited row
        public void OnCancelEditedRowClicked(object sender, RoutedEventArgs e)
        {
            instancesContainer.Rows.Remove(_addedRows[1]);
            int idx = instancesContainer.Rows.IndexOf(_editingRow);
            instancesContainer.Rows.Remove(_editingRow);
            instancesContainer.Rows.Insert(idx, ConstructSettingsBackedProductsRows(_editingProduct, OnTableRowClicked));

            _editingRow.Background = null;
            if (_selectedRow != null) _selectedRow.Background = null;

            _addedRows = null;
            _selectedRow = null;
            _editingRow = null;
        }

        // Edit row ------------------------
        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            if (_selectedRow != null && _addedRows == null)
            {
                _editingProduct = DataCollection.FindBackedProduct(ExtractBackedProductData(_selectedRow));
                _editingRow = _selectedRow;
                TableInstancesFactory.ReplaceParagraphToTextBox(ref _selectedRow);
                int idx = instancesContainer.Rows.IndexOf(_selectedRow);
                TableRow toolsRow = TableInstancesFactory.ConstructSaveRow(0, 1, OnSaveEditedRowClicked, OnCancelEditedRowClicked);
                _addedRows = new TableRow[] { null, toolsRow };
                instancesContainer.Rows.Insert(idx + 1, toolsRow);
            }
        }

        // Delete row ----------------------
        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (_selectedRow != null && _addedRows == null)
            {
                BackedProduct product = DataCollection.FindBackedProduct(ExtractBackedProductData(_selectedRow));
                DataCollection.RemoveBackedProduct(product);
                instancesContainer.Rows.Remove(_selectedRow);
                _selectedRow = null;
            }
        }



        public static List<TableRow> ConstructSettingsBackedProductsRows(List<BackedProduct> instances, MouseButtonEventHandler rowClickHandler)
        {
            List<TableRow> rows = new List<TableRow>();
            TableRow row;

            foreach (BackedProduct i in instances)
            {
                row = new TableRow();

                row.Cells.Add(TableInstancesFactory.ConstructCell(i.Name, new string[] { "padding" }));
                row.Cells.Add(TableInstancesFactory.ConstructCell(i.RyeFlourRatio.ToString(), new string[] { "padding" }));
                row.Cells.Add(TableInstancesFactory.ConstructCell(i.WheatFlourRatio.ToString(), new string[] { "padding" }));
                row.MouseDown += rowClickHandler;

                rows.Add(row);
            }

            return rows;
        }

        private TableRow ConstructSettingsBackedProductsRows(BackedProduct instance, MouseButtonEventHandler rowClickHandler)
        {
            TableRow row;

            row = new TableRow();

            row.Cells.Add(TableInstancesFactory.ConstructCell(instance.Name, new string[] { "padding" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell(instance.RyeFlourRatio.ToString(), new string[] { "padding" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell(instance.WheatFlourRatio.ToString(), new string[] { "padding" }));
            row.MouseDown += rowClickHandler;

            return row;
        }

        private TableRow[] ConstructSettingsBackedProductsAddRow(RoutedEventHandler doneClickHandler, RoutedEventHandler cancelClickHandler)
        {
            TableRow[] rows = new TableRow[2];
            TableRow row = new TableRow();

            row.Cells.Add(TableInstancesFactory.ConstructCell("", null, true));
            row.Cells.Add(TableInstancesFactory.ConstructCell("", null, true));
            row.Cells.Add(TableInstancesFactory.ConstructCell("", null, true));

            rows[0] = row;
            rows[1] = TableInstancesFactory.ConstructSaveRow(0, 1, doneClickHandler, cancelClickHandler);

            return rows;
        }

        private string[] ExtractBackedProductData(TableRow row)
        {
            string[] data = new string[3];

            data[0] = ((row.Cells[0].Blocks.ElementAt(0) as Paragraph).Inlines.ElementAt(0) as Run).Text;
            data[1] = ((row.Cells[1].Blocks.ElementAt(0) as Paragraph).Inlines.ElementAt(0) as Run).Text;
            data[2] = ((row.Cells[2].Blocks.ElementAt(0) as Paragraph).Inlines.ElementAt(0) as Run).Text;

            return data;
        }
    }
}
