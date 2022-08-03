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
using System.Diagnostics;
using System.IO;
using System.Net;


namespace App_Cleaner_Pc
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DirectoryInfo winTemp;
        public DirectoryInfo appTemp;
        public string version = " 1.0.0";


        public MainWindow()
        {
            InitializeComponent();
            winTemp = new DirectoryInfo(@"C:\Windows\Temp"); // get doc o  file  temporary in windows 
            appTemp = new DirectoryInfo(System.IO.Path.GetTempPath()); // the path who return get doc 
            News_App();
            Getdate();
        }
        // Creation of the link on the server to get the news
        public void News_App()
        {
            string url= "https://sdonfallou.github.io/actualyappcleaner/filetext.txt";
            using (WebClient client = new WebClient())
            {
                string news= client.DownloadString(url);
                if (news != string.Empty)
                {
                    newstxt.Content = news;
                    newstxt.Visibility = Visibility.Visible;
                   // band_news.Visibility = Visibility.Visible;
                }
            }
        }
        // Version Check 
        public void Check_Version()
        {
            string url = "https://sdonfallou.github.io/actualyappcleaner/filetext.txt";
            using (WebClient client = new WebClient())
            {
                string versionUpdate = client.DownloadString(url);
                if (version != versionUpdate)
                {
                    MessageBox.Show("New Version Available","UPDATE STATUS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Software already update", "UPDATE STATUS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        // calcul  the size of doc or file to cancel
        public long DirSize(DirectoryInfo dir) {
            // take all element of the file to calcul the total 
            return dir.GetFiles().Sum(fi => fi.Length) + dir.GetDirectories().Sum(di => DirSize(di));
        }
        // to void an file 
        public void ClearTempData(DirectoryInfo di)
        {
            // This function take a parameters winTemp and appTemp and do a loop foreach file in order to create an document delete
            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                    Console.WriteLine(file.FullName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("File Delete "+ ex.Message);
                }
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {  // Creation of the file delete
                try
                {
                    dir.Delete(true);
                    Console.WriteLine(dir.FullName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("FILES :"+ex.Message);
                }
            }
        }        
    

        private void Button_Click_Clean(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Cleaning in progress...");
            btn_click.Content = "Cleaning in progress";
            // to delete file  stock in the presspaper
            // we have already the function ready to use
            Clipboard.Clear(); // on copy colle click
          
            try
            {
                ClearTempData(winTemp);
            } catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);

            }

            try
            {
                ClearTempData(appTemp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);

            }

            btn_click.Content = "Cleaning finished";
            general_title.Content = "Cleaning Done !";
            space.Content = "0Mb";
        }


    

        private void Button_Click_site_web(object sender, RoutedEventArgs e)
        {
           try
             {
              Process.Start(new ProcessStartInfo("www.Ccleaner.com")
              {
                  UseShellExecute = true
               });
           }catch (Exception ex)
           {
            Console.WriteLine("Error : " + ex.Message);
           }
        }

        private void Button_Click_history(object sender, RoutedEventArgs e)
        {
         MessageBox.Show("Todo ", "story");
        }

        private void Button_Click_Analyse(object sender, RoutedEventArgs e)
        {
            AnalyseFolders();
        }
        public void AnalyseFolders()
        {
            Console.WriteLine("Start of Analyse...");
            long totalSize = 0;
            // put TRy catch to make a code robust21
            try {
                totalSize += DirSize(winTemp) / 1000000;
                totalSize += DirSize(appTemp) / 1000000;
            }catch (Exception ex)
            {
                Console.WriteLine("Impossible to Analyse this Files "+ex.Message);
            }


            
            space.Content = totalSize + "Mb";
            
            date.Content = DateTime.Today;
            Last_date_update_done();
        }
        // Save a date to show the last 
        public void Last_date_update_done()
        {
            string datelastupdate = DateTime.Today.ToString();
            File.WriteAllText("date.txt", datelastupdate);
        }

        // get date 
        public void Getdate()
        {
            string dateFile = File.ReadAllText("date.txt");
            if (dateFile != string.Empty)
            {
                date.Content = dateFile;
            }

        }

        private void Button_Click_update(object sender, RoutedEventArgs e)
        {
            Check_Version();
        }
    }
}
