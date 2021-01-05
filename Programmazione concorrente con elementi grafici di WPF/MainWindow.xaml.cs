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
using System.Threading;

namespace Programmazione_concorrente_con_elementi_grafici_di_WPF
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread t1;
        Thread t2;
        Thread t3;

        Random r;

        public MainWindow()
        {
            InitializeComponent();
            r = new Random();

            t1 = new Thread(new ThreadStart(MuoviPrimaNavicella));
            t2 = new Thread(new ThreadStart(MuoviSecondaNavicella));
            t3 = new Thread(new ThreadStart(MuoviTerzaNavicella));

            t1.Start();
            t2.Start();
            t3.Start();
        }

        public void MetodoMovimento(Image img)
        {
            int marginTop = 0;
            int marginLeft = 0;
            img.Dispatcher.BeginInvoke(new Action(() =>
            {
                marginTop = (int)img.Margin.Top;
                marginLeft = (int)imgNavicella1.Margin.Left;
            }));
            while (marginLeft < 700)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(r.Next(1, 751)));
                marginLeft += 50;
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    img.Margin = new Thickness(marginLeft, marginTop, 0, 0);
                }));
            }
        }

        public void MuoviPrimaNavicella()
        {
            MetodoMovimento(imgNavicella1);
        }

        public void MuoviSecondaNavicella()
        {
            MetodoMovimento(imgNavicella2);
        }

        public void MuoviTerzaNavicella()
        {
            MetodoMovimento(imgNavicella3);
        }

    }
}
