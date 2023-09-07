using GridPerformance.Helpers;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace GridPerformance.Modelos
{
	public class DadosGrid
	{
		public int Id { get; set; }
		public string? Descricao { get; set; }
		public string? Localizacao { get; set; }
		public int Qtde { get; set; }
		public double Valor { get; set; }
		public DateTime Data { get; set; }
		public SolidColorBrush? Cores { get; set; }
		public bool Ok { get; set; }

		public static IEnumerable<DadosGrid> ObterModelo(int nrLinhas, int min, int max)
		{
			var random = new Random();

			for (int i = 0; i < nrLinhas; i++)
			{
				yield return new DadosGrid
				{
					Id = i,
					Descricao = RandomHelper.StringAleatoria(random, min, max),
					Localizacao = RandomHelper.StringAleatoria(random, min, max),
					Qtde = random.Next(),
					Valor = random.NextDouble(),
					Data = RandomHelper.DiasAleatorios(random),
					Cores = RandomHelper.CoresAleatorias(random),
					Ok = random.Next(2) == 1
				};
			}
		}
	}
}
