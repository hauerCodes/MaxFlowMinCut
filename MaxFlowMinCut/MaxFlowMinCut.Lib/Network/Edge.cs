// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Edge.cs" company="FH Wr. Neustadt">
//   Christoph Hauer & Markus Zytek
// </copyright>
// <summary>
//   The edge.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Lib.Network
{
    using System;
    using System.Drawing;

    /// <summary>
    /// The edge.
    /// </summary>
    [Serializable]
    public class Edge
    {
        public Edge(Node nodeFrom, Node nodeTo, int capacity)
        {
            this.NodeFrom = nodeFrom;
            this.NodeTo = nodeTo;
            this.Capacity = capacity;
            this.Foreground = Color.Black;
            this.Thickness = 1;
        }

        public Node NodeFrom { get; set; }

        public Node NodeTo { get; set; }

        public int Capacity { get; set; }

        public int Flow { get; set; }

        public string Label { get; set; }

        public Color Foreground { get; set; }

        public int Thickness{ get; set; }
    }
}