using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naptar
{
    public class Felhasznalo
    {
		private string nev; //név

		public string Nev
		{
			get { return nev; }
			set { nev = value; }
		}

		//private DateTime[] datumok; //beírt esemény dátumok

		//public DateTime[] Datumok
		//{
		//	get { return datumok; }
		//	set { datumok = value; }
		//}

        private string[,] esemenyek { get; set; }

		public string[,] Esemenyek //1. oszlop: dátum, 2. oszlop esemény leírás
		{
			get { return esemenyek; }
			set { esemenyek = value; }
		}
    }
}
