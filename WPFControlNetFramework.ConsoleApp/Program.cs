using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfControlNetFramework;

namespace WPFControlNetFramework.ConsoleApp
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application app = new Application();
            var wpfWindow = new NetFrameworkWPFWindow();
            wpfWindow.MyButtonText = System.Environment.GetEnvironmentVariable("MyButtonText");
            app.Run(wpfWindow);
            Console.WriteLine(wpfWindow.MyButtonText);
        }
    }
}
