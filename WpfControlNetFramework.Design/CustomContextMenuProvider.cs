using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Windows.Design.Interaction;

namespace WpfControlNetFramework.Design
{
    class CustomContextMenuProvider : Microsoft.Windows.Design.Interaction.PrimarySelectionContextMenuProvider
    {
        private MenuAction redBackgroundMenuAction;
        private MenuAction whiteBackgroundMenuAction;
        private MenuAction blueBackgroundMenuAction;
        private MenuAction menuAction_ShowDesignTimeUI_v1;
        private MenuAction menuAction_ShowDesignTimeUI_v2;
        private MenuAction menuAction_ShowDesignTimeUI_v3;
        private MenuAction menuAction_ShowDesignTimeUI_v4;

        public CustomContextMenuProvider()
        {
            // Set up the MenuAction items
            redBackgroundMenuAction = new MenuAction("Red Background");
            redBackgroundMenuAction.Execute +=
                new EventHandler<MenuActionEventArgs>(RedBackground_Execute);

            whiteBackgroundMenuAction = new MenuAction("White Background");
            whiteBackgroundMenuAction.Execute +=
                new EventHandler<MenuActionEventArgs>(WhiteBackground_Execute);

            blueBackgroundMenuAction = new MenuAction("Blue Background");
            blueBackgroundMenuAction.Execute +=
                new EventHandler<MenuActionEventArgs>(BlueBackground_Execute);

            menuAction_ShowDesignTimeUI_v1 = new MenuAction("Show UI from new process in designtools.dll");
            menuAction_ShowDesignTimeUI_v1.Execute +=
                new EventHandler<MenuActionEventArgs>(Show_ShowDesignTimeUI_v1_Execute);

            menuAction_ShowDesignTimeUI_v2 = new MenuAction("Show UI from XAML DependencyProperty - New Process");
            menuAction_ShowDesignTimeUI_v2.Execute +=
                new EventHandler<MenuActionEventArgs>(Show_ShowDesignTimeUI_v2_Execute);

            menuAction_ShowDesignTimeUI_v3 = new MenuAction("Show UI from XAML DependencyProperty - New Process v2");
            menuAction_ShowDesignTimeUI_v3.Execute +=
                new EventHandler<MenuActionEventArgs>(Show_ShowDesignTimeUI_v3_Execute);

            menuAction_ShowDesignTimeUI_v4 = new MenuAction("Show UI from XAML DependencyProperty - New Thread");
            menuAction_ShowDesignTimeUI_v4.Execute +=
                new EventHandler<MenuActionEventArgs>(Show_ShowDesignTimeUI_v4_Execute);


            // Set up the MenuGroup
            MenuGroup myMenuGroup = new MenuGroup("MyMenuGroup", "Custom background");
            myMenuGroup.HasDropDown = false;
            myMenuGroup.Items.Add(menuAction_ShowDesignTimeUI_v1);
            myMenuGroup.Items.Add(menuAction_ShowDesignTimeUI_v2);
            myMenuGroup.Items.Add(menuAction_ShowDesignTimeUI_v3);
            myMenuGroup.Items.Add(menuAction_ShowDesignTimeUI_v4);
            myMenuGroup.Items.Add(redBackgroundMenuAction);
            myMenuGroup.Items.Add(whiteBackgroundMenuAction);
            myMenuGroup.Items.Add(blueBackgroundMenuAction);
            this.Items.Add(myMenuGroup);
        }

        private void Show_ShowDesignTimeUI_v1_Execute(object sender, MenuActionEventArgs e)
        {
            // SUCCESS - Show .Net Core UI at design-time from a .Net Core console app launched in a 
            // new process from this .Net Framework WpfControlNetCore.DesignTools.dll assembly
            var item = e.Selection.PrimarySelection;
            ProcessStartInfo start = new ProcessStartInfo();
            start.UseShellExecute = false;
            start.CreateNoWindow = true;

            string exeFile = @"C:\Users\chris.000\source\repos\WPFNetFrameworkCustomControl\NetFramework\bin\Debug\WPFControlNetFramework.ConsoleApp.exe";
            //string exeFile = System.Reflection.Assembly.GetAssembly(this.GetType()).Location;
            //exeFile = new System.IO.DirectoryInfo(exeFile).Parent.FullName + @"\WPFControlNetFramework.ConsoleApp.exe";

            start.FileName = exeFile;            
            // How to share data between processes? Using Envronment Variable here but 
            // is MemoryMappedFile better for larger data?
            start.EnvironmentVariables["MyButtonText"] = item.Properties["Content"].ComputedValue as string;
            start.RedirectStandardOutput = true; // set to true to read console app StandardOutput below

            using (Process process = Process.Start(start))
            {
                // Read resulting text from the NetCore console app process with the StreamReader
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd().TrimEnd('\r', '\n');
                    item.Properties["Content"].SetValue(result);
                }
            }
        }
        private void Show_ShowDesignTimeUI_v2_Execute(object sender, MenuActionEventArgs e)
        {
            // FAILS - Show .Net Core UI at design-time by using a dependency property as a trigger.
            var item = e.Selection.PrimarySelection;
            item.Properties["DependencyPropertyTrigger"].SetValue("Test New Process");
            item.Properties["DependencyPropertyTrigger"].ClearValue();
            string moreinfo = item.Properties["MoreInfo"].ComputedValue as string;
        }

        private void Show_ShowDesignTimeUI_v3_Execute(object sender, MenuActionEventArgs e)
        {
            var item = e.Selection.PrimarySelection;
            item.Properties["DependencyPropertyTrigger"].SetValue("Test New Process v2");
            item.Properties["DependencyPropertyTrigger"].ClearValue();
        }
        private void Show_ShowDesignTimeUI_v4_Execute(object sender, MenuActionEventArgs e)
        {
            var item = e.Selection.PrimarySelection;
            item.Properties["DependencyPropertyTrigger"].SetValue("Test New Thread");
            item.Properties["DependencyPropertyTrigger"].ClearValue();
        }

        private void RedBackground_Execute(object sender, MenuActionEventArgs e)
        {
            var item = e.Selection.PrimarySelection;
            item.Properties["Background"].SetValue(System.Windows.Media.Brushes.Red);
        }

        private void WhiteBackground_Execute(object sender, MenuActionEventArgs e)
        {
            var item = e.Selection.PrimarySelection;
            item.Properties["Background"].SetValue(System.Windows.Media.Brushes.White);
        }

        private void BlueBackground_Execute(object sender, MenuActionEventArgs e)
        {
            var item = e.Selection.PrimarySelection;
            item.Properties["Background"].SetValue(System.Windows.Media.Brushes.Blue);
        }

    }
}
