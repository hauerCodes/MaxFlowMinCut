// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FordFulkerson.cs" company="FH Wr. Neustadt">
//   Christoph Hauer & Markus Zytek
// </copyright>
// <summary>
//   The ford fulkerson.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MaxFlowMinCut.Lib.Algorithm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using MaxFlowMinCut.Lib.Network;

    /// <summary>
    /// The ford fulkerson algorithm.
    /// </summary>
    public class FordFulkerson
    {
        private Lib.Graph flowGraph;

        private Lib.Graph resiGraph;

        //static Dictionary<int, Node> Nodes { get; set; }
        //static Dictionary<string, Edge> Edges { get; set; }
        private const float MaxValue = float.MaxValue;

        public FordFulkerson(Lib.Graph inputGraph)
        {
            this.flowGraph = (Lib.Graph)inputGraph.Clone();
            this.resiGraph = (Lib.Graph)inputGraph.Clone();
            //Nodes = new Dictionary<int, Node>();
            //Edges = new Dictionary<string, Edge>();
            this.Run();
        }

        /*public static void Main(string[] args)
        {
            new FordFulkerson().Run();
            Console.WriteLine("Press key to exit ...");
            Console.ReadKey();
        }*/

        /*void ParseData()
        {
            Reset();
            var names = new[] { "s", "2", "3", "4", "5", "t" };
            foreach (Node node in names.Select(name => new Node() { Name = name }))
            {
                Nodes.Add(node.Id, node);
            }


            var edges = new[] { "s 2 10", "s 3 10", "2 5 8", "2 3 2", "2 4 4", "3 5 9", "4 t 10", "5 4 6", "5 t 10" };

            foreach (var edge in edges)
            {
                string[] s = edge.Split(' ');

                Node node1 = Nodes.First(p => p.Value.Name.Equals(s[0])).Value;
                Node node2 = Nodes.First(p => p.Value.Name.Equals(s[1])).Value;
                float capacity = float.Parse(s[2]);

                AddEdge(node1, node2, capacity);
                AddEdge(node2, node1, 0f); // residual, if undirected graph, set value as capacity
            }
        }*/

        public void Run()
        {
            ParseData();
            Algo();
        }

        void Algo()
        {
            var nodeSource = Nodes[0];
            var nodeTerminal = Nodes[Nodes.Count - 1];

            PrintNodes();

            this.FordFulkersonAlgo(nodeSource, nodeTerminal);
        }


        void FordFulkersonAlgo(Node nodeSource, Node nodeTerminal)
        {
            Console.WriteLine("\n** FordFulkerson");
            var flow = 0f;

            var path = Bfs(nodeSource, nodeTerminal);

            while (path != null && path.Count > 0)
            {
                var minCapacity = MaxValue;
                foreach (var edge in path)
                {
                    if (edge.Capacity < minCapacity)
                    {
                        minCapacity = edge.Capacity; // update
                    }
                }

                if (minCapacity == MaxValue || minCapacity < 0)
                {
                    throw new Exception("minCapacity " + minCapacity);
                }

                AugmentPath(path, minCapacity);
                flow += minCapacity;

                path = this.Bfs(nodeSource, nodeTerminal);
            }

            // max flow
            Console.WriteLine("\n** Max flow = " + flow);

            // min cut
            Console.WriteLine("\n** Min cut");
            this.FindMinCut(nodeSource);
        }


        static void AugmentPath(IEnumerable<Edge> path, float minCapacity)
        {
            foreach (var edge in path)
            {
                var keyResidual = GetKey(edge.NodeTo.Id, edge.NodeFrom.Id);
                var edgeResidual = Edges[keyResidual];

                edge.Capacity -= minCapacity;
                edgeResidual.Capacity += minCapacity;
            }
        }

        // similar to bfs (breadth-first search)
        void FindMinCut(Node root)
        {
            var queue = new Queue<Node>();
            var discovered = new HashSet<Node>();
            var minCutNodes = new List<Node>();
            var minCutEdges = new List<Edge>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (discovered.Contains(current))
                {
                    continue;
                }

                minCutNodes.Add(current);
                discovered.Add(current);

                var edges = current.NodeEdges;
                foreach (var edge in edges)
                {
                    var next = edge.NodeTo;
                    if (edge.Capacity <= 0 || discovered.Contains(next))
                    {
                        continue;
                    }

                    queue.Enqueue(next);
                    minCutEdges.Add(edge);
                }
            }

            // bottleneck as a list of arcs
            var minCutResult = new List<Edge>();
            List<int> nodeIds = minCutNodes.Select(node => node.Id).ToList();

            var nodeKeys = new HashSet<int>();
            foreach (var node in minCutNodes)
            {
                nodeKeys.Add(node.Id);
            }

            var edgeKeys = new HashSet<string>();
            foreach (var edge in minCutEdges)
            {
                edgeKeys.Add(edge.Name);
            }


            this.ParseData(); // reset the graph

            // finding by comparing residual and original graph

            foreach (var id in nodeIds)
            {
                var node = Nodes[id];
                var edges = node.NodeEdges;
                foreach (var edge in edges)
                {
                    if (nodeKeys.Contains(edge.NodeTo.Id))
                    {
                        continue;
                    }

                    if (edge.Capacity > 0 && !edgeKeys.Contains(edge.Name))
                    {
                        minCutResult.Add(edge);
                    }
                }
            }

            float maxflow = 0;
            foreach (var edge in minCutResult)
            {
                maxflow += edge.Capacity;
                Console.WriteLine(edge.Info());
            }

            Console.WriteLine("min-cut total maxflow = " + maxflow);
        }

        /*
           Customized for network flow, capacity
          
            Wikipedia
            1. Enqueue the root node.
            2. Dequeue a node and examine it.
                * If the element sought is found in this node, quit the search and return a result.
                * Otherwise enqueue any successors (the direct child nodes) that haven't been seen.
            3. If the queue is empty, every node on the graph has been examined 
                – quit the search and return "not found".
            4. Repeat from Step 2.
         */
        private List<Edge> Bfs(Node root, Node target) // breadth-first search
        {
            root.TraverseParent = null;
            target.TraverseParent = null; //reset

            var queue = new Queue<Node>();
            var discovered = new HashSet<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                discovered.Add(current);

                if (current.Id == target.Id)
                {
                    return GetPath(current);
                }

                var nodeEdges = current.NodeEdges;
                foreach (var edge in nodeEdges)
                {
                    var next = edge.NodeTo;
                    var c = this.GetCapacity(current, next);
                    if (c > 0 && !discovered.Contains(next))
                    {
                        next.TraverseParent = current;
                        queue.Enqueue(next);
                    }
                }
            }

            return null;
        }


        static List<Edge> GetPath(Node node)
        {
            var path = new List<Edge>();
            var current = node;
            while (current.TraverseParent != null)
            {
                var key = GetKey(current.TraverseParent.Id, current.Id);
                var edge = Edges[key];
                path.Add(edge);
                current = current.TraverseParent;
            }

            return path;
        }

        public static string GetKey(int id1, int id2)
        {
            return id1 + "|" + id2;
        }

        public float GetCapacity(Node node1, Node node2)
        {
            var edge = Edges[GetKey(node1.Id, node2.Id)];
            return edge.Capacity;
        }

        public void AddEdge(Node nodeFrom, Node nodeTo, float capacity)
        {
            var key = GetKey(nodeFrom.Id, nodeTo.Id);
            var edge = new Edge() { NodeFrom = nodeFrom, NodeTo = nodeTo, Capacity = capacity, Name = key };
            Edges.Add(key, edge);
            nodeFrom.NodeEdges.Add(edge);
        }


        static void PrintNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];
                Console.WriteLine(node + " outnodes=" + node.GetInfo());
            }
        }

        static void Reset()
        {
            Nodes.Clear();
            Edges.Clear();
            //Node.ResetCounter();
        }

        /*public class Node
        {
            private static int _counter;
            public readonly int Id;
            public string Name { get; set; }
            public List<Edge> NodeEdges { get; set; }
            public Node TraverseParent { get; set; }

            public Node()
            {
                Id = _counter++;
                NodeEdges = new List<Edge>();
            }

            public static void ResetCounter()
            {
                _counter = 0;
            }

            public string GetInfo()
            {
                var sb = new StringBuilder();
                foreach (var edge in this.NodeEdges)
                {
                    var node = edge.NodeTo;
                    if (edge.Capacity > 0)
                    {
                        sb.Append(node.Name + "C" + edge.Capacity + " ");
                    }
                }
                return sb.ToString();
            }

            public override string ToString()
            {
                return string.Format("Id={0}, Name={1}", this.Id, this.Name);
            }
        }*/
        /*public class Edge
        {
            public Node NodeFrom { get; set; }
            public Node NodeTo { get; set; }
            public float Capacity { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return string.Format("NodeFrom={0}, NodeTo={1}, C={2}", this.NodeFrom.Name, this.NodeTo.Name, this.Capacity);
            }

            public string Info()
            {
                return string.Format("NodeFrom=({0}), NodeTo=({1}), C={2}", this.NodeFrom, this.NodeTo, this.Capacity);
            }
        }*/
    }
}