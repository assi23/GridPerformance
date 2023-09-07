using GridPerformance.Modelos;
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
		private int qtdLinhasGerar;
		private int valorMin;
		private int valorMax;
		private bool deveVarrer;
		public MainWindow()
		{
			InitializeComponent();
			Tabela.BeginningEdit += (s, ss) => ss.Cancel = true;
		}

		private void SetarValores()
		{
			int.TryParse(tb_nrLinha.Text, out qtdLinhasGerar);
			int.TryParse(tb_valorMin.Text, out valorMin);
			int.TryParse(tb_valorMax.Text, out valorMax);
			deveVarrer = cb_varredura.IsChecked ?? false;
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			base.Cursor = Cursors.Wait;

			lbTempoProcessamento.Content = "Gerando linhas ...";

			SetarValores();

			LimparTabela();

			var dataInicio = DateTime.Now;

			AdicionarNaLista();

			VarrerTabela();

			var dataFim = DateTime.Now;

			lbTotalLinhas.Content = "Total de linhas:" + Tabela.Items.Count;
			lbTempoProcessamento.Content = "Tempo de processamento:" + dataFim.Subtract(dataInicio).Milliseconds + " ms";

			base.Cursor = Cursors.Arrow;
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

			for (int i = 0; i < Tabela.Items.Count; i++)
			{
				Tabela.SelectedIndex = i;
				DoEvents();
				Tabela.ScrollIntoView(Tabela.Items[i]);
				pbProcessamento.Value = i;
			}

			pbProcessamento.Visibility = Visibility.Hidden;
		}

		private void AdicionarNaLista()
		{
			foreach (var item in DadosGrid.ObterModelo(qtdLinhasGerar, valorMin, valorMax))
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
}

