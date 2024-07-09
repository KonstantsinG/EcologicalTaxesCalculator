using CalculatorTools.Interfaces;
using CalculatorTools.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using UI.Tables;

namespace UI.Views
{
    /// <summary>
    /// Логика взаимодействия для YearReportView.xaml
    /// </summary>
    public partial class YearReportView : UserControl, INotifyPropertyChanged, IThreatOfDataLoss
    {
        private ResultsWindow _resultsWindow;
        private bool[] _monthsExists;
        private bool _saved = false;

        private string _dateText = DateTime.Now.ToShortDateString();

        public string DateText
        {
            get => _dateText;
            set
            {
                if (_dateText == value) return;

                _dateText = value;
                OnPropertyChanged(nameof(DateText));

                _saved = false;
                CheckMonths();
            }
        }

        public YearReportView()
        {
            InitializeComponent();
            DataContext = this;

            datePicker.SelectedDate = DateTime.Now;
            EcologicalTaxesHandler.CurrentDate = datePicker.SelectedDate.Value;
            CheckMonths();
        }

        private void CheckMonths()
        {
            resultInputPanel.ChangeImage(true); resultInputPanel.Text = "Результаты не сохранены";

            string[] monthsStr = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            _monthsExists = XMLReader.CheckForMonths(DateTime.Parse(DateText), monthsStr);

            januaryPanel.ChangeImage(!_monthsExists[0]);
            februaryPanel.ChangeImage(!_monthsExists[1]);
            marchPanel.ChangeImage(!_monthsExists[2]);
            aprilPanel.ChangeImage(!_monthsExists[3]);
            mayPanel.ChangeImage(!_monthsExists[4]);
            junePanel.ChangeImage(!_monthsExists[5]);
            julyPanel.ChangeImage(!_monthsExists[6]);
            augustPanel.ChangeImage(!_monthsExists[7]);
            septemberPanel.ChangeImage(!_monthsExists[8]);
            octoberPanel.ChangeImage(!_monthsExists[9]);
            novemberPanel.ChangeImage(!_monthsExists[10]);
            decemberPanel.ChangeImage(!_monthsExists[11]);
        }

        private void OnDatePickerSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckMonths();
            EcologicalTaxesHandler.CurrentDate = DateTime.Parse(DateText);
        }

        private void OnResultsBlockMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_monthsExists.Any(m => m == false)) throw new Exception("Невозможно создать отчёт, так как не для всех необходимых месяцев существуют отчёты.");

                EcologicalTaxesHandler.CurrentDate = DateTime.Parse(DateText);
                string title = DateTime.Parse(DateText).Year + " год";
                if (_resultsWindow != null) _resultsWindow.Close();
                _resultsWindow = new ResultsWindow(new ResultsEcologicalTaxesTable(EcologicalTaxesHandler.ReportTypes.Year, title));
                _resultsWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка просмотра результатов", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_monthsExists.Any(m => m == false)) throw new Exception("Невозможно создать отчёт, так как не для всех необходимых месяцев существуют отчёты.");

                EcologicalTaxesHandler.CurrentDate = DateTime.Parse(DateText);
                XMLWriter.SaveYearReport(DateTime.Parse(DateText));
                resultInputPanel.ChangeImage(); resultInputPanel.Text = "Результаты сохранены";
                _saved = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения результатов", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Check for leaving
        public bool IsReady()
        {
            if (!_saved && _monthsExists.All (m => m == true)) return true;
            else return false;
        }

        // Save from outside view
        public void Save()
        {
            if (IsReady())
            {
                EcologicalTaxesHandler.CurrentDate = DateTime.Parse(DateText);
                XMLWriter.SaveYearReport(DateTime.Parse(DateText));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
