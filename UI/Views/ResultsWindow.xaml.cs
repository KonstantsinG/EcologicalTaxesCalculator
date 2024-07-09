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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI.Views
{
    /// <summary>
    /// Логика взаимодействия для ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        public ResultsWindow(UserControl view)
        {
            InitializeComponent();
            presenter.Content = view;
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (presenter.Content is IThreatOfDataLoss)
            {
                IThreatOfDataLoss threatView = (IThreatOfDataLoss)presenter.Content;

                if (threatView.IsReady())
                {
                    var res = MessageBox.Show("Вы уверены, что хотите оставить введённые данные несохранённым? Если вы выйдете сейчас, вся информация будет утеряна.\n\nСохранить введённые данные?",
                        "Несохранённые данные", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (res == MessageBoxResult.Yes) threatView.Save();
                }
            }
        }
    }
}
