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
    /// Логика взаимодействия для ResultsHotWaterBoilresTable.xaml
    /// </summary>
    public partial class ResultsHotWaterBoilersTable : UserControl
    {
        public ResultsHotWaterBoilersTable(bool stored = false)
        {
            InitializeComponent();

            Table table = ConstructSettingsFurnacesTable(stored);
            tableRoot.Blocks.Add(table);

            double sumWidth = 50;
            foreach (TableColumn col in table.Columns) sumWidth += col.Width.Value;
            tableRoot.PageWidth = sumWidth;
        }

        private TableRow ConstructSettingsFurnaceRow(List<HotWaterBoiler> boilers, string propName, string rowName, string extraValue)
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
                    if (boilers.Last() == b) cell.BorderThickness = new Thickness(0, 1, 0, 0);
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

        private TableRowGroup ConstructSettingsFurnacesGroup(List<HotWaterBoiler> boilers)
        {
            TableRowGroup group = new TableRowGroup();
            TableRow row;

            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "Place", "Местоположение", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "FuelType", "Вид топлива", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "MinCombustionHeat", "Низшая теплота сгорания", "МДж/м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "RatedLoad", "Нагрузка номинальная", "МВт"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "EfficiencyGrossOfBoiler", "КПД «Брутто» котла", "%"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "WorkingTimePerYear", "Число часов работы/год", "ч."));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "FuelConsumptionOnMaxPower", "Расход топлива на макс. нагрузке", "м^3/с"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "FuelConsumptionPerYear", "Годовой расход топлива", "тыс.м^3/год"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "FuelConsumptionOnMeanPower", "Расход топлива при средней нагрузке", "м^3/с"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "TheoreticalVolumeOfDryFlueGases", "Теоретический объём сухих дымовых газов", "м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "VolumeOfDryFlueGases", "Объём сухих дымовых газов", "м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "StrangeEmptyField", " - Наименование параметра отсутствует -", "тыс.м^3/год"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "ActualFuelConsumption", "Фактический расход топлива", "м^3/с"));

            TableRow[] rows = ConstructDeviation("Расчёт выбросов углерода оксида", boilers.Count + 2);
            group.Rows.Add(rows[0]); group.Rows.Add(rows[1]);

            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "HeatLossDueToChemicalIncompleteCombustionOfFuel_ForGross", "Потери теплоты при хим. неполн. сгорания топлива (валовые)", "%"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "HeatLossDueToChemicalIncompleteCombustionOfFuel_ForMax", "Потери теплоты при хим. неполн. сгорания топлива (макс.)", "%"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "HeatLossDueToChemicalIncompleteCombustionOfFuel_Coeff", "К-т, учит. долю потери теплоты, вследствии хим. неполн. сгорания", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "CarbonOxidOut_ForGross", "Выход углерода оксида (валовый)", "г/м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "CarbonOxidOut_ForMax", "Выход углерода оксида (макс.)", "г/м^3"));
            row = ConstructSettingsFurnaceRow(boilers, "CarbonOxidGrossOut", "Валовый выброс углерода оксида", "т/год");
            foreach (TableCell cell in row.Cells) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow); group.Rows.Add(row);
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "CarbonOxidMaxCount", "Макс. количество углерода оксида", "г/с"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "CalculatedMaxCOConcentration", "Рассчитанная макс. концентрация CO", "мг/м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "CarbonOxidMaxEmission_Norm", "Норм. макс. выброс углерода оксида", "г/с"));

            rows = ConstructDeviation("Расчёт выбросов азота оксидов", boilers.Count + 2);
            group.Rows.Add(rows[0]); group.Rows.Add(rows[1]);

            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "NitrogenOxidSpecificEmissionString", "Удельный выброс азота оксидов", "г/МДж")); // --------------------------
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "NitrogenOxidSpecificMeanEmissionString", "Средний удельный выброс азота оксидов", "г/МДж")); // --------------
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "BurnerConstructionCoeff", "К-т, учит. конструкцию горелки", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "AirTemperatureForBurnerCoeff", "К-т, учит. температуру воздуха, подаваемого для горения", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "FlueGasRecirculationCoeff", "К-т, учит. влияние рециркуляции дымовых газов", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "SteppedAirInputIntoCombustionChamberCoeff", "К-т, учит. ступенчатый ввод воздуха в топочную камеру", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "NitrogenOxidMaxOnNO2", "Макс. количество азота оксидов в пересчёте на NO2", "г/с"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "NitrogetOxidsGrossOut", "Валовый выброс азота оксидов", "т/год"));
            row = ConstructSettingsFurnaceRow(boilers, "NitrogenOxidIVGrossOut", "Валовый выброс азота (IV)оксида", "т/год");
            foreach (TableCell cell in row.Cells) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow); group.Rows.Add(row);
            row = ConstructSettingsFurnaceRow(boilers, "NitrogenOxidIIGrossOut", "Валовый выброс азота (II)оксида", "т/год");
            foreach (TableCell cell in row.Cells) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow); group.Rows.Add(row);
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "CalculatedMaxConcentrationNO2String", "Рассчит. макс. концентрация азота оксидов в пересчёте на NO2", "мг/м^3"));

            rows = ConstructDeviation("Расчёт выбросов бенз(а)пирена", boilers.Count + 2);
            group.Rows.Add(rows[0]); group.Rows.Add(rows[1]);

            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "ExcessAirInFireboxCoeff", "К-т избытка воздуха в топке", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "BoilerLroadOnBenzopyreneConcentrationCoeff", "К-т, учит. влияние нагрузки котла на концентрацию бенз(а)пирена", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "BoilerLroadOnBenzopyreneConcentrationCoeff_GrossOut", "К-т, учит. влияние нагрузки котла на концентрацию бенз(а)пирена (валовый)", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "FlueGasRecirculationBenzapyreneCoeff", "К-т, учит. влияние рециркуляции дымовых газов", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "SteppedAirInputIntoCombustionChamberBenzapyreneCoeff", "К-т, учит. ступенчатого сжигания на концентрацию бенз(а)пирена", " - "));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "BenzapyreneConcentrationString", "Концентрация бенз(а)пирена", "мг/м3")); // --------------------------------------
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "HeatVoltageOfCombustionVolumeString", "Теплонапряжение топочного объёма", "кВт/м3")); // --------------------------
            row = ConstructSettingsFurnaceRow(boilers, "BenzapyreneGrossOutString", "Валовый выброс бенз(а)пирена", "т/год"); // ---------------------------------------------------
            foreach (TableCell cell in row.Cells) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow); group.Rows.Add(row);
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "BenzapyreneMaxOutString", "Максимальный выброс бенз(а)пирена", "г/с")); // ----------------------------------------

            rows = ConstructDeviation("Расчёт выбросов ртути", boilers.Count + 2);
            group.Rows.Add(rows[0]); group.Rows.Add(rows[1]);

            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "MercurySpecificEmission", "Удельный показатель выбросов", "г/тыс. м^3"));
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "FuelConsumptionInBurningInstallation", "Расход топлива в топливосгорающей установке", "тыс. м^3/ч"));
            row = ConstructSettingsFurnaceRow(boilers, "MercuryGrossOutString", "Валовый выброс ртути", "т/год"); // ---------------------------------------------------------------
            foreach (TableCell cell in row.Cells) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow); group.Rows.Add(row);
            group.Rows.Add(ConstructSettingsFurnaceRow(boilers, "MercuryMaxOut", "Максимальный выброс ртути", "г/с"));

            return group;
        }

        private Table ConstructSettingsFurnacesTable(bool stored)
        {
            Table table = new Table();
            TableRowGroup rowGroup = new TableRowGroup();
            TableRow row = new TableRow();
            List<HotWaterBoiler> boilers;
            if (stored)
            {
                string path = XMLReader.GetPath(DataCollection.DataPath, "month", EcologicalTaxesHandler.CurrentDate, "hotWaterBoilers.xml");
                boilers = XMLReader.ReadHotWaterBoilers(path);
            }
            else
                boilers = DataCollection.Instance.HotWaterBoilers;

            table.BorderBrush = new SolidColorBrush(Colors.Black);
            table.BorderThickness = new Thickness(1);
            table.CellSpacing = 0;

            TableColumn col = new TableColumn();
            col.Width = new GridLength(450);
            table.Columns.Add(col);
            col = new TableColumn();
            col.Width = new GridLength(120);
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
            table.RowGroups.Add(ConstructSettingsFurnacesGroup(boilers));

            return table;
        }
    }
}
