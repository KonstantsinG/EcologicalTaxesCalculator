using CalculatorTools.Interfaces;
using CalculatorTools.Items;
using CalculatorTools.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Tables;

namespace UI.Views
{
    /// <summary>
    /// Логика взаимодействия для NewReportView.xaml
    /// </summary>
    public partial class MonthReportView : UserControl, INotifyPropertyChanged, IThreatOfDataLoss
    {
        private ResultsWindow _resultsWindow = null;
        private bool _backedProductsEntered = false;
        private bool _productionEquipmentEntered = false;
        private bool _saved = false;

        private string _dateText = DateTime.Now.ToShortDateString();

        public string DateText
        {
            get => _dateText;
            set
            {
                if (_dateText == value) return;

                _dateText = value;
                OnPropertyChanged("DateText");

                _saved = false;
                _backedProductsEntered = false;
                backedProductsInputPanel.ChangeImage(true);
                _productionEquipmentEntered = false;
                productionEquipmentInputPanel.ChangeImage(true);
                resultInputPanel.ChangeImage(true);
            }
        }

        public MonthReportView()
        {
            InitializeComponent();
            DataContext = this;

            datePicker.SelectedDate = DateTime.Now;
            EcologicalTaxesHandler.CurrentDate = datePicker.SelectedDate.Value;
        }

        private void OnDataInputBlockMouseDown(object sender, MouseButtonEventArgs e)
        {
            dataInputStack.Visibility = Visibility.Visible;
            prodEquStack.Visibility = Visibility.Collapsed;

            dataInputBlock.FontWeight = FontWeights.Bold;
            backProdBlock.FontWeight = FontWeights.Light;
            prodEquBlock.FontWeight = FontWeights.Light;
            resultsBlock.FontWeight = FontWeights.Light;
        }

