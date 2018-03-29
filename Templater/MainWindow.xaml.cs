using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Templater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int cLeft = 1;

        public static class GlobalData
        {
            public static string[] lines;
            public static List<string> replacables;
            public static List<string> replacements;
            public static List<TextBox> boxes = new List<TextBox>();
            public static bool Generated = false;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public System.Windows.Controls.TextBox AddNewTextBox(String txtString)
        {
            
            TextBox txt = new TextBox();
            txt.Name = "autoBox" + cLeft;
            txt.Height = 23;
            txt.Width = 314;
            txt.Margin = new Thickness(100, (cLeft*30) +40 , 0, 10);
            txt.HorizontalAlignment = HorizontalAlignment.Left;
            txt.VerticalAlignment = VerticalAlignment.Top;
            txt.Text = txtString;
            cLeft = cLeft + 1;
            GlobalData.boxes.Add(txt);
            return txt;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!GlobalData.Generated)
            {
                MessageBox.Show("No Data Fields Generated!");
            }
            else
            {
                //submitButton.Background = Brushes.Red;
                int size = GlobalData.replacables.Count;
                GlobalData.replacements = new List<string>(size);

                for (int i = 0; i < size; i++)
                {
                    GlobalData.replacements.Add(GlobalData.boxes[i].Text);
                }

                //Call Replace Variable and Write to File
                foreach (string line in GlobalData.lines)
                {
                    using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(txtOutFile.Text, true))
                    {
                        string replaced = ReplaceFiller(line);
                        file.WriteLine(replaced);
                    }
                }
                MessageBox.Show("Done!");
                //submitButton.Background = Brushes.Green;
            }
        }

        private string ReplaceFiller(string line)
        {
            string newline = line;
            foreach(string check in GlobalData.replacables)
            {
                if(line.Contains(check))
                {
                    newline = line.Replace(check,GlobalData.replacements[GlobalData.replacables.IndexOf(check)]);
                }
            }
            return(newline);
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            //Console.WriteLine("Button Clicked");
            if (!File.Exists(txtFile.Text))
            {
                MessageBox.Show("Template File does not exist");
                //Generate Dialogue box to find template
            }
            else
            {
                //Put each line into an array
                GlobalData.lines = System.IO.File.ReadAllLines(txtFile.Text);


                //string sPattern = "\\$[A-Z,a-z,_,0-9]+";
                Regex regex = new Regex("\\$[A-Z,a-z,_,0-9]+");

                //count Variable and initialize replacables array
                foreach (string s in GlobalData.lines)
                {
                    foreach (Match match in regex.Matches(s))
                    {
                        //Console.WriteLine(match.Value);
                        index++;
                    }
                }

                //GlobalData.replacables = new String[index];
                index = 0;
                GlobalData.replacables = new List<string>();
                //Make array of vairables
                foreach (string s in GlobalData.lines)
                {
                    foreach (Match match in regex.Matches(s))
                    {
                        if (GlobalData.replacables.Contains(match.Value))
                        {
                            //Console.WriteLine(match.Value + " Already exists");
                        }
                        else
                        {
                            GlobalData.replacables.Add(match.Value);
                            index++;
                        }
                    }
                }
                foreach (string value in GlobalData.replacables)
                {
                    myGrid.Children.Add(AddNewTextBox(value));
                }
                GlobalData.Generated = true;
            }
        }

        private void MainWindow1_Initialized(object sender, EventArgs e)
        { }

        private void btnTemplate_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
           // dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                txtFile.Text = filename;
            }
        }

        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                txtOutFile.Text = filename;
            }
        }

        private void submitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //submitButton.Background = Brushes.Red;
        }
    }
}
