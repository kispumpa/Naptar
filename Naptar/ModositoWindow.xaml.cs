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
        public string Nev { get; private set; }
        public DateTime Datumtol { get; set; }
        public DateTime Datumig { get; set; }
        public ModositoWindow(DateTime datumtol) // új esemény
        {
            InitializeComponent();

            dtp_tol.Value = datumtol;
        }

        public ModositoWindow(string nev, DateTime datumtol, DateTime datumig) //esemény módosítás
        {
            InitializeComponent();

            tb_esemenyNev.Text = nev;
            dtp_tol.Value = datumtol;
            if (datumig.Year != 0001)
            {
                cb_tobbnap.IsEnabled = true;
                dtp_ig.Value = datumig;
            }
        }

        private void btn_modMentes_Click(object sender, RoutedEventArgs e)
        {
            Nev = tb_esemenyNev.Text;
            Datumtol = (DateTime)dtp_tol.Value;
            if (cb_tobbnap.IsChecked == true)
            {
                Datumig = (DateTime)dtp_ig.Value;
            }
            ;
            this.DialogResult = true;
        }
    }
}
