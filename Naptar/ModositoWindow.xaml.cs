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

namespace Naptar
{
    /// <summary>
    /// Interaction logic for ModositoWindow.xaml
    /// </summary>
    public partial class ModositoWindow : Window
    {
        public string Nev { get; set; }
        public ModositoWindow()
        {
            InitializeComponent();

        }

        public ModositoWindow(string nev, string datum)
        {

        }

        private void btn_modMentes_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
