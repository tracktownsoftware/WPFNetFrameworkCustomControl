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
using System.Windows.Shapes;

namespace WpfControlNetFramework
{
    /// <summary>
    /// Interaction logic for NetFrameworkWPFWindow.xaml
    /// </summary>
    public partial class NetFrameworkWPFWindow : Window
    {
        public NetFrameworkWPFWindow()
        {
            InitializeComponent();
            this.buttonCancel.Click += ButtonCancel_Click;
            this.buttonSave.Click += ButtonSave_Click;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            this.MyButtonText = textBox.Text;
            this.DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private string _myButtonText;
        public string MyButtonText
        {
            get
            {
                return _myButtonText;
            }
            set
            {
                _myButtonText = value;
                textBox.Text = value;
            }
        }
    }
}
