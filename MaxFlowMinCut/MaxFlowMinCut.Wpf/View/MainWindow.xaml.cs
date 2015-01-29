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
    using MaxFlowMinCut.Wpf.Visualizer;

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
            this.viewModel.FlowGraphChanged += this.OnFlowGraphChanged;
            this.viewModel.ResidualGraphChanged += this.OnResidualGraphChanged;
        }

        private void OnFlowGraphChanged(object sender, Lib.Graph libGraph)
        {
            var oldGraph = this.GViewerFlow.Graph;

            //oldGraph.FindNode("a").Attr.Pos;
            
            VisualGraph visualGraph = new VisualGraph(libGraph);
            Graph msaglGraph = visualGraph.CreateFlowGraph();
            //this.GViewerFlow.NeedToCalculateLayout = true;
            this.GViewerFlow.Graph = msaglGraph;
            //this.GViewerFlow.NeedToCalculateLayout = false;
        }

        private void OnResidualGraphChanged(object sender, Lib.Graph libGraph)
        {
            if (libGraph != null)
            {
                VisualGraph visualGraph = new VisualGraph(libGraph);
                Graph msaglGraph = visualGraph.CreateResidualGraph();
                this.GViewerResidual.Graph = msaglGraph;
            }
            else
            {
                this.GViewerResidual.Graph = null;
            }
        }
    }
}
