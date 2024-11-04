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
    /// Interaction logic for UjProfilWindow.xaml
    /// </summary>
    public partial class UjProfilWindow : Window
    {
        public string Nev { get; private set; }

        public UjProfilWindow()
        {
            InitializeComponent();
        }

        private void btn_profilmentes_Click(object sender, RoutedEventArgs e)
        {
            Nev = tb_profilnev.Text;
            
            this.DialogResult = true; //kilép
        }
    }
}
