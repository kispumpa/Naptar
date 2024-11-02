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

		private string[] datumok; //beírt események

		public string[] Datumok
		{
			get { return datumok; }
			set { datumok = value; }
		}

	}
}
