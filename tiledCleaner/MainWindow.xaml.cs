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
using System.Text.RegularExpressions;
using System.IO;

namespace tiledCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Fonction pour importer des tiled .tmx
        private void import(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            dlg.Filter = "Tiled documents(.tmx)|*.tmx";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                Debug.WriteLine(dlg.FileName);

                jobsList.Items.Add(filename);
            }
        }

        private void deleteFileFromList(object sender, RoutedEventArgs e)
        {
            String fileToDelete = jobsList.SelectedItem.ToString();
            jobsList.Items.RemoveAt(jobsList.Items.IndexOf(jobsList.SelectedItem));
            Debug.WriteLine(fileToDelete + " a été supprimé");
        }

        private void cleanFile(object sender, RoutedEventArgs e)
        {
            String fileToClean = jobsList.SelectedItem.ToString();
            String path = @fileToClean;

            if (File.Exists(path))
            {
                Debug.WriteLine("Le fichier " + fileToClean + " existe bien !");

                String tiledContent = File.ReadAllText(path);

                String pattern = "(?<=source=\")(.+?)(?=tileset_)";
                String replacement = "";
                Regex rgx = new Regex(pattern);

                String cleanedTiledContent = rgx.Replace(tiledContent, replacement);

                File.WriteAllText(path, cleanedTiledContent);

                Debug.WriteLine("Le fichier " + fileToClean + " a été nettoyé."+"\n");

                Run log = new Run("Le fichier " + fileToClean + " a été nettoyé."+"\n");

                Debug.WriteLine(logsResult.Inlines.Count);


                if (logsResult.Inlines.Count > 0)
                {
                    logsResult.Inlines.InsertAfter(logsResult.Inlines.LastInline, log);
                }
                else
                {
                    logsResult.Inlines.Add(log);
                }



                //jobsList.Items.RemoveAt(jobsList.Items.IndexOf(jobsList.SelectedItem));
            }
            else
            {
                Debug.WriteLine("ERREUR FICHIER INEXISTANT!!!!");
            }
        }

        private void dradAndDropFiles(object sender, DragEventArgs e)
        {
            string[]files= (string[]) e.Data.GetData(DataFormats.FileDrop);


            for (int i = 0; i < files.Count(); i++)
            {
                if (files[i].Contains(".tmx"))
                {
                    jobsList.Items.Add(files[i]);
                }
                else
                    logsResult.Inlines.Add(string.Format("le fichier {0} n'est pas un fichier valide \n", files[i]));
                
            }


                Debug.WriteLine(files);
            Debug.WriteLine(e);
        }
    }
}