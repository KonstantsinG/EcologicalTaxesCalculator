using CalculatorTools.Items;
using CalculatorTools.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UI.Controls;
using UI.Tables;

namespace UI.Views
{
    /// <summary>
    /// Логика взаимодействия для ArchiveView.xaml
    /// </summary>
    public partial class ArchiveView : UserControl
    {
        private ResultsWindow _resultsWindow = null;

        private ReportPanel _selectedReport = null;


        public ArchiveView()
        {
            InitializeComponent();

            foreach (ReportsContainer rc in LoadReports())
                foldersContainer.Children.Add(rc);
        }

        private void SelectPanel(ReportPanel panel)
        {
            if (_selectedReport == panel)
            {
                _selectedReport = null;
                return;
            }
            if (_selectedReport != null) _selectedReport.DeselectPanel();

            _selectedReport = panel;
        }

        private List<ReportsContainer> LoadReports()
        {
            List<ReportsContainer> containers = new List<ReportsContainer>();
            List<ReportsCollection> colls = XMLReader.ReadReports();

            foreach (ReportsCollection coll in colls)
                containers.Add(ConstructReportContainer(coll));

            return containers;
        }

        private ReportsContainer ConstructReportContainer(ReportsCollection reps)
        {
            ReportsContainer cont = new ReportsContainer()
            {
                Text = reps.Year,
                MonthReports = ConstructReportPanels(reps.MonthReports, ReportPanel.ReportImage.MonthReport, reps.Year),
                QuartalReports = ConstructReportPanels(reps.QuartalReports, ReportPanel.ReportImage.QuartalReport, reps.Year),
                YearReports = reps.YearReport ? ConstructReportPanels(new List<string>() { reps.Year }, ReportPanel.ReportImage.YearReport, reps.Year) : new ObservableCollection<ReportPanel>()
            };

            return cont;
        }

        private ObservableCollection<ReportPanel> ConstructReportPanels(List<string> names, ReportPanel.ReportImage type, string year)
        {
            ObservableCollection<ReportPanel> panels = new ObservableCollection<ReportPanel>();
            ReportPanel p;

            foreach (string name in names)
            {
                p = new ReportPanel();
                if (type == ReportPanel.ReportImage.MonthReport)
                {
                    p.Text = EcologicalTaxesHandler.GetMonthString(name);
                    p.Date = new DateTime(int.Parse(year), int.Parse(name), 1);
                }
                else if (type == ReportPanel.ReportImage.QuartalReport)
                {
                    p.Text = EcologicalTaxesHandler.GetQuartalString(name);
                    p.Quartal = int.Parse(name);
                    p.Date = new DateTime(int.Parse(year), 1, 1);
                }
                else if (type == ReportPanel.ReportImage.YearReport)
                {
                    p.Text = name;
                    p.Date = new DateTime(int.Parse(year), 1, 1);
                }

                p.ImageType = type;
                p.SelectAction = SelectPanel;
                panels.Add(p);
            }

            return panels;
        }

        private void ShowReport<T>() where T : UserControl
        {
            try
            {
                if (_selectedReport == null) throw new Exception("Для просмотра результатов необходимо выбрать отчёт.");
                if (_selectedReport.ImageType != ReportPanel.ReportImage.MonthReport) throw new Exception("Данный тип отчёта доступен только для месячного интервала.");

                if (_resultsWindow != null) _resultsWindow.Close();
                EcologicalTaxesHandler.CurrentDate = _selectedReport.Date.Value;

                var constructor = typeof(T).GetConstructor(new[] { typeof(bool) });
                if (constructor == null) throw new ArgumentException($"Тип {typeof(T).Name} должен иметь конструктор, принимающий один параметр типа bool");

                _resultsWindow = new ResultsWindow((UserControl)constructor.Invoke(new object[] { true }));
                _resultsWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка просмотра результатов", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnBackProdBlockMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowReport<ResultsBackedProductsTable>();
        }

        private void OnProdEquFurnsMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowReport<ResultsFurnacesTable>();
        }

        private void OnProdEquSteamBoilsMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowReport<ResultsSteamBoilersTable>();
        }

        private void OnProdEquHotWaterBoilsMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowReport<ResultsHotWaterBoilersTable>();
        }

        private void OnEcologicalTaxMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_selectedReport == null) throw new Exception("Для просмотра результатов необходимо выбрать отчёт.");

                if (_resultsWindow != null) _resultsWindow.Close();
                EcologicalTaxesHandler.CurrentDate = _selectedReport.Date.Value;
                if (_selectedReport.Quartal != -1) EcologicalTaxesHandler.CurrentQuartal = _selectedReport.Quartal;

                EcologicalTaxesHandler.ReportTypes type = EcologicalTaxesHandler.ReportTypes.Month;
                string title = "";
                if (_selectedReport.ImageType == ReportPanel.ReportImage.MonthReport)
                {
                    type = EcologicalTaxesHandler.ReportTypes.Month;
                    title = EcologicalTaxesHandler.GetMonthString(_selectedReport.Date.Value.Month.ToString()).ToLower() + " " + 
                            _selectedReport.Date.Value.Year.ToString() + " года";
                }
                else if (_selectedReport.ImageType == ReportPanel.ReportImage.QuartalReport)
                {
                    type = EcologicalTaxesHandler.ReportTypes.Quartal;
                    title = EcologicalTaxesHandler.GetQuartalString(_selectedReport.Quartal.ToString()).ToLower() + " " +
                            _selectedReport.Date.Value.Year.ToString() + " года";
                }
                else if (_selectedReport.ImageType == ReportPanel.ReportImage.YearReport)
                {
                    type = EcologicalTaxesHandler.ReportTypes.Year;
                    title = _selectedReport.Date.Value.Year.ToString() + " год";
                }

                _resultsWindow = new ResultsWindow(new ResultsEcologicalTaxesTable(type, title, true));
                _resultsWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка просмотра результатов", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
