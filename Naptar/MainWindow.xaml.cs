using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public static ObservableCollection<Felhasznalo> profilok;
        DateTime kivalasztott;
        DateTime ig;

        public MainWindow()
        {
            InitializeComponent();

            calendar.SelectionMode = CalendarSelectionMode.MultipleRange; //kijelölések miatt
            string jsonContent = File.ReadAllText("users.txt");
            if (jsonContent == string.Empty)
            {
                string ures = "Nincs mentés";
                cb_mentesek.Items.Add(ures);
                cb_mentesek.IsEnabled = false;
            }
            else
            {
                Deserialize("users.txt");
                foreach (Felhasznalo f in profilok) //hozzáadja a profilokat
                {
                    cb_mentesek.Items.Add(f.Nev);
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Serialize("users.txt", profilok); //kilépéskor elmenti a változtatásokat
            base.OnClosing(e);
        }


        private void btn_torles_Click(object sender, RoutedEventArgs e)
        {
            string fejlec = "Esemény törlés";
            string uzenet = "Biztos törölni akarod ezt:" +
                $"\n{lb_kijelzo.SelectedItem}";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage image = MessageBoxImage.Warning;
            MessageBoxResult valasz = MessageBox.Show(uzenet, fejlec, button, image, MessageBoxResult.Yes);
            if (valasz == MessageBoxResult.Yes)
            {
                string esemeny = (string)lb_kijelzo.SelectedItem;
                string[] split = esemeny.Split('-');
                string[] oraperc = split[0].Trim().Split(':');
                Esemeny torlendo;
                bool talalt = false;

                foreach (Felhasznalo f in profilok)
                {
                    foreach (Esemeny es in f.Esemenyek)
                    {

                        if (es.Datum.Year == kivalasztott.Year && es.Datum.Month == kivalasztott.Month && es.Datum.Day == kivalasztott.Day && es.Datum.Hour == int.Parse(oraperc[0]) && es.Datum.Minute == int.Parse(oraperc[1]) && cb_mentesek.SelectedValue == f.Nev)
                        {
                            f.Esemenyek.Remove(es);
                            talalt = true;
                            lb_kijelzo.Items.Remove(lb_kijelzo.SelectedItem);
                            break;
                        }
                    }
                    if (talalt == true)
                    {
                        break;
                    }
                }
            }
        }

        private void btn_modositas_Click(object sender, RoutedEventArgs e)
        {
            if (lb_kijelzo.SelectedItem != null)
            {
                ModositoWindow mw = new ModositoWindow((string)lb_kijelzo.SelectedItem, kivalasztott, ig);
                if (mw.ShowDialog() == true)
                {

                }
            }
            else
            {
                ModositoWindow mw = new ModositoWindow(kivalasztott);
                if (mw.ShowDialog() == true)
                {
                    Esemeny uj = new Esemeny() { Megnevezes = mw.Nev, Datum = mw.Datumtol };
                    foreach (Felhasznalo f in profilok)
                    {
                        if (cb_mentesek.SelectedValue == f.Nev)
                        {
                            f.Esemenyek.Add(uj);
                            lb_kijelzo.Items.Add($"{uj.Datum.Hour}:{uj.Datum.Minute} - {uj.Megnevezes}");
                        }

                    }
                }
            }


        }

        private void btn_ujprofil_Click(object sender, RoutedEventArgs e)
        {
            UjProfilWindow upw = new UjProfilWindow();
            if (upw.ShowDialog() == true)
            {
                Felhasznalo uj = new Felhasznalo() { Nev = upw.Nev, Esemenyek = new List<Esemeny>() };
                cb_mentesek.Items.Add(uj.Nev);
                profilok.Add(uj);
            }
        }

        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            lb_kijelzo.Items.Clear();
            btn_modositas.Content = "Új esemény";
            btn_modositas.IsEnabled = true;
            btn_torles.IsEnabled = false;
            DependencyObject originalSource = e.OriginalSource as DependencyObject;
            CalendarDayButton day = FindParentOfType<CalendarDayButton>(originalSource); //kattintott nap, Datacontextbe teljes dátum
            kivalasztott = (DateTime)day.DataContext;

            if (day != null)
            {
                foreach (Felhasznalo f in profilok)
                {
                    foreach (Esemeny es in f.Esemenyek)
                    {
                        if (es.Datum.Year == ((DateTime)day.DataContext).Year && es.Datum.Month == ((DateTime)day.DataContext).Month && es.Datum.Day == ((DateTime)day.DataContext).Day && cb_mentesek.SelectedValue == f.Nev)
                        {
                            lb_kijelzo.Items.Add($"{es.Datum.Hour}:{es.Datum.Minute} - {es.Megnevezes}");
                        }
                    }
                }
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

        private void cb_mentesek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lb_kijelzo.Items.Clear();
            calendar.SelectedDates.Clear(); //törli az előző kijelölést
            btn_torles.IsEnabled = false;
            btn_modositas.IsEnabled = false;
            foreach (Felhasznalo f in profilok)
            {
                if (cb_mentesek.SelectedValue == f.Nev)
                {
                    foreach (Esemeny es in f.Esemenyek)
                    {
                        calendar.SelectedDates.Add(es.Datum);
                    }
                }
            }
        }

        static void Serialize(string filename, ObservableCollection<Felhasznalo> list)
        {
            string jsonContent = JsonConvert.SerializeObject(list);
            File.WriteAllText(filename, jsonContent);
        }
        public static void Deserialize(string filename)
        {
            string jsonContent = File.ReadAllText(filename);
            profilok = JsonConvert.DeserializeObject<ObservableCollection<Felhasznalo>>(jsonContent);
        }

        private void lb_kijelzo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_modositas.Content = "Módosítás";
            btn_torles.IsEnabled = true;
        }
    }
}