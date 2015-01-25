// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FordFulkerson.cs" company="FH Wr. Neustadt">
//   Christoph Hauer & Markus Zytek
// </copyright>
// <summary>
//   The ford fulkerson algorithm.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MaxFlowMinCut.Lib.Algorithm
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using MaxFlowMinCut.Lib.Network;

    /// <summary>
    /// The ford fulkerson algorithm.
    /// </summary>
    public class FordFulkerson
    {
        public int MaxFlow
        {
            get
            {
                return maxFlow;
            }
        }

        private int maxFlow;

        //private int minCut;

        public int MinCut
        {
            get
            {
                return this.minCutEdges.Sum(e => e.Capacity);
            }
        }

        public Graph flowGraph;

        private Graph resiGraph;
        private List<Edge> minCutEdges = new List<Edge>();

        private List<Node> minCutNodes = new List<Node>();

        public FordFulkerson(Graph inputGraph, string sourceNodeName, string terminalNodeName)
        {
            this.flowGraph = (Graph)inputGraph.Clone();
            this.resiGraph = (Graph)inputGraph.Clone();

            foreach (var edge in this.flowGraph.Edges.ToList())
            {
                var residualEdge = new Edge(edge.NodeTo, edge.NodeFrom, 0);
                edge.NodeTo.Edges.Add(residualEdge);
                this.flowGraph.Edges.Add(residualEdge);
            }


            foreach (var edge in this.resiGraph.Edges.ToList())
            {
                var residualEdge = new Edge(edge.NodeTo, edge.NodeFrom, 0);
                edge.NodeTo.Edges.Add(residualEdge);
                this.resiGraph.Edges.Add(residualEdge);
            }

            Node sourceNode = this.flowGraph.Nodes.Find(n => n.Name.Equals(sourceNodeName));
            Node terminalNode = this.flowGraph.Nodes.Find(n => n.Name.Equals(terminalNodeName));

            Debug.WriteLine("\n** FordFulkerson");

            // MAX-FLOW 
            this.FindMaxFlow(sourceNode, terminalNode);
            Debug.WriteLine("Max-Flow: {0}", this.MaxFlow);

            // MIN-CUT
            this.FindMinCut(sourceNode);
            Debug.WriteLine("Min-Cut: {0}", this.MinCut);

            Debug.WriteLine("Min-Cut-Nodes:");
            this.minCutNodes.ForEach(n => Debug.WriteLine(n.Name));

            Debug.WriteLine("Min-Cut-Edges:");
            this.minCutEdges.ForEach(e => Debug.WriteLine("{0}--{1}-->{2}", e.NodeFrom.Name, e.Capacity, e.NodeTo.Name));
        }

        private void FindMaxFlow(Node sourceNode, Node terminalNode)
        {
            var path = this.BreadthFirstSearch(sourceNode, terminalNode);
            this.maxFlow = 0;

            while (path != null && path.Count > 0)
            {
                var minCapacity = path.Min(e => e.Capacity);

                this.AugmentedPath(path, minCapacity);
                this.maxFlow += minCapacity;
                path = this.BreadthFirstSearch(sourceNode, terminalNode);
            }
        }

        void AugmentedPath(IEnumerable<Edge> path, int minCapacity)
        {
            foreach (var edge in path)
            {
                edge.Capacity -= minCapacity;
                var residualEdge = edge.NodeTo.Edges.Find(e => e.NodeTo.Equals(edge.NodeFrom));
                residualEdge.Capacity += minCapacity;
            }
        }

        void FindMinCut(Node root)
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
            foreach (var node in this.resiGraph.Nodes.Where(p => this.minCutNodes.Any(r => r.Name.Equals(p.Name))).ToList())
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


        static List<Edge> GetPath(Node currentNode)
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
    }
}