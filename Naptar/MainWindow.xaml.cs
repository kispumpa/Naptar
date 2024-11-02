using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Naptar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Felhasznalo> profilok= new List<Felhasznalo>();
            string jsonContent = File.ReadAllText("users.txt");
            if (jsonContent == string.Empty)
            {
                string ures = "Nincs mentés";
                cb_mentesek.Items.Add(ures);
                cb_mentesek.IsEnabled = false;
            }
            else
            {
                profilok = JsonConvert.DeserializeObject<List<Felhasznalo>>(jsonContent);
                foreach (Felhasznalo f in profilok)
                {
                    cb_mentesek.Items.Add(f.Nev);
                }
            }
        }

        private void btn_torles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_modositas_Click(object sender, RoutedEventArgs e)
        {
            
            ModositoWindow mw = new ModositoWindow();
            if (mw.ShowDialog() == true)
            {
                 
            }
        }

        private void btn_ujprofil_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject originalSource = e.OriginalSource as DependencyObject;
            CalendarDayButton day = FindParentOfType<CalendarDayButton>(originalSource); //kattintott nap, Datacontextbe teljes dátum

            if (day != null)
            {
                //open menu
            }
            e.Handled = true;
        }

        public static T FindParentOfType<T>(DependencyObject source) where T : DependencyObject
        {
            T ret = default(T);
            DependencyObject parent = VisualTreeHelper.GetParent(source);

            if (parent != null)
            {
                ret = parent as T ?? FindParentOfType<T>(parent) as T;
            }
            return ret;
        }
    }
}