        private void OnDataInputBackProdMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_resultsWindow != null) _resultsWindow.Close();
            _resultsWindow = new ResultsWindow(new InputBackedProductsTable(CloseWindow, BackedProductsEntered));
            _resultsWindow.Show();
        }

        private void OnDataInputProdEquMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_resultsWindow != null) _resultsWindow.Close();
            _resultsWindow = new ResultsWindow(new InputProductionEquipmentTable(CloseWindow, ProductionEquipmentEntered));
            _resultsWindow.Show();
        }

        private void OnDataInputTaxParamsMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_resultsWindow != null) _resultsWindow.Close();
            _resultsWindow = new ResultsWindow(new InputTaxParamsTable(CloseWindow, TaxParamsUpdated));
            _resultsWindow.Show();
        }

        private void OnBackProdBlockMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_backedProductsEntered)
            {
                MessageBox.Show("Перед тем как просматривать результаты, пожалуйста, введите всю необходимую информацию", "Ошибка получения результатов", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dataInputStack.Visibility = Visibility.Collapsed;
            prodEquStack.Visibility = Visibility.Collapsed;

            dataInputBlock.FontWeight = FontWeights.Light;
            backProdBlock.FontWeight = FontWeights.Bold;
            prodEquBlock.FontWeight = FontWeights.Light;
            resultsBlock.FontWeight = FontWeights.Light;

            if (_resultsWindow != null) _resultsWindow.Close();
            _resultsWindow = new ResultsWindow(new ResultsBackedProductsTable());
            _resultsWindow.Show();
        }

        private void OnProdEquBlockMouseDown(object sender, MouseButtonEventArgs e)
        {
            dataInputStack.Visibility = Visibility.Collapsed;
            prodEquStack.Visibility = Visibility.Visible;

            dataInputBlock.FontWeight = FontWeights.Light;
            backProdBlock.FontWeight = FontWeights.Light;
            prodEquBlock.FontWeight = FontWeights.Bold;
            resultsBlock.FontWeight = FontWeights.Light;
        }

        private void OnProdEquFurnsMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_productionEquipmentEntered)
            {
                MessageBox.Show("Перед тем, как просматривать результаты, пожалуйста, введите всю необходимую информацию", "Ошибка получения результатов", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_resultsWindow != null) _resultsWindow.Close();
            _resultsWindow = new ResultsWindow(new ResultsFurnacesTable());
            _resultsWindow.Show();
        }

        private void OnProdEquSteamBoilsMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_productionEquipmentEntered)
            {
                MessageBox.Show("Перед тем, как просматривать результаты, пожалуйста, введите всю необходимую информацию", "Ошибка получения результатов", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_resultsWindow != null) _resultsWindow.Close();
            _resultsWindow = new ResultsWindow(new ResultsSteamBoilersTable());
            _resultsWindow.Show();
        }

        private void OnProdEquHotWaterBoilsMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_productionEquipmentEntered)
            {
                MessageBox.Show("Перед тем, как просматривать результаты, пожалуйста, введите всю необходимую информацию", "Ошибка получения результатов", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_resultsWindow != null) _resultsWindow.Close();
            _resultsWindow = new ResultsWindow(new ResultsHotWaterBoilersTable());
            _resultsWindow.Show();
        }

        private void OnResultsBlockMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_backedProductsEntered || !_productionEquipmentEntered)
            {
                MessageBox.Show("Перед тем, как просматривать результаты, пожалуйста, введите всю необходимую информацию", "Ошибка получения результатов", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dataInputStack.Visibility = Visibility.Collapsed;
            prodEquStack.Visibility = Visibility.Collapsed;

            dataInputBlock.FontWeight = FontWeights.Light;
            backProdBlock.FontWeight = FontWeights.Light;
            prodEquBlock.FontWeight = FontWeights.Light;
            resultsBlock.FontWeight = FontWeights.Bold;

            DateTime date = DateTime.Parse(DateText);
            string title = EcologicalTaxesHandler.GetMonthString(date.Month.ToString()).ToLower() + " " + date.Year.ToString() + " года";
            EcologicalTaxesHandler.CurrentDate = date;
            if (_resultsWindow != null) _resultsWindow.Close();
            _resultsWindow = new ResultsWindow(new ResultsEcologicalTaxesTable(EcologicalTaxesHandler.ReportTypes.Month, title));
            _resultsWindow.Show();
        }

        private void CloseWindow()
        {
            _resultsWindow.Close();
            _resultsWindow = null; 
        }

        private void BackedProductsEntered()
        {
            CloseWindow();
            _backedProductsEntered = true;

            backedProductsInputPanel.ChangeImage();
        }

        private void ProductionEquipmentEntered()
        {
            CloseWindow();
            _productionEquipmentEntered = true;

            productionEquipmentInputPanel.ChangeImage();
        }

        private void TaxParamsUpdated()
        {
            CloseWindow();
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_backedProductsEntered || !_productionEquipmentEntered)
            {
                MessageBox.Show("Перед тем, как сохранить результаты, пожалуйста, введите всю необходимую информацию", "Ошибка получения результатов", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _saved = true;
            DateTime date = DateTime.Parse(DateText);
            EcologicalTaxesHandler.CurrentDate = date;
            resultInputPanel.ChangeImage();
            resultInputPanel.Text = "Результаты сохранены";

            List<BackedProduct> prods = DataCollection.Instance.BackedProducts;
            List<Furnace> furns = DataCollection.Instance.Furnaces;
            List<SteamBoiler> steamBoils = DataCollection.Instance.SteamBoilers;
            List<HotWaterBoiler> waterBoils = DataCollection.Instance.HotWaterBoilers;
            List<TaxParam> taxParams = DataCollection.Instance.TaxParams;

            XMLWriter.SaveMonthReport(prods, furns, steamBoils, waterBoils, taxParams, date);
        }

        // Check for leaving
        public bool IsReady()
        {
            if (!_saved && _backedProductsEntered && _productionEquipmentEntered) return true;
            else return false;
        }

        // Save from outside view
        public void Save()
        {
            if (IsReady())
            {
                DateTime date = DateTime.Parse(DateText);
                EcologicalTaxesHandler.CurrentDate = date;

                List<BackedProduct> prods = DataCollection.Instance.BackedProducts;
                List<Furnace> furns = DataCollection.Instance.Furnaces;
                List<SteamBoiler> steamBoils = DataCollection.Instance.SteamBoilers;
                List<HotWaterBoiler> waterBoils = DataCollection.Instance.HotWaterBoilers;
                List<TaxParam> taxParams = DataCollection.Instance.TaxParams;

                XMLWriter.SaveMonthReport(prods, furns, steamBoils, waterBoils, taxParams, date);
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
