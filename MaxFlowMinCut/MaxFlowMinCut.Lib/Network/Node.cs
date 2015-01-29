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
            this.ID = Guid.NewGuid();
            this.Name = name;
            this.Edges = new List<Edge>();
        }

        public Guid ID { get; set; }

        public List<Edge> Edges { get; set; }

        public string Name { get; set; }

        public Node ParentNode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is minimum cut.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is minimum cut; otherwise, <c>false</c>.
        /// </value>
        public bool IsMinCut { get; set; }

        public bool Equals(Node compareNode)
        {
            return this.ID.Equals(compareNode.ID); 
        }
    }
}