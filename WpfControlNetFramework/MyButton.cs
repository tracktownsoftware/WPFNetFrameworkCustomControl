using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace WpfControlNetFramework
{

    public class MyButton : Button
    {
        public static readonly DependencyProperty DependencyPropertyTriggerProperty = DependencyProperty.Register(
"DependencyPropertyTrigger", typeof(string), typeof(MyButton), new PropertyMetadata("ShowUI", new PropertyChangedCallback(TriggerChangedCallback)));

        public static readonly DependencyProperty MoreInfoProperty = DependencyProperty.Register(
"MoreInfo", typeof(string), typeof(MyButton), new PropertyMetadata("ABC"));


        static MyButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyButton), new FrameworkPropertyMetadata(typeof(MyButton)));
        }

        public string MoreInfo
        {
            get
            {
                return (string)this.GetValue(MoreInfoProperty);
            }
            set
            {
                //var oldValue = MoreInfo;
                this.SetValue(MoreInfoProperty, value);
                //OnPropertyChanged(new DependencyPropertyChangedEventArgs(MoreInfoProperty,oldValue,value));
            }
        }

        private static void TriggerChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //myButton.MoreInfo = "hello this is a test!";
            d.SetValue(MoreInfoProperty, "hello this is a test!");
            if ((string)(e.NewValue) == "ShowUI")
            {
                MyButton myButton = d as MyButton;
                myButton.ShowUI(e.OldValue as string);
            }
        }
        //private static object TriggerCoerceValueCallback(DependencyObject d, object value)
        //{
        //    //Gauge g = (Gauge)d;
        //    //double current = (double)value;
        //    //if (current < g.MinReading) current = g.MinReading;
        //    //if (current > g.MaxReading) current = g.MaxReading;
        //    return "";
        //}

        public string DependencyPropertyTrigger
        {
            get
            {
                return (string)this.GetValue(DependencyPropertyTriggerProperty);
            }
            set

            {
                this.SetValue(DependencyPropertyTriggerProperty, value);
            }
        }

        private void ShowUI(string value)
        {
            if (value.Contains("New Process v2"))
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                //#if DEBUG
                string exeFile = @"C:\Users\chris.000\source\repos\WPFNetFrameworkCustomControl\NetFramework\bin\Debug\WPFControlNetFramework.ConsoleApp.exe";
                //#else
                //string exeFile = System.Reflection.Assembly.GetAssembly(this.GetType()).Location;
                //exeFile = new System.IO.DirectoryInfo(exeFile).Parent.FullName + @"\Design\WPFControlNetCore.ConsoleApp.exe";
                //#endif
                start.FileName = exeFile;
                this.SetValue(MoreInfoProperty, exeFile);

                start.EnvironmentVariables["MyButtonText"] = this.Content as string;
                start.RedirectStandardOutput = true; // set to true to read console app StandardOutput below
                using (Process process = Process.Start(start))
                {
                    // Read resulting text from the NetCore console app process with the StreamReader
                    using (System.IO.StreamReader reader = process.StandardOutput)
                    {
                        //string result = reader.ReadToEnd().TrimEnd('\r', '\n');
                        this.Content = reader.ReadToEnd().TrimEnd('\r', '\n');
                    }
                }
            }
            else if (value.Contains("New Thread"))
            {
                string newContent = this.Content as string;
                //this.Content = "apple banana";
                Thread t = new Thread(() =>
                {
                    var netFrameworkWPFWindow = new NetFrameworkWPFWindow();
                    netFrameworkWPFWindow.MyButtonText = newContent;
                    if (netFrameworkWPFWindow.ShowDialog() == true)
                    {
                        newContent = netFrameworkWPFWindow.MyButtonText;
                    }
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();
                this.Content = newContent;
            }
            else if (value.Contains("New Process"))
            {
                // var NetFrameworkWPFWindow = new NetFrameworkWPFWindow();
                //NetFrameworkWPFWindow.ShowDialog();
                //Just try to launch browser window
                string url = @"https://www.cnn.com";
                Process.Start(url); //This doesn't work in .Net Core
                                    //Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
        }
    }
}
