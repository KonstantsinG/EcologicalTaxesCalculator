using CalculatorTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            subViewRenderer.Content = new WelcomeView();
        }

        private void OnNewReportMouseEnter(object sender, MouseEventArgs e)
        {
            rectNewReport.Fill = new SolidColorBrush(Color.FromArgb(255, 218, 255, 226));
            textBoxNewReport.Foreground = new SolidColorBrush(Color.FromArgb(255, 103, 137, 172));
        }

        private void OnNewReportMouseLeave(object sender, MouseEventArgs e)
        {
            rectNewReport.Fill = new SolidColorBrush(Color.FromArgb(255, 103, 137, 172));
            textBoxNewReport.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 218, 226));
        }

        private void OnArchiveMouseEnter(object sender, MouseEventArgs e)
        {
            rectArchive.Fill = new SolidColorBrush(Color.FromArgb(255, 218, 255, 226));
            textBoxArchive.Foreground = new SolidColorBrush(Color.FromArgb(255, 103, 137, 172));
        }

        private void OnArchiveMouseLeave(object sender, MouseEventArgs e)
        {
            rectArchive.Fill = new SolidColorBrush(Color.FromArgb(255, 103, 137, 172));
            textBoxArchive.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 218, 226));
        }

        private void OnArchiveMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseReportCheck((UserControl)subViewRenderer.Content);

            ArchiveView view = new ArchiveView();
            subViewRenderer.Content = view;
        }

        private void OnSettingsMouseEnter(object sender, MouseEventArgs e)
        {
            rectSettings.Fill = new SolidColorBrush(Color.FromArgb(255, 218, 255, 226));
            textBoxSettings.Foreground = new SolidColorBrush(Color.FromArgb(255, 103, 137, 172));
        }

        private void OnSettingsMouseLeave(object sender, MouseEventArgs e)
        {
            rectSettings.Fill = new SolidColorBrush(Color.FromArgb(255, 103, 137, 172));
            textBoxSettings.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 218, 226));
        }

        private void OnSettingsMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseReportCheck((UserControl)subViewRenderer.Content);

            SettingsView view = new SettingsView();
            subViewRenderer.Content = view;
        }

        private void OnNewReportMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseReportCheck((UserControl)subViewRenderer.Content);

            NewReportView view = new NewReportView(NewMonthReport, NewQuarterReport, NewYearReport);
            subViewRenderer.Content = view;
        }

        private void NewMonthReport(object sender, RoutedEventArgs e)
        {
            MonthReportView view = new MonthReportView();
            subViewRenderer.Content = view;
        }

        private void NewQuarterReport(object sender, RoutedEventArgs e)
        {
            QuartalReportView view = new QuartalReportView();
            subViewRenderer.Content = view;
        }

        private void NewYearReport(object sender, RoutedEventArgs e)
        {
            YearReportView view = new YearReportView();
            subViewRenderer.Content = view;
        }


        private void CloseReportCheck(UserControl view)
        {
            if (view == null) return;

            if (view is IThreatOfDataLoss)
            {
                IThreatOfDataLoss threatView = (IThreatOfDataLoss)view;

                if (threatView.IsReady())
                {
                    var res = MessageBox.Show("Вы уверены, что хотите оставить отчёт несохранённым? Если вы выйдете сейчас, вся информация будет утеряна.\n\nСохранить отчёт?",
                        "Несохранённые данные", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (res == MessageBoxResult.Yes) threatView.Save();
                }
            }
        }
    }
}
