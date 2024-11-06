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

        #region Clicks

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
                EsemenyTorles();
            }
        }
        private void btn_modositas_Click(object sender, RoutedEventArgs e)
        {
            if (lb_kijelzo.SelectedItem != null) //esemény módosítás
            {
                string[] split = ((string)lb_kijelzo.SelectedItem).Split('-');
                string esemenyNev = split[1].Trim();
                ModositoWindow mw = new ModositoWindow(esemenyNev, kivalasztott, ig);
                if (mw.ShowDialog() == true)
                {
                    Esemeny uj = new Esemeny() { Megnevezes = mw.Nev, Datum = mw.Datumtol };
                    foreach (Felhasznalo f in profilok)
                    {
                        if (cb_mentesek.SelectedValue == f.Nev)
                        {
                            f.Esemenyek.Add(uj);
                            lb_kijelzo.Items.Add($"{uj.Datum.Hour}:{uj.Datum.Minute} - {uj.Megnevezes}");
                            EsemenyTorles();
                        }

                    }
                }
            }
            else
            {
                ModositoWindow mw = new ModositoWindow(kivalasztott); //új esemény
                if (mw.ShowDialog() == true)
                {
                    Esemeny uj = new Esemeny() { Megnevezes = mw.Nev, Datum = mw.Datumtol };
                    if (mw.Datumig.Year != 0001)
                    {
                        ig = mw.Datumig;
                    }
                    foreach (Felhasznalo f in profilok)
                    {
                        if (cb_mentesek.SelectedValue == f.Nev)
                        {
                            f.Esemenyek.Add(uj);
                            lb_kijelzo.Items.Add($"{uj.Datum.Hour}:{uj.Datum.Minute} - {uj.Megnevezes}");
                            if (mw.Datumig.Year != 0001)
                            {
                                ;
                                Esemeny vege = new Esemeny() { Megnevezes = mw.Nev + " esemény vége", Datum = mw.Datumig };
                                f.Esemenyek.Add(vege);

                                Esemeny potuj = new Esemeny() { Megnevezes = mw.Nev, Datum = new DateTime(mw.Datumtol.Year, mw.Datumtol.Month, mw.Datumtol.Day) };
                                Esemeny potvege = new Esemeny() { Datum = new DateTime(mw.Datumig.Year, mw.Datumig.Month, mw.Datumig.Day) };
                                long ticks = potvege.Datum.Ticks - potuj.Datum.Ticks;
                                TimeSpan ts = new TimeSpan(ticks);
                                if (ts.Days > 1)
                                {
                                    for (int i = 0; i < ts.Days - 1; i++)
                                    {
                                        potuj.Datum = potuj.Datum.AddDays(1);
                                        DateTime novelo = potuj.Datum;
                                        Esemeny ki = new Esemeny() { Megnevezes = mw.Nev, Datum = novelo };
                                        f.Esemenyek.Add(ki);
                                    }
                                }
                            }
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
            EsemenyHighlight();
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


        #endregion

        #region SelectionChanges

        private void cb_mentesek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lb_kijelzo.Items.Clear();
            calendar.SelectedDates.Clear(); //törli az előző kijelölést
            btn_torles.IsEnabled = false;
            btn_modositas.IsEnabled = false;
            EsemenyHighlight();
        }
        private void lb_kijelzo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_modositas.Content = "Módosítás";
            btn_torles.IsEnabled = true;
        }

        #endregion

        #region segéd metódusok

        static void Serialize(string filename, ObservableCollection<Felhasznalo> list)
        {
            string jsonContent = JsonConvert.SerializeObject(list);
            File.WriteAllText(filename, jsonContent);
        }
        static void Deserialize(string filename)
        {
            string jsonContent = File.ReadAllText(filename);
            profilok = JsonConvert.DeserializeObject<ObservableCollection<Felhasznalo>>(jsonContent);
        }
        static T FindParentOfType<T>(DependencyObject source) where T : DependencyObject
        {
            T ret = default(T);
            DependencyObject parent = VisualTreeHelper.GetParent(source);

            if (parent != null)
            {
                ret = parent as T ?? FindParentOfType<T>(parent) as T;
            }
            return ret;
        }
        void EsemenyTorles()
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
        void EsemenyHighlight()
        {
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

        #endregion

    }
}