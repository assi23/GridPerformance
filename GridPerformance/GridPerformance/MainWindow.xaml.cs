using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace GridPerformance
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private  int qtdLinhasGerar;
		private  int valorMin;
		private  int valorMax;
		private  bool deveVarrer;
		public MainWindow()
		{
			InitializeComponent();
			Tabela.BeginningEdit += (s, ss) => ss.Cancel = true;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			base.Cursor = Cursors.Wait;
			lbTempoProcessamento.Content = "Gerando linhas ...";
			SetarValores();

			Tabela.ItemsSource = null;
			Tabela.Items.Clear();
			DoEvents();

			var dataInicio = DateTime.Now;
			AdicionarNaLista();
			VarrerTabela();

			var dataFim = DateTime.Now;
			lbTotalLinhas.Content = "Total de linhas:" + Tabela.Items.Count;
			lbTempoProcessamento.Content = "Tempo de processamento:" + dataFim.Subtract(dataInicio).Milliseconds + " ms";
			base.Cursor = Cursors.Arrow;
		}

		private void SetarValores()
		{
			int.TryParse(tb_nrLinha.Text, out qtdLinhasGerar);
			int.TryParse(tb_valorMin.Text, out valorMin);
			int.TryParse(tb_valorMax.Text, out valorMax);
			deveVarrer = cb_varredura.IsChecked ?? false;
		}

		private void VarrerTabela()
		{
			if (deveVarrer)
			{
				pbProcessamento.Maximum = Tabela.Items.Count;
				pbProcessamento.Visibility = Visibility.Visible;

				for (int i = 0; i < Tabela.Items.Count; i++)
				{
					Tabela.SelectedIndex = i;
					DoEvents();
					Tabela.ScrollIntoView(Tabela.Items[i]);
					pbProcessamento.Value = i;
				}

				pbProcessamento.Visibility = Visibility.Hidden;
			}
		}

		private void AdicionarNaLista()
		{
			foreach (var item in Modelo.ObterModelo(qtdLinhasGerar, valorMin, valorMax))
			{
				Tabela.Items.Add(item);
			}
		}

		public void DoEvents()
		{
			DispatcherFrame frame = new DispatcherFrame();
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
				new DispatcherOperationCallback(ExitFrame), frame);
			Dispatcher.PushFrame(frame);
		}
		public object ExitFrame(object f)
		{
			((DispatcherFrame)f).Continue = false;

			return null;
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
					Id = i,
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

