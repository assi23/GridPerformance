using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GridPerformance
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Tabela_AddingNewItem();
		}

		private void Tabela_AddingNewItem()
		{
			int.TryParse(tb_nrLinha.Text, out var qtdLinhasGerar);
			int.TryParse(tb_valorMin.Text, out var valorMin);
			int.TryParse(tb_valorMax.Text, out var valorMax);

			foreach (var item in Modelo.ObterModelo(qtdLinhasGerar, valorMin, valorMax))
			{
			
				Tabela.Items.Add(item);

			}

		}
	}

	public class Modelo
	{
		public int Id { get; set; }
		public string? Descricao { get; set; }
		public string? Localizacao { get; set; }
		public int Qtde { get; set; }
		public decimal Valor { get; set; }
		public DateTime Data { get; set; }
		public SolidColorBrush? Cores { get; set; }
		public bool Ok { get; set; }

		public static IEnumerable<Modelo> ObterModelo(int nrLinhas, int min, int max)
		{
			var random = new Random();

			for (int i = 0; i < nrLinhas; i++)
			{
				yield return new Modelo
				{
					Id = random.Next(),
					Descricao = StringAleatoria(random, min, max),
					Localizacao = StringAleatoria(random, min, max),
					Qtde = random.Next(),
					Valor = (decimal)random.NextDouble(),
					Data = DiasAleatorios(random),
					Cores = CoresAleatorias(random),
					Ok = random.Next(2) == 1
				};
			}


		}

		static string StringAleatoria(Random random, int min, int max)
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

		static DateTime DiasAleatorios(Random random)
		{
			DateTime start = new DateTime(1995, 1, 1);
			int range = (DateTime.Today - start).Days;
			return start.AddDays(random.Next(range));
		}

		static SolidColorBrush CoresAleatorias(Random random)
			=> new SolidColorBrush(Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 233)));
	}
}

