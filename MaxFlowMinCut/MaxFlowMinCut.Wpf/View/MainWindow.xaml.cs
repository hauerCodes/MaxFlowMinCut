using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaxFlowMinCut.Wpf.View
{
    using MaxFlowMinCut.Wpf.ViewModel;

    using Microsoft.Msagl.Drawing;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            this.viewModel = (MainWindowViewModel)this.DataContext;
            this.viewModel.FlowGraphChanged += OnFlowGraphChanged;
            this.viewModel.ResidualGraphChanged += OnResidualGraphChanged;

        }

        private void OnResidualGraphChanged(object sender, Graph graph)
        {
            this.GViewerResidual.Graph = graph;
        }

        private void OnFlowGraphChanged(object sender, Graph graph)
        {
            this.GViewerFlow.Graph = graph;
        }
    }
}
