using System;
using System.Windows.Media;

namespace GridPerformance.Helpers
{
	public static class RandomHelper
	{
		public static string StringAleatoria(Random random, int min, int max)
		{
			int tamanhoString = random.Next(min, max + 1);
			int valorRandomico;
			string str = "";
			char letra;
			for (int i = 0; i < tamanhoString; i++)
			{
				valorRandomico = random.Next(0, 26);

				letra = Convert.ToChar(valorRandomico + 65);

				str = str + letra;
			}
			return str;
		}

		public static DateTime DiasAleatorios(Random random)
		{
			DateTime start = new DateTime(1995, 1, 1);
			int range = (DateTime.Today - start).Days;
			return start.AddDays(random.Next(range));
		}

		public static SolidColorBrush CoresAleatorias(Random random)
			=> new SolidColorBrush(Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 233)));
	}
}
