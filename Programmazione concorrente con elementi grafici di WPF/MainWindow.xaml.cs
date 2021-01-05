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
using System.Threading; //libreria per poter usare i thread

namespace Programmazione_concorrente_con_elementi_grafici_di_WPF
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //3 thread per il movimento delle immagini che verranno avviati successivamente, un thread per ogni ogni immagine
        Thread t1;
        Thread t2;
        Thread t3;
        //random che verrà usato successivamente per rendere casuale la velocità dei thread
        Random r;  
        //margini di ogni immagine che stanno a significare il punto di partenza di ogni immagine
        Thickness navicella1Partenza;
        Thickness navicella2Partenza;
        Thickness navicella3Partenza;

        public MainWindow()
        {
            InitializeComponent();
            //creazione del random in modo da non doverlo ricreare ogni volta che si clicca il bottone
            r = new Random(); 
            //assegnazione dei margini iniziali delle immagine
            navicella1Partenza = imgNavicella1.Margin;
            navicella2Partenza = imgNavicella2.Margin;
            navicella3Partenza = imgNavicella3.Margin;
            //non impostiamo gli uri perchè le immagini non dovendo cambiare ma solo muoversi vengono impostate nel mainwindow
        }
        //metodo di movimento delle immagini (da sinistra verso destra) che verrà usato in ogni thread
        public void MetodoMovimento(Image img)   //passiamo l'immagine da muovere
        {
            try
            {
                //prendiamo il margine dell'immagine interessata
                int marginLeft = 0;
                int marginTop = 0;
                int marginBottom = 0;
                int marginRight = 0;
                this.Dispatcher.BeginInvoke(new Action(() =>        //utilizziamo il dispatcher per prendere il margine
                {
                    marginLeft = (int)img.Margin.Left;// facciamo un cast a int perchè ci restituisce un double
                    marginTop = (int)img.Margin.Top;
                    marginBottom = (int)img.Margin.Bottom;
                    marginRight = (int)img.Margin.Right;
                }));
                //facciamo un ciclo fino al margine di arrivo
                while (marginLeft < 700)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(r.Next(1, 751)));//facciamo lo sleep con il random per avere velocità casuali
                    marginLeft += 50; //finito lo sleep muoviamo l'immagine aggiungendo 50 al margine sinistro (per muoverla verso destra)
                    this.Dispatcher.BeginInvoke(new Action(() =>    //utilizziamo il dispatcher per impostare il margine
                    {
                        img.Margin = new Thickness(marginLeft, marginTop, marginBottom, marginRight);     //impostiamo il nuovo margine
                    }));
                }
                //controlliamo il nome identificativo dell'immagine passata da cui possiamo capire se si tratta della navicella 1, della navicella 2 o della navicella 3
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if(img.Name.Contains("1")) //perciò in base al nome dell'immagine aggiungiamo alla lista della classifica il nome della navicella 
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
                    //controlliamo se la lista ha tre elementi, in caso affermativo vuol dire che tutti i thread sono stati eseguiti e che perciò possiamo 
                    //riattivare il bottone per avviare i thread che era stato disattivato precedentemente (disattivato appena si clicca)
                    if (lstPodio.Items.Count == 3)
                    {
                        btnInizia.IsEnabled = true;
                    }
                }));
            }
            catch(Exception ex)
            {
                throw ex; //in caso di errore lanciamo l'eccezione al metodo chiamate
            }
        }

        public void MuoviPrimaNavicella()
        {
            try
            {
                MetodoMovimento(imgNavicella1);  //chiamiamo il metodo per il movimento passando la prima navicella
            }
            catch (Exception ex)
            {
                throw ex;  //in caso di errore lanciamo l'eccezione al metodo chiamate
            }
        }

        public void MuoviSecondaNavicella()
        {
            try
            {
                MetodoMovimento(imgNavicella2); //chiamiamo il metodo per il movimento passando la seconda navicella
            }
            catch (Exception ex)
            {
                throw ex;  //in caso di errore lanciamo l'eccezione al metodo chiamate
            }
        }

        public void MuoviTerzaNavicella()
        {
            try
            {
                MetodoMovimento(imgNavicella3); //chiamiamo il metodo per il movimento passando la terza navicella
            }
            catch (Exception ex)
            {
                throw ex;  //in caso di errore lanciamo l'eccezione al metodo chiamate
            }
        }

        private void btnInizia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnInizia.IsEnabled = false; //disattiviamo il bottone per evitare errori o conflitti tra i thread

                lstPodio.Items.Clear();  //resettiamo la lista della classifica
                //impostiamo i margini delle immagini ai margini di partenza
                imgNavicella1.Margin = navicella1Partenza;    
                imgNavicella2.Margin = navicella2Partenza;
                imgNavicella3.Margin = navicella3Partenza;
                //assegnamo i metodi di movimento ai thread tramite il delegate threadstart
                t1 = new Thread(new ThreadStart(MuoviPrimaNavicella));
                t2 = new Thread(new ThreadStart(MuoviSecondaNavicella));
                t3 = new Thread(new ThreadStart(MuoviTerzaNavicella));
                //avviamo i thread
                t1.Start();
                t2.Start();
                t3.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message, "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //in caso di errore mostriamo un messagebox dal titolo "Errore" e con descrizione "Errore: descrizione errore" e con immagine
            // un'immagine di errore, al quale l'utente potrà solamente dire ok o cliccare la x in alto a destra
        }
    }
}
