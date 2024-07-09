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

namespace UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для ReportPanel.xaml
    /// </summary>
    public partial class ReportPanel : UserControl, INotifyPropertyChanged
    {
        private DateTime? _date = null;

        public DateTime? Date
        {
            get => _date;
            set => _date = value;
        }

        private int _quartal = -1;

        public int Quartal
        {
            get => _quartal;
            set => _quartal = value;
        }

        public enum ReportImage
        {
            MonthReport,
            QuartalReport,
            YearReport
        }

        private bool _selected;

        public bool Selected
        {
            get => _selected;
            set => _selected = value;
        }

        private Action<ReportPanel> _selectAction = null;

        public Action<ReportPanel> SelectAction
        {
            get => _selectAction;
            set => _selectAction = value;
        }

        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        private string _image;

        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        private ReportImage _imageType;

        public ReportImage ImageType
        {
            get => _imageType;
            set
            {
                _imageType = value;

                if (value == ReportImage.MonthReport)
                    Image = "/Images/monthReport.png";
                else if (value == ReportImage.QuartalReport)
                    Image = "/Images/quartalReport.png";
                else if (value == ReportImage.YearReport)
                    Image = "/Images/yearReport.png";
            }
        }

        public ReportPanel()
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

        private void OnGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            _selected = !_selected;

            if (_selected) borders.Visibility = Visibility.Visible;
            else borders.Visibility = Visibility.Collapsed;

            if (_selectAction != null) _selectAction(this);
        }

        public void DeselectPanel()
        {
            _selected = false;
            borders.Visibility = Visibility.Collapsed;
        }
    }
}
