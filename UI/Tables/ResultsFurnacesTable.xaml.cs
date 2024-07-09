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
    /// Логика взаимодействия для ResultsFurnacesTable.xaml
    /// </summary>
    public partial class ResultsFurnacesTable : UserControl
    {
        public ResultsFurnacesTable()
        {
            InitializeComponent();

            Table table = ConstructSettingsFurnacesTable(false);
            tableRoot.Blocks.Add(table);

            double sumWidth = 50;
            foreach (TableColumn col in table.Columns) sumWidth += col.Width.Value;
            tableRoot.PageWidth = sumWidth;
        }

        public ResultsFurnacesTable(bool stored)
        {
            InitializeComponent();

            Table table = ConstructSettingsFurnacesTable(stored);
            tableRoot.Blocks.Add(table);

            double sumWidth = 50;
            foreach (TableColumn col in table.Columns) sumWidth += col.Width.Value;
            tableRoot.PageWidth = sumWidth;
        }

        private TableRow ConstructSettingsFurnaceRow(List<Furnace> furns, string propName, string rowName, string extraValue)
        {
            TableRow row = new TableRow();
            TableCell cell;
            var prop = typeof(Furnace).GetProperty(propName);
            row.Cells.Add(TableInstancesFactory.ConstructCell(rowName, new string[] { "padding", "borders" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell(extraValue, new string[] { "padding", "borders", "center" }));

            if (prop != null)
            {
                foreach (Furnace f in furns)
                {
                    cell = TableInstancesFactory.ConstructCell(prop.GetValue(f).ToString(), new string[] { "padding", "borders" });
                    if (furns.Last() == f) cell.BorderThickness = new Thickness(0, 1, 0, 0);
                    row.Cells.Add(cell);
                }
            }

            return row;
        }

        private TableRow[] ConstructDeviation(string text, int colsCount)
        {
            TableRow[] rows = new TableRow[2];
            rows[0] = TableInstancesFactory.ConstructEmptyRow(colsCount);
            rows[1] = TableInstancesFactory.ConstructEmptyRow(colsCount);
            ((rows[1].Cells[0].Blocks.ElementAt(0) as Paragraph).Inlines.ElementAt(0) as Run).Text = text;
            ((rows[1].Cells[0].Blocks.ElementAt(0) as Paragraph).Inlines.ElementAt(0) as Run).FontWeight = FontWeights.Bold;

            return rows;
        }

        private TableRowGroup ConstructSettingsFurnacesGroup(List<Furnace> furns)
        {
            TableRowGroup group = new TableRowGroup();
            TableRow row;

            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "Place", "Местоположение", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "FuelType", "Вид топлива", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "MinCombustionHeat", "Низшая теплота сгорания", "МДж/м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "WorkingTimePerYear", "Число часов работы/год", "ч."));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "FuelConsumptionOnMaxPower", "Расход топлива на макс. нагрузке", "м^3/с"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "FuelConsumptionPerYear", "Годовой расход топлива", "тыс.м^3/год"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "FuelConsumptionOnMeanPower", "Расход топлива при средней нагрузке", "м^3/с"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "TheoreticalVolumeOfDryFlueGases", "Теоретический объём сухих дымовых газов", "м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "VolumeOfDryFlueGases", "Объём сухих дымовых газов", "м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "StrangeEmptyField", " - Наименование параметра отсутствует -", "тыс.м^3/год"));

            TableRow[] rows = ConstructDeviation("Расчёт выбросов углерода оксида", furns.Count + 2);
            group.Rows.Add(rows[0]); group.Rows.Add(rows[1]);

            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "HeatLossDueToChemicalIncompleteCombustionOfFuel_ForGross", "Потери теплоты при хим. неполн. сгорания топлива (валовые)", "%"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "HeatLossDueToChemicalIncompleteCombustionOfFuel_ForMax", "Потери теплоты при хим. неполн. сгорания топлива (макс.)", "%"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "HeatLossDueToChemicalIncompleteCombustionOfFuel_Coeff", "К-т, учит. долю потери теплоты, вследствии хим. неполн. сгорания", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "CarbonOxidOut_ForGross", "Выход углерода оксида (валовый)", "г/м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "CarbonOxidOut_ForMax", "Выход углерода оксида (макс.)", "г/м^3"));
            row = ConstructSettingsFurnaceRow(furns, "CarbonOxidGrossOut", "Валовый выброс углерода оксида", "т/год");
            foreach (TableCell cell in row.Cells) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow); group.Rows.Add(row);
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "CarbonOxidMaxCount", "Макс. количество углерода оксида", "г/с"));

            rows = ConstructDeviation("Расчёт выбросов азота оксидов", furns.Count + 2);
            group.Rows.Add(rows[0]); group.Rows.Add(rows[1]);

            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "NitrogenOxidSpecificEmissionString", "Удельный выброс азота оксидов", "г/МДж")); // --------------------------
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "NitrogenOxidSpecificMeanEmissionString", "Средний удельный выброс азота оксидов", "г/МДж")); // --------------
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "BurnerConstructionCoeff", "К-т, учит. конструкцию горелки", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "AirTemperatureForBurnerCoeff", "К-т, учит. температуру воздуха, подаваемого для горения", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "FlueGasRecirculationCoeff", "К-т, учит. влияние рециркуляции дымовых газов", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "SteppedAirInputIntoCombustionChamberCoeff", "К-т, учит. ступенчатый ввод воздуха в топочную камеру", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "NitrogenOxidMaxOnNO2", "Макс. количество азота оксидов в пересчёте на NO2", "г/с"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "NitrogetOxidsGrossOut", "Валовый выброс азота оксидов", "т/год"));
            row = ConstructSettingsFurnaceRow(furns, "NitrogenOxidIVGrossOut", "Валовый выброс азота (IV)оксида", "т/год");
            foreach (TableCell cell in row.Cells) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow); group.Rows.Add(row);
            row = ConstructSettingsFurnaceRow(furns, "NitrogenOxidIIGrossOut", "Валовый выброс азота (II)оксида", "т/год");
            foreach (TableCell cell in row.Cells) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow); group.Rows.Add(row);

            rows = ConstructDeviation("Расчёт выбросов бенз(а)пирена", furns.Count + 2);
            group.Rows.Add(rows[0]); group.Rows.Add(rows[1]);

            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "ExcessAirInFireboxCoeff", "К-т избытка воздуха в топке", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "BoilerLroadOnBenzopyreneConcentrationCoeff", "К-т, учит. влияние нагрузки котла на концентрацию бенз(а)пирена", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "BoilerLroadOnBenzopyreneConcentrationCoeff_GrossOut", "К-т, учит. влияние нагрузки котла на концентрацию бенз(а)пирена (валовый)", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "FlueGasRecirculationBenzapyreneCoeff", "К-т, учит. влияние рециркуляции дымовых газов", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "SteppedAirInputIntoCombustionChamberBenzapyreneCoeff", "К-т, учит. ступенчатого сжигания на концентрацию бенз(а)пирена", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "BenzapyreneConcentrationString", "Концентрация бенз(а)пирена", "мг/м3")); // --------------------------------------
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "HeatVoltageOfCombustionVolumeString", "Теплонапряжение топочного объёма", "кВт/м3")); // --------------------------
            row = ConstructSettingsFurnaceRow(furns, "BenzapyreneGrossOutString", "Валовый выброс бенз(а)пирена", "т/год"); // ---------------------------------------------------
            foreach (TableCell cell in row.Cells) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow); group.Rows.Add(row);
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "BenzapyreneMaxOutString", "Максимальный выброс бенз(а)пирена", "г/с")); // ----------------------------------------

            rows = ConstructDeviation("Расчёт выбросов ртути", furns.Count + 2);
            group.Rows.Add(rows[0]); group.Rows.Add(rows[1]);

            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "MercurySpecificEmission", "Удельный показатель выбросов", "г/тыс. м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "FuelConsumptionInBurningInstallation", "Расход топлива в топливосгорающей установке", "тыс. м^3/ч"));
            row = ConstructSettingsFurnaceRow(furns, "MercuryGrossOutString", "Валовый выброс ртути", "т/год"); // ---------------------------------------------------------------
            foreach (TableCell cell in row.Cells) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow); group.Rows.Add(row);
            group.Rows.Add(ConstructSettingsFurnaceRow(furns, "MercuryMaxOut", "Максимальный выброс ртути", "г/с"));

            return group;
        }

        private Table ConstructSettingsFurnacesTable(bool stored)
        {
            Table table = new Table();
            TableRowGroup rowGroup = new TableRowGroup();
            TableRow row = new TableRow();
            List<Furnace> furns;
            if (stored)
            {
                string path = XMLReader.GetPath(DataCollection.DataPath, "month", EcologicalTaxesHandler.CurrentDate, "furnaces.xml");
                furns = XMLReader.ReadFurnaces(path);
            }
            else
                furns = DataCollection.Instance.Furnaces;

            table.BorderBrush = new SolidColorBrush(Colors.Black);
            table.BorderThickness = new Thickness(1);
            table.CellSpacing = 0;

            TableColumn col = new TableColumn();
            col.Width = new GridLength(450);
            table.Columns.Add(col);
            col = new TableColumn();
            col.Width = new GridLength(120);
            table.Columns.Add(col);
            for (int i = 0; i < furns.Count; i++)
            {
                col = new TableColumn();
                col.Width = new GridLength(150);
                table.Columns.Add(col);
            }

            TableCell cell = new TableCell() { ColumnSpan = furns.Count + 2, Background = new SolidColorBrush(Colors.LightGray), TextAlignment = TextAlignment.Center, Padding = new Thickness(10) };
            Paragraph p = new Paragraph();
            Run r = new Run() { FontWeight = FontWeights.Bold, Text = "Печи хлебопекарные", FontSize = 20 };
            p.Inlines.Add(r);
            cell.Blocks.Add(p);
            row.Cells.Add(cell);

            rowGroup.Rows.Add(row);
            row = new TableRow();

            row.Cells.Add(TableInstancesFactory.ConstructCell("Наименование величин", new string[] { "bold", "padding", "borders" }));
            row.Cells.Add(TableInstancesFactory.ConstructCell("Ед. изм.", new string[] { "bold", "padding", "borders" }));

            foreach (Furnace f in furns)
            {
                cell = TableInstancesFactory.ConstructCell(f.Name, new string[] { "bold", "padding", "borders" });
                if (furns.Last() == f) cell.BorderThickness = new Thickness(0, 1, 0, 0);
                row.Cells.Add(cell);
            }
            rowGroup.Rows.Add(row);

            table.RowGroups.Add(rowGroup);
            table.RowGroups.Add(ConstructSettingsFurnacesGroup(furns));

            return table;
        }
    }
}
