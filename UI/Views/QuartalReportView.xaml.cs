using CalculatorTools.Interfaces;
using CalculatorTools.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для QYReportView.xaml
    /// </summary>
    public partial class QuartalReportView : UserControl, INotifyPropertyChanged, IThreatOfDataLoss
    {
        private ResultsWindow _resultsWindow;
        private bool[] _monthExists = new bool[] { false, false, false };
        private bool _cBoxHandled = true;
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
                HandleCBox();
            }
        }

        public QuartalReportView()
        {
            InitializeComponent();
            DataContext = this;

            datePicker.SelectedDate = DateTime.Now;
            EcologicalTaxesHandler.CurrentDate = datePicker.SelectedDate.Value;
            quartalCBox.SelectedIndex = 0;
            HandleCBox();
        }

        private void OnQuartalCBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            _cBoxHandled = !cmb.IsDropDownOpen;
            HandleCBox();
        }

        private void OnQuartalCBoxDropDownClosed(object sender, EventArgs e)
        {
            if (_cBoxHandled) HandleCBox();
            _cBoxHandled = true;
        }

        private void HandleCBox()
        {
            int val = quartalCBox.SelectedIndex; if (val == -1) return;
            EcologicalTaxesHandler.CurrentQuartal = val + 1;
            string[] months = EcologicalTaxesHandler.GetQuartalMonths(val + 1);

            resultInputPanel.ChangeImage(true); resultInputPanel.Text = "Результаты не сохранены";

            monthOnePanel.Text = EcologicalTaxesHandler.GetMonthString(months[0]);
            monthTwoPanel.Text = EcologicalTaxesHandler.GetMonthString(months[1]);
            monthThreePanel.Text = EcologicalTaxesHandler.GetMonthString(months[2]);

            _monthExists = XMLReader.CheckForMonths(DateTime.Parse(DateText), months);
            monthOnePanel.ChangeImage(!_monthExists[0]);
            monthTwoPanel.ChangeImage(!_monthExists[1]);
            monthThreePanel.ChangeImage(!_monthExists[2]);
        }

        private void OnDatePickerSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            HandleCBox();
        }

        private void OnResultsBlockMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_monthExists.Any(me => me == false)) throw new Exception("Невозможно создать отчёт, так как не для всех необходимых месяцев существуют отчёты.");

                EcologicalTaxesHandler.CurrentDate = DateTime.Parse(DateText);
                int val = quartalCBox.SelectedIndex;
                if (val == -1) throw new Exception("Пожалуйста, выберите номер квартала.");

                string title = ((quartalCBox.SelectedItem as ComboBoxItem).Content as string).ToLower() + " " + DateTime.Parse(DateText).Year + " года";
                if (_resultsWindow != null) _resultsWindow.Close();
                _resultsWindow = new ResultsWindow(new ResultsEcologicalTaxesTable(EcologicalTaxesHandler.ReportTypes.Quartal, title));
                _resultsWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка создания отчёта", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnSaveResultsButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_monthExists.Any(me => me == false)) throw new Exception("Невозможно создать отчёт, так как не для всех необходимых месяцев существуют отчёты.");

                EcologicalTaxesHandler.CurrentDate = DateTime.Parse(DateText);
                int val = quartalCBox.SelectedIndex;
                if (val == -1) throw new Exception("Пожалуйста, выберите номер квартала.");

                string[] months = EcologicalTaxesHandler.GetQuartalMonths(val + 1);
                XMLWriter.SaveQuartalReport(DateTime.Parse(DateText), months, (val + 1).ToString());
                resultInputPanel.ChangeImage(); resultInputPanel.Text = "Результаты сохранены";
                _saved = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка просмотра результатов", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Check for leaving
        public bool IsReady()
        {
            if (!_saved && _monthExists.All(m => m == true) && quartalCBox.SelectedIndex != -1) return true;
            else return false;
        }

        // Save from outside view
        public void Save()
        {
            if (IsReady())
            {
                EcologicalTaxesHandler.CurrentDate = DateTime.Parse(DateText);
                int val = quartalCBox.SelectedIndex;
                string[] months = EcologicalTaxesHandler.GetQuartalMonths(val + 1);
                XMLWriter.SaveQuartalReport(DateTime.Parse(DateText), months, (val + 1).ToString());
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
