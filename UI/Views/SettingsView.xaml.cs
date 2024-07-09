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
using UI.Controls;
using UI.Tables;

namespace UI.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            settingsTablesRenderer.Content = new SettingsImage();
        }

        private void OnBackedProductsMouseDown(object sender, MouseButtonEventArgs e)
        {
            backedProductsTB.FontWeight = FontWeights.Bold;
            furnacesTB.FontWeight = FontWeights.Light;
            steamBoilersTB.FontWeight = FontWeights.Light;
            waterBoilersTB.FontWeight = FontWeights.Light;

            var backProdTable = new SettingsBackedProductsTable();
            settingsTablesRenderer.Content = backProdTable;
        }

        private void OnFurnacesMouseDown(object sender, MouseButtonEventArgs e)
        {
            backedProductsTB.FontWeight = FontWeights.Light;
            furnacesTB.FontWeight = FontWeights.Bold;
            steamBoilersTB.FontWeight = FontWeights.Light;
            waterBoilersTB.FontWeight = FontWeights.Light;

            var furnsTable = new SettingsFurnacesTable();
            settingsTablesRenderer.Content = furnsTable;
        }

        private void OnSteamBoilersMouseDown(object sender, MouseButtonEventArgs e)
        {
            backedProductsTB.FontWeight = FontWeights.Light;
            furnacesTB.FontWeight = FontWeights.Light;
            steamBoilersTB.FontWeight = FontWeights.Bold;
            waterBoilersTB.FontWeight = FontWeights.Light;

            var boilersTable = new SettingsSteamBoilersTable();
            settingsTablesRenderer.Content = boilersTable;
        }

        private void OnHotWaterBoilersMouseDown(object sender, MouseButtonEventArgs e)
        {
            backedProductsTB.FontWeight = FontWeights.Light;
            furnacesTB.FontWeight = FontWeights.Light;
            steamBoilersTB.FontWeight = FontWeights.Light;
            waterBoilersTB.FontWeight = FontWeights.Bold;

            var boilersTable = new SettingsHotWaterBoilersTable();
            settingsTablesRenderer.Content = boilersTable;
        }
    }
}
