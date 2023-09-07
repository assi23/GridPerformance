using GridPerformance.Modelos;
using System;
using System.Diagnostics;
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
		private int qtdLinhasGerar;
		private int valorMin;
		private int valorMax;
		private bool deveVarrer;
		protected PerformanceCounter cpuCounter;

		public MainWindow()
		{
			InitializeComponent();
			cpuCounter = new PerformanceCounter();

			cpuCounter.CategoryName = "Processor";
			cpuCounter.CounterName = "% Processor Time";
			cpuCounter.InstanceName = "_Total";
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			SetarValores();

			if (!Validacoes())
				return;

			base.Cursor = Cursors.Wait;

			lbTempoProcessamento.Content = "Gerando linhas ...";

			LimparTabela();

			var dataInicio = DateTime.Now;

			AdicionarNaLista();

			VarrerTabela();

			var dataFim = DateTime.Now;

			lbTotalLinhas.Content = "Total de linhas:" + Tabela.Items.Count;
			lbTempoProcessamento.Content = "Tempo de processamento:" + dataFim.Subtract(dataInicio).Milliseconds + " ms";
			Tabela.SelectedIndex = -1;

			base.Cursor = Cursors.Arrow;
		}

		private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender.GetType() == typeof(DataGridCell))
			{
				DataGridCell cell = sender as DataGridCell;
				cell.IsEditing = true;
			}
		}

		private bool Validacoes()
		{
			lbErro.Content = "";
			lbErro.Visibility = Visibility.Hidden;

			if (valorMin <= 0 || valorMin >= valorMax)
			{
				lbErro.Visibility = Visibility.Visible;
				lbErro.Content = "O valor mínimo não pode ser menor que 0 ou maior que o valor máximo!";
			}
			if (valorMax <= 0 || valorMax <= valorMin)
			{
				lbErro.Visibility = Visibility.Visible;
				lbErro.Content += "O valor máximo não pode ser menor que 0 ou menor que o valor mínimo!";
			}
			if (qtdLinhasGerar <= 0)
			{
				lbErro.Visibility = Visibility.Visible;
				lbErro.Content += "O número de linhas a ser gerado não pode ser menor que 0!";
			}

			if (!string.IsNullOrWhiteSpace(lbErro.Content.ToString()))
				return false;

			return true;
		}

		private void LimparTabela()
		{
			Tabela.ItemsSource = null;
			Tabela.Items.Clear();
			DoEvents();
		}

		private void VarrerTabela()
		{
			if (!deveVarrer)
				return;

			pbProcessamento.Maximum = Tabela.Items.Count;
			pbProcessamento.Visibility = Visibility.Visible;
			lbCpu.Visibility = Visibility.Visible;

			for (int i = 0; i < qtdLinhasGerar; i++)
			{
				Tabela.SelectedIndex = i;
				DoEvents();
				Tabela.ScrollIntoView(Tabela.Items[i]);
				pbProcessamento.Value = i;
				lbCpu.Content = "CPU:" + ObterUsoCpu();
			}

			pbProcessamento.Visibility = Visibility.Hidden;
			lbCpu.Visibility = Visibility.Hidden;
		}

		private void AdicionarNaLista()
		{
			foreach (var item in DadosGrid.ObterModelo(qtdLinhasGerar, valorMin, valorMax))
			{
				Tabela.Items.Add(item);
			}
		}

		private void SetarValores()
		{
			int.TryParse(tb_nrLinha.Text, out qtdLinhasGerar);
			int.TryParse(tb_valorMin.Text, out valorMin);
			int.TryParse(tb_valorMax.Text, out valorMax);
			deveVarrer = cb_varredura.IsChecked ?? false;
		}

		public string ObterUsoCpu()
		{
			return cpuCounter.NextValue() + "%";
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
}

