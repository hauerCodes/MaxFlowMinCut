// -----------------------------------------------------------------------
// <copyright file="FordFulkerson.cs" company="FH Wr.Neustadt">
//      Copyright Christoph Hauer. All rights reserved.
// </copyright>
// <author>Christoph Hauer</author>
// <summary>MaxFlowMinCut.Lib - FordFulkerson.cs</summary>
// -----------------------------------------------------------------------
namespace MaxFlowMinCut.Lib.Algorithm
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;

    using MaxFlowMinCut.Lib.History;
    using MaxFlowMinCut.Lib.Network;

    /// <summary>
    /// The ford fulkerson algorithm.
    /// </summary>
    public class FordFulkerson
    {
        /// <summary>
        /// The flow graph.
        /// </summary>
        private Graph flowGraph;

        /// <summary>
        /// The max flow.
        /// </summary>
        private int maxFlow;

        /// <summary>
        /// The min cut edges.
        /// </summary>
        private List<Edge> minCutEdges = new List<Edge>();

        /// <summary>
        /// The min cut nodes.
        /// </summary>
        private List<Node> minCutNodes = new List<Node>();

        /// <summary>
        /// The residual graph
        /// </summary>
        private Graph residualGraph;

        /// <summary>
        /// The source node name.
        /// </summary>
        private string sourceNodeName;

        /// <summary>
        /// The terminal node name.
        /// </summary>
        private string terminalNodeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FordFulkerson"/> class.
        /// </summary>
        /// <param name="inputGraph">
        /// The input graph.
        /// </param>
        /// <param name="sourceNodeName">
        /// Name of the source node.
        /// </param>
        /// <param name="terminalNodeName">
        /// Name of the terminal node.
        /// </param>
        public FordFulkerson(Graph inputGraph, string sourceNodeName, string terminalNodeName)
        {
            this.sourceNodeName = sourceNodeName;
            this.terminalNodeName = terminalNodeName;

            Initialize(inputGraph);

            //foreach (var edge in this.residualGraph.Edges.ToList())
            //{
            //    var residualEdge = new Edge(edge.NodeTo, edge.NodeFrom, 0);
            //    edge.NodeTo.Edges.Add(residualEdge);
            //    this.residualGraph.Edges.Add(residualEdge);
            //}
        }

        /// <summary>
        /// Initializes the specified input graph.
        /// </summary>
        /// <param name="inputGraph">The input graph.</param>
        private void Initialize(Graph inputGraph)
        {
            this.flowGraph = (Graph)inputGraph.Clone();
            this.residualGraph = (Graph)inputGraph.Clone();

            foreach (var edge in this.residualGraph.Edges.ToList())
            {
                var residualEdge = new Edge(edge.NodeTo, edge.NodeFrom, 0);
                edge.NodeTo.Edges.Add(residualEdge);
                this.residualGraph.Edges.Add(residualEdge);
            }
        }

        /// <summary>
        /// Gets the max flow.
        /// </summary>
        public int MaxFlow
        {
            get
            {
                return this.maxFlow;
            }
        }

        /// <summary>
        /// Gets the min cut.
        /// </summary>
        public int MinCut
        {
            get
            {
                return this.minCutEdges.Sum(e => e.Capacity);
            }
        }

        /// <summary>
        /// The graph history.
        /// </summary>
        private GraphHistory graphHistory;

        /// <summary>
        /// The run algorithm.
        /// </summary>
        /// <returns>
        /// The <see cref="GraphHistory"/>.
        /// </returns>
        public GraphHistory RunAlgorithm()
        {
            graphHistory = new GraphHistory();

            Node sourceNode = this.residualGraph.Nodes.Find(n => n.Name.Equals(this.sourceNodeName));
            Node terminalNode = this.residualGraph.Nodes.Find(n => n.Name.Equals(this.terminalNodeName));

            // add initial step to graph history
            graphHistory.AddGraphStep(this.flowGraph, this.residualGraph);

            Debug.WriteLine("\n** FordFulkerson");

            // MAX-FLOW 
            this.FindMaxFlow(sourceNode, terminalNode);
            Debug.WriteLine("Max-Flow: {0}", this.MaxFlow);

            // MIN-CUT
            //this.FindMinCut(sourceNode);
            Debug.WriteLine("Min-Cut: {0}", this.MinCut);

            Debug.WriteLine("Min-Cut-Nodes:");
            this.minCutNodes.ForEach(n => Debug.WriteLine(n.Name));

            Debug.WriteLine("Min-Cut-Edges:");
            this.minCutEdges.ForEach(e => Debug.WriteLine("{0}--{1}-->{2}", e.NodeFrom.Name, e.Capacity, e.NodeTo.Name));

            return graphHistory;
        }

        /// <summary>
        /// The get path.
        /// </summary>
        /// <param name="currentNode">
        /// The current node.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private static List<Edge> GetPath(Node currentNode)
        {
            var path = new List<Edge>();

            while (currentNode.ParentNode != null)
            {
                var parentEdge = currentNode.ParentNode.Edges.Find(e => e.NodeTo.Equals(currentNode));
                path.Add(parentEdge);
                currentNode = currentNode.ParentNode;
            }

            return path;
        }

        /// <summary>
        /// The augmented path.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="minCapacity">
        /// The min capacity.
        /// </param>
        private void AugmentedPath(IEnumerable<Edge> path, int minCapacity)
        {
            foreach (var edge in path)
            {
                edge.Capacity -= minCapacity;
                var residualEdge = edge.NodeTo.Edges.Find(e => e.NodeTo.Equals(edge.NodeFrom));
                residualEdge.Capacity += minCapacity;
            }
        }

        /// <summary>
        /// The breadth first search.
        /// </summary>
        /// <param name="sourceNode">
        /// The source node.
        /// </param>
        /// <param name="targetNode">
        /// The target node.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<Edge> BreadthFirstSearch(Node sourceNode, Node targetNode)
        {
            sourceNode.ParentNode = null;
            targetNode.ParentNode = null;

            var queuedNodes = new Queue<Node>();
            var discoveredNodes = new List<Node>();
            queuedNodes.Enqueue(sourceNode);

            while (queuedNodes.Count > 0)
            {
                var currentNode = queuedNodes.Dequeue();
                discoveredNodes.Add(currentNode);

                if (currentNode.Name == targetNode.Name)
                {
                    return GetPath(currentNode);
                }

                var currentNodeEdges = currentNode.Edges;
                foreach (var edge in currentNodeEdges)
                {
                    var nodeTo = edge.NodeTo;
                    if (edge.Capacity > 0 && !discoveredNodes.Contains(nodeTo))
                    {
                        nodeTo.ParentNode = currentNode;
                        queuedNodes.Enqueue(nodeTo);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The find max flow.
        /// </summary>
        /// <param name="sourceNode">
        /// The source node.
        /// </param>
        /// <param name="terminalNode">
        /// The terminal node.
        /// </param>
        private void FindMaxFlow(Node sourceNode, Node terminalNode)
        {
            this.maxFlow = 0;

            List<Edge> path = this.BreadthFirstSearch(sourceNode, terminalNode);

            // color found path
            HighlightFoundPath(path, Color.Orange);
            graphHistory.AddGraphStep(this.flowGraph, this.residualGraph);
            ResetHighlight(path);

            while (path != null && path.Count > 0)
            {
                var minCapacity = path.Min(e => e.Capacity);

                this.AugmentedPath(path, minCapacity);

                //TODO update flow values in flow graph
                List<Edge> updatedPath = this.UpdateFlowValuesFlowGraph(path, minCapacity);

                //TODO color augmented path in flow graph!
                HighlightFoundPath(updatedPath, Color.Blue);
                graphHistory.AddGraphStep(this.flowGraph);
                //ResetHighlight(updatedPath);

                this.maxFlow += minCapacity;
                path = this.BreadthFirstSearch(sourceNode, terminalNode);

                HighlightFoundPath(path, Color.Orange);
                graphHistory.AddGraphStep(this.flowGraph, this.residualGraph);
                ResetHighlight(path);
            }
        }

        private List<Edge> UpdateFlowValuesFlowGraph(List<Edge> path, int minCapacity)
        {
            List<Edge> updatedPath = new List<Edge>();

            foreach (var edge in path)
            {
                Edge found = flowGraph.Edges.FirstOrDefault(e => e.NodeFrom.Name.Equals(edge.NodeFrom.Name) && e.NodeTo.Name.Equals(edge.NodeTo.Name));

                if (found != null)
                {
                    found.Flow += minCapacity;
                }
                else
                {
                    found = flowGraph.Edges.FirstOrDefault(e => e.NodeTo.Name.Equals(edge.NodeFrom.Name) && e.NodeFrom.Name.Equals(edge.NodeTo.Name));

                    if (found != null)
                    {
                        found.Flow += minCapacity;
                    }
                }

                if (found != null && found.Flow == found.Capacity)
                {
                    found.Thickness = 3;
                }

                updatedPath.Add(found);
            }
            return updatedPath;
        }

        /// <summary>
        /// Highlights the found path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="highlightColor">Color of the highlight.</param>
        private void HighlightFoundPath(List<Edge> path, Color highlightColor)
        {
            if (path != null)
            {
                path.ForEach(edge => edge.Foreground = highlightColor);
            }
        }

        /// <summary>
        /// Resets the highlight.
        /// </summary>
        /// <param name="path">The path.</param>
        private void ResetHighlight(List<Edge> path)
        {
            if (path != null)
            {
                path.ForEach(edge => edge.Foreground = Color.Black);
            }
        }

        /// <summary>
        /// The find min cut.
        /// </summary>
        /// <param name="root">
        /// The root.
        /// </param>
        private void FindMinCut(Node root)
        {
            var queuedNodes = new Queue<Node>();
            var discoveredNodes = new List<Node>();

            this.minCutNodes.Clear();
            this.minCutEdges.Clear();

            queuedNodes.Enqueue(root);

            while (queuedNodes.Count > 0)
            {
                var currentNode = queuedNodes.Dequeue();
                if (!discoveredNodes.Contains(currentNode))
                {
                    this.minCutNodes.Add(currentNode);
                    discoveredNodes.Add(currentNode);

                    var edges = currentNode.Edges;
                    foreach (var edge in edges)
                    {
                        var nodeTo = edge.NodeTo;
                        if (edge.Capacity > 0 && !discoveredNodes.Contains(nodeTo))
                        {
                            queuedNodes.Enqueue(nodeTo);
                            this.minCutEdges.Add(edge);
                        }
                    }
                }
            }

            this.minCutEdges = new List<Edge>();
            foreach (
                var node in this.residualGraph.Nodes.Where(p => this.minCutNodes.Any(r => r.Name.Equals(p.Name))).ToList())
            {
                var edges = node.Edges;
                foreach (var edge in edges)
                {
                    if (this.minCutNodes.Any(p => p.Name.Equals(edge.NodeTo.Name)))
                    {
                        continue;
                    }

                    if (edge.Capacity > 0 && !this.minCutEdges.Contains(edge))
                    {
                        this.minCutEdges.Add(edge);
                    }
                }
            }
        }
    }
}