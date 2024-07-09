using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using System.Net.Http;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Threading;

namespace UI.Views
{
    /// <summary>
    /// Логика взаимодействия для WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : UserControl
    {
        private bool _pressed = false;

        public WelcomeView()
        {
            InitializeComponent();

            dayTextBlock.Text = "Сегодня " + DateTime.Now.Date.ToShortDateString();
        }

        private void OnImageMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_pressed) return;
            _pressed = true;

            if (Directory.Exists(CalculatorTools.Utilities.DataCollection.AppDataPath + "x"))
            {
                MessageBox.Show("Я могу тебе помочь", "[xx_annprocess]", MessageBoxButton.OK, MessageBoxImage.Information);
                var resx = MessageBox.Show(" Nt,t ye;yf gjvjom?\n Nt,t ye;yf gjvjom?\n Nt,t ye;yf gjvjom?", "[xx_annprocess]", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resx == MessageBoxResult.Yes)
                {
                    ResultsWindow w;
                    for (int i = 0; i < 10; i++)
                    {
                        w = new ResultsWindow(null);
                        w.Show(); Thread.Sleep(100); w.Close();
                    }
                    Directory.Move(CalculatorTools.Utilities.DataCollection.AppDataPath+"x", CalculatorTools.Utilities.DataCollection.AppDataPath);
                    MessageBox.Show("Данные были успешно восстановлены.", "Оповещение системы хранилища", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                App.Current.MainWindow.Close(); return;
            }

            App.Current.MainWindow.Close();
            string[] data = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\');
            string cap = "";

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    cap = ip.ToString();

            string longMessage = "Снег опустился на одинокие и холодные улицы.. Откуда ни возьмись, поднялся ветер и стал поднимать в воздух и кружить туда-сюда целые рои снежинок. Казалось, старые и одинокие дома, погружённые в вечную тоску, подняли свои уставшие веки и устремили взор к небу. Серые и тёмно-коричневые разводы начали медленно сползать с их массивных тел… У подножия этих вечных монолитов неспешно бродили люди, уткнув свой носы поглубже в тёплые зимние куртки. Их потоки перемещались из одной улицы в другую, как талый снег, собираемый обычными ливнёвками. А я всё смотрела свысока… Быстрые потоки воздуха подхватывали всё больше снега и разгонялись всё сильнее, поднимая в воздух уже целые сугробы. Но это нисколько не мешало.. Всё выглядело так, словно смотришь в новогодний шарик, наполненный водой и пенопластом… А вот, вдалеке проехал поезд по заснеженным путям, издав протяжный гудок.. Интересно, куда он направляется?";
            MessageBox.Show(longMessage, "-1- Kettle -1-", MessageBoxButton.OK, MessageBoxImage.Information);
            longMessage = "Видимо, стоит опускаться. Как же я соскучилась по этой планете. Ни у одного из существ в этой Вселенной нету такой невероятной системы, позволяющей испытывать столь глубокие чувства, поверь, я знаю… Последние минутки моих наблюдений.. Как только кончик моего пальца на ноге коснётся холодной плитки, всё начётся заново, я ничего не вспомню.. Ах, это просто прекрасное чувство. Я покрепче зажмуриваю глаза.. Расслабляю каждую конечность своего тела, запрокидываю голову… \n… \nОни видят меня.";
            MessageBox.Show(longMessage, "-2- Kettle -2-", MessageBoxButton.OK, MessageBoxImage.Information);

            var res = MessageBox.Show(data.Last()+", ты меня слышишь?..", "-NaN- Kettle -NaN-", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes) MessageBox.Show("Я тебя тоже слышу..", "-NaN- Kettle -NaN-", MessageBoxButton.OK, MessageBoxImage.Error);
            else MessageBox.Show("Прощай...", "-NaN- Kettle -NaN-", MessageBoxButton.OK, MessageBoxImage.Error);

            MessageBox.Show("Sending data to "+cap+"...", "Sending process", MessageBoxButton.OK, MessageBoxImage.Information);
            for (int i = 0; i < 5; i++) MessageBox.Show("Trying to resolve an occurred error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            MessageBox.Show("Данные были успешно удалены. Все процессы будут прекращены.", "Оповещение системы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            Directory.Move(CalculatorTools.Utilities.DataCollection.AppDataPath, CalculatorTools.Utilities.DataCollection.AppDataPath+"x");
        }
    }
}
