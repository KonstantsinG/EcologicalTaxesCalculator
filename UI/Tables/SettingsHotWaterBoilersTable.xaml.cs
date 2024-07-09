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
    /// Логика взаимодействия для SettingsHotWaterBoilersTable.xaml
    /// </summary>
    public partial class SettingsHotWaterBoilersTable : UserControl
    {
        private Table _table;
        private List<TableCell> _selectedColumn = new List<TableCell>();
        private List<TableCell> _editingColumn = new List<TableCell>();
        private int _selectedColumnIndex = 0;
        private bool _editing = false;
        private HotWaterBoiler _editingBoiler = null;
        private TableRow _toolsRow = null;

        public SettingsHotWaterBoilersTable()
        {
            InitializeComponent();

            _table = ConstructSettingsBoilersTable(OnTableColumnClick);
            double width = 0;
            foreach (TableColumn col in _table.Columns)
                width += col.Width.Value;
            tableRoot.PageWidth = width + 50;
            tableRoot.Blocks.Add(_table);
        }

        private void OnTableColumnClick(object sender, MouseButtonEventArgs e)
        {
            TableCell cell = (TableCell)sender;
            TableRow row = (TableRow)cell.Parent;
            _selectedColumnIndex = row.Cells.IndexOf(cell);
            TableRowGroup group = (TableRowGroup)row.Parent;
            Table table = (Table)group.Parent;

            ResetHighlightings();
            TableCell nameCell = table.RowGroups[0].Rows[1].Cells[_selectedColumnIndex];
            nameCell.Background = new SolidColorBrush(Colors.LightSkyBlue);
            _selectedColumn = new List<TableCell>() { nameCell };

            foreach (TableRow r in group.Rows)
            {
                r.Cells[_selectedColumnIndex].Background = new SolidColorBrush(Colors.LightSkyBlue);
                _selectedColumn.Add(r.Cells[_selectedColumnIndex]);
            }
        }

        private void ResetHighlightings()
        {
            foreach (TableCell cell in _table.RowGroups[0].Rows[1].Cells)
            {
                if (cell.Background != null)
                    cell.Background = null;
            }

            foreach (TableRow row in _table.RowGroups[1].Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    if (cell.Background != null)
                        cell.Background = null;
                }
            }
        }

        public void OnCancelEditedRowClicked(object sender, RoutedEventArgs e)
        {
            _editing = false;
            TableCell cell;
            string[] data = ExtractColumnData(_editingColumn, true);

            if (_editingBoiler.Name != data[0])
                ((_editingColumn[0].Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text = _editingBoiler.Name;
            if (_editingBoiler.Place != data[1])
                ((_editingColumn[1].Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text = _editingBoiler.Place;
            if (_editingBoiler.FuelType != data[2])
                ((_editingColumn[2].Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text = _editingBoiler.FuelType;
            if (_editingBoiler.MinCombustionHeat.ToString() != data[3])
                ((_editingColumn[3].Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text = _editingBoiler.MinCombustionHeat.ToString();
            if (_editingBoiler.FuelConsumptionOnMaxPower.ToString() != data[4])
                ((_editingColumn[4].Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text = _editingBoiler.FuelConsumptionOnMaxPower.ToString();
            if (_editingBoiler.TheoreticalVolumeOfDryFlueGases.ToString() != data[5])
                ((_editingColumn[5].Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text = _editingBoiler.TheoreticalVolumeOfDryFlueGases.ToString();

            for (int i = 0; i < _editingColumn.Count; i++)
            {
                cell = _editingColumn[i];
                TableInstancesFactory.ReplaceTextBoxToParagraph(ref cell);
            }

            _table.RowGroups[1].Rows.Remove(_toolsRow);
            _editingColumn.Clear();
            _selectedColumn.Clear();
            ResetHighlightings();
        }

        public void OnCancelAddedRowClicked(object sender, RoutedEventArgs e)
        {
            _editing = false;

            for (int i = 0; i < 7; i++)
                _table.RowGroups[1].Rows[i].Cells.RemoveAt(_table.RowGroups[1].Rows[i].Cells.Count - 1);

            tableRoot.PageWidth -= 150;
            _table.RowGroups[0].Rows[0].Cells[0].ColumnSpan -= 1;

            _table.RowGroups[0].Rows[1].Cells.RemoveAt(_table.RowGroups[0].Rows[1].Cells.Count - 1);

            _table.RowGroups[1].Rows.Remove(_toolsRow);
            _selectedColumn.Clear();
            _editingColumn.Clear();
            ResetHighlightings();
        }

        public void OnSaveEditedRowClicked(object sender, RoutedEventArgs e)
        {
            TableCell cell;
            _editing = false;
            string[] data = ExtractColumnData(_editingColumn, true);
            data = TableInstancesFactory.CheckForDots(data);

            try
            {
                DataCollection.UpdateHotWaterBoiler(_editingBoiler, data);

                for (int i = 0; i < _editingColumn.Count; i++)
                {
                    cell = _editingColumn[i];
                    TableInstancesFactory.ReplaceTextBoxToParagraph(ref cell);
                }

                _table.RowGroups[1].Rows.Remove(_toolsRow);
                _editingColumn.Clear();
                _selectedColumn.Clear();
                ResetHighlightings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка изменения данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OnSaveAddedRowClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                _editing = false;
                string[] data = ExtractColumnData(_editingColumn, true);
                data = TableInstancesFactory.CheckForDots(data);
                HotWaterBoiler boiler = new HotWaterBoiler(data[0], data[7], data[1], double.Parse(data[2]), double.Parse(data[5]), double.Parse(data[6]), double.Parse(data[3]), double.Parse(data[4]));
                DataCollection.AddHotWaterBoiler(boiler);
                TableCell cell;

                for (int i = 0; i < _editingColumn.Count; i++)
                {
                    cell = _editingColumn[i];
                    if (((cell.Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text.Contains("."))
                        ((cell.Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text = ((cell.Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text.Replace(".", ",");
                    TableInstancesFactory.ReplaceTextBoxToParagraph(ref cell);
                }

                _table.RowGroups[1].Rows.Remove(_toolsRow);
                _selectedColumn.Clear();
                _editingColumn.Clear();
                ResetHighlightings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            if (_selectedColumn.Count > 0 && !_editing)
            {
                TableCell cell;
                _editing = true;
                _editingColumn = new List<TableCell>(_selectedColumn);
                _editingBoiler = DataCollection.FindHotWaterBoiler(ExtractColumnData(_editingColumn));

                for (int i = 0; i < _selectedColumn.Count; i++)
                {
                    cell = _selectedColumn[i];
                    TableInstancesFactory.ReplaceParagraphToTextBox(ref cell);
                }

                int colsCount = DataCollection.Instance.HotWaterBoilers.Count + 2;
                _toolsRow = TableInstancesFactory.ConstructSaveRow(_selectedColumnIndex - 1, colsCount - _selectedColumnIndex - 1, OnSaveEditedRowClicked, OnCancelEditedRowClicked);
                _table.RowGroups[1].Rows.Add(_toolsRow);
            }
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (_editing == false)
            {
                TableCell cell;
                _editing = true;

                for (int i = 0; i < 7; i++)
                {
                    cell = TableInstancesFactory.ConstructCell("", new string[] { "padding" }, true);
                    cell.BorderThickness = new Thickness(1, 1, 0, 0); cell.BorderBrush = new SolidColorBrush(Colors.Black);
                    cell.MouseDown += OnTableColumnClick;
                    _editingColumn.Add(cell);
                    _table.RowGroups[1].Rows[i].Cells.Add(cell);
                }

                tableRoot.PageWidth += 150;
                _table.RowGroups[0].Rows[0].Cells[0].ColumnSpan += 1;

                cell = new TableCell(); BlockUIContainer ui = new BlockUIContainer(); TextBox tBox = new TextBox();
                _editingColumn.Add(cell);
                tBox.Text = ""; tBox.FontWeight = FontWeights.Bold;
                ui.Child = tBox; cell.Blocks.Add(ui); cell.Padding = new Thickness(5); cell.BorderThickness = new Thickness(1, 1, 0, 0); cell.BorderBrush = new SolidColorBrush(Colors.Black);
                _table.RowGroups[0].Rows[1].Cells.Add(cell);

                int colsCount = DataCollection.Instance.HotWaterBoilers.Count + 3;
                _toolsRow = TableInstancesFactory.ConstructSaveRow(colsCount - 2, 0, OnSaveAddedRowClicked, OnCancelAddedRowClicked);
                _table.RowGroups[1].Rows.Add(_toolsRow);
            }
        }

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (_editing == false && _selectedColumn.Count > 0)
            {
                HotWaterBoiler boiler = DataCollection.FindHotWaterBoiler(ExtractColumnData(_selectedColumn));
                DataCollection.RemoveHotWaterBoiler(boiler);

                for (int i = 0; i < 7; i++)
                    _table.RowGroups[1].Rows[i].Cells.RemoveAt(_selectedColumnIndex);

                tableRoot.PageWidth -= 150;
                _table.RowGroups[0].Rows[0].Cells[0].ColumnSpan -= 1;

                _table.RowGroups[0].Rows[1].Cells.RemoveAt(_selectedColumnIndex);

                _selectedColumn.Clear();
                _editingColumn.Clear();
                ResetHighlightings();
            }
        }



        private TableRow ConstructSettingsBoilerRow(List<HotWaterBoiler> boilers, string propName, string rowName, string extraValue, MouseButtonEventHandler columnClickHandler)
        {
            TableRow row = new TableRow();
            TableCell cell;
            var prop = typeof(HotWaterBoiler).GetProperty(propName);
            row.Cells.Add(TableInstancesFactory.ConstructCell(rowName, new string[] { "padding", "borders" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell(extraValue, new string[] { "padding", "borders", "center" }));

            if (prop != null)
            {
                foreach (HotWaterBoiler b in boilers)
                {
                    cell = TableInstancesFactory.ConstructCell(prop.GetValue(b).ToString(), new string[] { "padding", "borders" });
                    cell.MouseDown += columnClickHandler;
                    if (boilers.Last() == b) cell.BorderThickness = new Thickness(0, 1, 0, 0);
                    row.Cells.Add(cell);
                }
            }

            return row;
        }

        private TableRowGroup ConstructSettingsBoilersGroup(List<HotWaterBoiler> boilers, MouseButtonEventHandler columnClickHandler)
        {
            TableRowGroup group = new TableRowGroup();

            group.Rows.Add(ConstructSettingsBoilerRow(boilers, "Place", "Местоположение", " - ", columnClickHandler));
            group.Rows.Add(ConstructSettingsBoilerRow(boilers, "FuelType", "Вид топлива", " - ", columnClickHandler));
            group.Rows.Add(ConstructSettingsBoilerRow(boilers, "MinCombustionHeat", "Низшая теплота сгорания", "МДж/м^3", columnClickHandler));
            group.Rows.Add(ConstructSettingsBoilerRow(boilers, "RatedLoad", "Нагрузка номинальная", "МВт", columnClickHandler));
            group.Rows.Add(ConstructSettingsBoilerRow(boilers, "EfficiencyGrossOfBoiler", "КПД 'Брутто' котла", "%", columnClickHandler));
            group.Rows.Add(ConstructSettingsBoilerRow(boilers, "FuelConsumptionOnMaxPower", "Расход топлива на макс. нагрузке", "м^3/с", columnClickHandler));
            group.Rows.Add(ConstructSettingsBoilerRow(boilers, "TheoreticalVolumeOfDryFlueGases", "Теоретический объём сухих дымовых газов", "м^3", columnClickHandler));

            return group;
        }

        private Table ConstructSettingsBoilersTable(MouseButtonEventHandler columnClickHandler)
        {
            Table table = new Table();
            TableRowGroup rowGroup = new TableRowGroup();
            TableRow row = new TableRow();
            List<HotWaterBoiler> boilers = DataCollection.Instance.HotWaterBoilers;

            table.BorderBrush = new SolidColorBrush(Colors.Black);
            table.BorderThickness = new Thickness(1);
            table.CellSpacing = 0;

            TableColumn col = new TableColumn();
            col.Width = new GridLength(300);
            table.Columns.Add(col);
            col = new TableColumn();
            col.Width = new GridLength(100);
            table.Columns.Add(col);
            for (int i = 0; i < boilers.Count; i++)
            {
                col = new TableColumn();
                col.Width = new GridLength(150);
                table.Columns.Add(col);
            }

            TableCell cell = new TableCell() { ColumnSpan = boilers.Count + 2, Background = new SolidColorBrush(Colors.LightGray), TextAlignment = TextAlignment.Center, Padding = new Thickness(10) };
            Paragraph p = new Paragraph();
            Run r = new Run() { FontWeight = FontWeights.Bold, Text = "Котлы водонагревательные", FontSize = 20 };
            p.Inlines.Add(r);
            cell.Blocks.Add(p);
            row.Cells.Add(cell);

            rowGroup.Rows.Add(row);
            row = new TableRow();

            row.Cells.Add(TableInstancesFactory.ConstructCell("Наименование величин", new string[] { "bold", "padding", "borders" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell("Ед. изм.", new string[] { "bold", "padding", "borders" }));

            foreach (HotWaterBoiler b in boilers)
            {
                cell = TableInstancesFactory.ConstructCell(b.Name, new string[] { "bold", "padding", "borders" });
                if (boilers.Last() == b) cell.BorderThickness = new Thickness(0, 1, 0, 0);
                row.Cells.Add(cell);
            }
            rowGroup.Rows.Add(row);

            table.RowGroups.Add(rowGroup);
            table.RowGroups.Add(ConstructSettingsBoilersGroup(boilers, columnClickHandler));

            return table;
        }

        private string[] ExtractColumnData(List<TableCell> column, bool tBox = false)
        {
            string[] data = new string[column.Count];

            if (tBox)
            {
                for (int i = 0; i < column.Count; i++)
                    data[i] = ((column[i].Blocks.ElementAt(0) as BlockUIContainer).Child as TextBox).Text;
            }
            else
            {
                for (int i = 0; i < column.Count; i++)
                    data[i] = ((column[i].Blocks.ElementAt(0) as Paragraph).Inlines.ElementAt(0) as Run).Text;
            }

            return data;
        }
    }
}
