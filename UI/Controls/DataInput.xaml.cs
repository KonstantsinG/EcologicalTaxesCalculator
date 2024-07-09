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
    /// Логика взаимодействия для DataInput.xaml
    /// </summary>
    public partial class DataInput : UserControl, INotifyPropertyChanged
    {
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

        public DataInput()
        {
            InitializeComponent();

            bitmap.UriSource = new Uri("pack://application:,,,/Images/cross.png");
            DataContext = this;
        }

        public void ChangeImage(bool cross = false)
        {
            if (cross)
                imageViewer.Source = new BitmapImage(new Uri(@"/Images/cross.png", UriKind.Relative));
            else
                imageViewer.Source = new BitmapImage(new Uri(@"/Images/done.png", UriKind.Relative));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
