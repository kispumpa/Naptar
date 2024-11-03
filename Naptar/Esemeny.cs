using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naptar
{
    public class Esemeny
    {
		private string megnevezes;

		public string Megnevezes
		{
			get { return megnevezes; }
			set { megnevezes = value; }
		}

		private DateTime datum;

		public DateTime Datum
		{
			get { return datum; }
			set { datum = value; }
		}

	}
}
