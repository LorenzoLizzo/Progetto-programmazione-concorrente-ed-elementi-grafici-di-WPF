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

        Thickness navicella1Partenza;
        Thickness navicella2Partenza;
        Thickness navicella3Partenza;

        public MainWindow()
        {
            InitializeComponent();
            r = new Random();
            navicella1Partenza = imgNavicella1.Margin;
            navicella2Partenza = imgNavicella2.Margin;
            navicella3Partenza = imgNavicella3.Margin;
        }

        public void MetodoMovimento(Image img)
        {
            try
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

                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if(img.Name.Contains("1"))
                    {
                        lstPodio.Items.Add("La morte nera (navicella 1)");
                    }
                    else if(img.Name.Contains("2"))
                    {
                        lstPodio.Items.Add("Millennium falcon (navicella 2)");
                    }
                    else if (img.Name.Contains("3"))
                    {
                        lstPodio.Items.Add("Enterprise (navicella 3)");
                    }
                    if (lstPodio.Items.Count == 3)
                    {
                        btnInizia.IsEnabled = true;
                    }
                }));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void MuoviPrimaNavicella()
        {
            try
            {
                MetodoMovimento(imgNavicella1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MuoviSecondaNavicella()
        {
            try
            {
                MetodoMovimento(imgNavicella2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MuoviTerzaNavicella()
        {
            try
            {
                MetodoMovimento(imgNavicella3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnInizia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnInizia.IsEnabled = false;

                lstPodio.Items.Clear();

                imgNavicella1.Margin = navicella1Partenza;
                imgNavicella2.Margin = navicella2Partenza;
                imgNavicella3.Margin = navicella3Partenza;

                t1 = new Thread(new ThreadStart(MuoviPrimaNavicella));
                t2 = new Thread(new ThreadStart(MuoviSecondaNavicella));
                t3 = new Thread(new ThreadStart(MuoviTerzaNavicella));

                t1.Start();
                t2.Start();
                t3.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message, "Attenzione", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
