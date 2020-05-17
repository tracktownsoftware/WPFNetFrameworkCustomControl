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

        static MyButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyButton), new FrameworkPropertyMetadata(typeof(MyButton)));
        }

        private static void TriggerChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((string)(e.NewValue) == "ShowUI")
            {
                MyButton myButton = d as MyButton;
                myButton.ShowUI(e.OldValue as string);
            }
        }

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
            if (value.Contains("New Thread"))
            {
                string newContent = this.Content as string;
                Thread t = new Thread(() =>
                {
                    var netFrameworkWPFWindow = new NetFrameworkWPFWindow();
                    netFrameworkWPFWindow.Topmost = true;
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
            //else if (value.Contains("New Process"))
            //{
            //    // var NetFrameworkWPFWindow = new NetFrameworkWPFWindow();
            //    //NetFrameworkWPFWindow.ShowDialog();
            //    //Just try to launch browser window
            //    string url = @"https://www.cnn.com";
            //    Process.Start(url); //This doesn't work in .Net Core
            //                        //Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            //}
        }
    }
}
