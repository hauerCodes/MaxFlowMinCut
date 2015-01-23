// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="FH Wr. Neustadt">
//   Christoph Hauer & Markus Zytek
// </copyright>
// <summary>
//   The main window view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Wpf.ViewModel
{
    using System;
    using System.Collections.ObjectModel;

    using MaxFlowMinCut.Wpf.Model;
    using MaxFlowMinCut.Wpf.Visualizer;
    using Microsoft.Msagl.Drawing;

    /// <summary>
    /// The main window view model.
    /// </summary>
    internal class MainWindowViewModel
    {
        public ObservableCollection<InputEdge> InputEdges { get; set; }

        public MainWindowViewModel()
        {
            this.InputEdges = new ObservableCollection<InputEdge>();
            this.InputEdges.Add(new InputEdge("s", ));
        }

        /// <summary>
        /// The flow graph changed.
        /// </summary>
        public event EventHandler<Graph> FlowGraphChanged;

        /// <summary>
        /// The residual graph changed.
        /// </summary>
        public event EventHandler<Graph> ResidualGraphChanged;

        /// <summary>
        /// The raise flow graph changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void RaiseFlowGraphChanged(object sender, Lib.Graph graph)
        {
            if (this.FlowGraphChanged != null)
            {
                var visualGraph = new VisualGraph(graph);
                this.FlowGraphChanged(sender, visualGraph.Create());
            }
        }

        /// <summary>
        /// The raise residual graph changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void RaiseResidualGraphChanged(object sender, Lib.Graph graph)
        {
            if (this.ResidualGraphChanged != null)
            {
                var visualGraph = new VisualGraph(graph);
                this.ResidualGraphChanged(sender, visualGraph.Create());
            }
        }
    }
}