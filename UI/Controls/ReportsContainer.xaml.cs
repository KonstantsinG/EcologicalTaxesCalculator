using CalculatorTools.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для ReportsContainer.xaml
    /// </summary>
    public partial class ReportsContainer : UserControl, INotifyPropertyChanged
    {
        private string _text;

        private bool _selected = false;
        private bool _monthSelected = false;
        private bool _quartalSelected = false;
        private bool _yearSelected = false;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        private ObservableCollection<ReportPanel> _monthReports;

        public ObservableCollection<ReportPanel> MonthReports
        {
            get => _monthReports;
            set
            {
                _monthReports = value;
                OnPropertyChanged("MonthReports");
            }
        }

        private ObservableCollection<ReportPanel> _quartalReports;

        public ObservableCollection<ReportPanel> QuartalReports
        {
            get => _quartalReports;
            set
            {
                _quartalReports = value;
                OnPropertyChanged("QuartalReports");
            }
        }

        private ObservableCollection<ReportPanel> _yearReports;

        public ObservableCollection<ReportPanel> YearReports
        {
            get => _yearReports;
            set
            {
                _yearReports = value;
                OnPropertyChanged("YearReports");
            }
        }

        public ReportsContainer()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void OnFolderImgMouseDown(object sender, MouseButtonEventArgs e)
        {
            _selected = !_selected;

            if (_selected)
            {
                folderNameTBlock.FontWeight = FontWeights.Bold;
                selectionPanel.Fill = new SolidColorBrush(Color.FromArgb(255, 196, 220, 239));
                selectionBorder.Visibility = Visibility.Visible;
                subfoldersContainer.Visibility = Visibility.Visible;
            }
            else
            {
                folderNameTBlock.FontWeight= FontWeights.Normal;
                selectionPanel.Fill = null;
                selectionBorder.Visibility = Visibility.Collapsed;
                subfoldersContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void OnMonthFolderMouseDown(object sender, MouseButtonEventArgs e)
        {
            _monthSelected = !_monthSelected;

            if (_monthSelected)
            {
                monthReportsContainer.Visibility = Visibility.Visible;
                monthTBlock.FontWeight = FontWeights.Bold;
            }
            else
            {
                monthReportsContainer.Visibility = Visibility.Collapsed;
                monthTBlock.FontWeight = FontWeights.Light;
            }
        }

        private void OnQuartalFolderMouseDown(object sender, MouseButtonEventArgs e)
        {
            _quartalSelected = !_quartalSelected;

            if (_quartalSelected)
            {
                quartalReportsContainer.Visibility = Visibility.Visible;
                quartalTBlock.FontWeight = FontWeights.Bold;
            }
            else
            {
                quartalReportsContainer.Visibility = Visibility.Collapsed;
                quartalTBlock.FontWeight = FontWeights.Light;
            }
        }

        private void OnYearFolderMouseDown(object sender, MouseButtonEventArgs e)
        {
            _yearSelected = !_yearSelected;

            if (_yearSelected)
            {
                yearReportsContainer.Visibility = Visibility.Visible;
                yearTBlock.FontWeight = FontWeights.Bold;
            }
            else
            {
                yearReportsContainer.Visibility = Visibility.Collapsed;
                yearTBlock.FontWeight = FontWeights.Light;
            }
        }
    }
}
