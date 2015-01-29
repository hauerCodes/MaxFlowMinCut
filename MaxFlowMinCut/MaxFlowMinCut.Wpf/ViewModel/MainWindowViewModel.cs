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
    using MaxFlowMinCut.Lib.History;
    using MaxFlowMinCut.Wpf.View;

    using Mutzl.MvvmLight;

    using Application = System.Windows.Application;

    /// <summary>
    /// The main window view model.
    /// </summary>
    internal class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The visualized graph
        /// </summary>
        private Graph visualizedGraph;

        /// <summary>
        /// The current step
        /// </summary>
        private int currentStep = 0;

        /// <summary>
        /// The graph steps history.
        /// </summary>
        private GraphHistory graphSteps;

        public MainWindowViewModel()
        {
            InitializeGraph();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            this.VisualizeFlowGraphCommand = new RelayCommand(
                this.ExecuteVisualizeFlowGraph,
                this.CanExecuteVisualizeFlowGraphCommand);

            this.CalculateCommand = new DependentRelayCommand(
                this.ExecuteCalculateGraph,
                () => this.IsVisualized,
                this,
                () => this.IsVisualized);

            this.ClearGraphCommand = new RelayCommand(
                this.ExecuteClearGraph);

            this.StepForwardCommand = new DependentRelayCommand(
                this.ExecuteVisualizeNextGraphStep,
                () => this.IsCalculated,
                this,
                () => this.IsCalculated);

            this.ShowGraphHistoryCommand = new DependentRelayCommand(
                this.ExecuteShowGraphHistory,
            () => this.IsCalculated,
            this,
            () => this.IsCalculated);
        }

        private void ExecuteShowGraphHistory()
        {
            HistoryView view = new HistoryView 
            {
                DataContext = new HistoryViewModel(this.graphSteps)
            };

            view.Show();
        }

        /// <summary>
        /// Executes the visualize next graph step.
        /// </summary>
        private void ExecuteVisualizeNextGraphStep()
        {
            if (currentStep < graphSteps.MaxStep)
            {
                RaiseFlowGraphChanged(this, graphSteps[currentStep].FlowGraph);
                RaiseResidualGraphChanged(this, graphSteps[currentStep].ResidualGraph);
                currentStep++;
            }
        }

        /// <summary>
        /// Clears the graph.
        /// </summary>
        private void ExecuteClearGraph()
        {
            this.InputEdges.Clear();
            this.IsVisualized = false;
        }

        private void ExecuteCalculateGraph()
        {
            FordFulkerson fordFulkerson = new FordFulkerson(visualizedGraph, "s", "t");
            //this.RaiseResidualGraphChanged(this, graph);

            graphSteps = fordFulkerson.RunAlgorithm();
            IsCalculated = true;
        }

        private void InitializeGraph()
        {
            this.InputEdges = new ObservableCollection<InputEdge>();
            this.InputEdges.CollectionChanged += (sender, e) =>
            {
                this.IsVisualized = false;
                this.IsCalculated = false;
            };

            // TEST-GRAPH
            this.InputEdges.Add(new InputEdge("s", "2", 10));
            this.InputEdges.Add(new InputEdge("s", "3", 5));
            this.InputEdges.Add(new InputEdge("s", "4", 15));

            this.InputEdges.Add(new InputEdge("2", "3", 4));
            this.InputEdges.Add(new InputEdge("2", "5", 9));
            this.InputEdges.Add(new InputEdge("2", "6", 15));

            this.InputEdges.Add(new InputEdge("3", "6", 8));
            this.InputEdges.Add(new InputEdge("3", "4", 4));

            this.InputEdges.Add(new InputEdge("4", "7", 30));

            this.InputEdges.Add(new InputEdge("5", "6", 15));
            this.InputEdges.Add(new InputEdge("5", "t", 10));

            this.InputEdges.Add(new InputEdge("6", "7", 15));
            this.InputEdges.Add(new InputEdge("6", "t", 10));

            this.InputEdges.Add(new InputEdge("7", "3", 6));
            this.InputEdges.Add(new InputEdge("7", "t", 10));
        }

        public bool IsVisualized { get; private set; }

        public bool IsCalculated { get; private set; }

        public RelayCommand PlayStepsCommand { get; private set; }
        public RelayCommand StopCommand { get; private set; }
        public DependentRelayCommand StepForwardCommand { get; private set; }
        public RelayCommand StepBackCommand { get; private set; }
        public RelayCommand GoToStartCommand { get; private set; }
        public RelayCommand GoToEndCommand { get; private set; }
        public RelayCommand ClearGraphCommand { get; private set; }
        public DependentRelayCommand CalculateCommand { get; private set; }
        public RelayCommand VisualizeFlowGraphCommand { get; private set; }

        public DependentRelayCommand ShowGraphHistoryCommand { get; set; }

        public ObservableCollection<InputEdge> InputEdges { get; private set; }


        private bool CanExecuteVisualizeFlowGraphCommand()
        {
            return this.InputEdges.Count > 0;
        }

        private void ExecuteVisualizeFlowGraph()
        {
            visualizedGraph = ConvertInputEdgesToGraph(this.InputEdges);
            this.RaiseFlowGraphChanged(this, visualizedGraph);

            this.IsVisualized = true;

            //FordFulkerson fordFulkerson = new FordFulkerson(graph, "s", "t");
            //this.RaiseResidualGraphChanged(this, graph);
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.FlowGraphChanged(sender, graph);
                });
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.ResidualGraphChanged(sender, graph);
                });
            }
        }
    }
}