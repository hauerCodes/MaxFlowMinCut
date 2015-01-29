// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Node.cs" company="FH Wr. Neustadt">
//   Christoph Hauer & Markus Zytek
// </copyright>
// <summary>
//   The graph node.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MaxFlowMinCut.Lib.Network
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The graph node.
    /// </summary>
    [Serializable]
    public class Node
    {
        public Node(string name)
        {
            this.Name = name;
            this.Edges = new List<Edge>();
            this.Foreground = Color.Black;
            this.Fill = Color.White;
        }

        public List<Edge> Edges { get; set; }

        public string Name { get; set; }

        public Node ParentNode { get; set; }

        public Color Foreground { get; set; }

        public Color Fill { get; set; }
    }
}