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

		private List<Esemeny> esemenyek;

		public List<Esemeny> Esemenyek
		{
			get { return esemenyek; }
			set { esemenyek = value; }
		}
    }
}
