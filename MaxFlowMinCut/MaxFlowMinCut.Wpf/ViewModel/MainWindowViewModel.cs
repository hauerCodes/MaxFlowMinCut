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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using MaxFlowMinCut.Lib.Network;
    using MaxFlowMinCut.Wpf.Model;
    using MaxFlowMinCut.Wpf.Visualizer;
    using MaxFlowMinCut.Lib;
    using MaxFlowMinCut.Lib.Algorithm;

    /// <summary>
    /// The main window view model.
    /// </summary>
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<InputEdge> InputEdges { get; set; }

        public MainWindowViewModel()
        {
            this.InputEdges = new ObservableCollection<InputEdge>();

            // TEST-GRAPH
            this.InputEdges.Add(new InputEdge("s", "2", 10));
            this.InputEdges.Add(new InputEdge("s", "3", 10));
            this.InputEdges.Add(new InputEdge("2", "5", 8));
            this.InputEdges.Add(new InputEdge("2", "3", 2));
            this.InputEdges.Add(new InputEdge("2", "4", 4));
            this.InputEdges.Add(new InputEdge("3", "5", 9));
            this.InputEdges.Add(new InputEdge("4", "t", 10));
            this.InputEdges.Add(new InputEdge("5", "4", 6));
            this.InputEdges.Add(new InputEdge("5", "t", 10));

            this.VisualizeFlowGraphCommand = new RelayCommand(
                this.VisualizeFlowGraph,
                this.CanExecuteVisualizeFlowGraphCommand);
        }

        private bool CanExecuteVisualizeFlowGraphCommand()
        {
            return this.InputEdges.Count > 0;
        }

        private void VisualizeFlowGraph()
        {
            Graph graph = ConvertInputEdgesToGraph(this.InputEdges);
            this.RaiseFlowGraphChanged(this, graph);

            FordFulkerson fordFulkerson = new FordFulkerson(graph, "s", "t");
            this.RaiseResidualGraphChanged(this, fordFulkerson.flowGraph);
        }

        private static Graph ConvertInputEdgesToGraph(IEnumerable<InputEdge> inputEdges)
        {
            Graph graph = new Graph();

            foreach (var inputEdge in inputEdges)
            {
                var nodeFrom = graph.Nodes.Find(n => n.Name.Equals(inputEdge.NodeFrom));
                if (nodeFrom == null)
                {
                    nodeFrom = new Node(inputEdge.NodeFrom);
                    graph.Nodes.Add(nodeFrom);
                }

                var nodeTo = graph.Nodes.Find(n => n.Name.Equals(inputEdge.NodeTo));
                if (nodeTo == null)
                {
                    nodeTo = new Node(inputEdge.NodeTo);
                    graph.Nodes.Add(nodeTo);
                }

                var edge = new Edge(nodeFrom, nodeTo, inputEdge.Capacity);
                nodeFrom.Edges.Add(edge);
                graph.Edges.Add(edge);
            }

            return graph;
        }

        public RelayCommand VisualizeFlowGraphCommand { get; set; }

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
        private void RaiseFlowGraphChanged(object sender, Graph graph)
        {
            if (this.FlowGraphChanged != null)
            {
                this.FlowGraphChanged(sender, graph);
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
        private void RaiseResidualGraphChanged(object sender, Graph graph)
        {
            if (this.ResidualGraphChanged != null)
            {
                this.ResidualGraphChanged(sender, graph);
            }
        }
    }
}