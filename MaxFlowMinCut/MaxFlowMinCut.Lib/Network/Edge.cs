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
            this.ID = Guid.NewGuid();
            this.NodeFrom = nodeFrom;
            this.NodeTo = nodeTo;
            this.Capacity = capacity;
        }

        public Guid ID { get; set; }

        public Node NodeFrom { get; set; }

        public Node NodeTo { get; set; }

        public int Capacity { get; set; }

        public int Flow { get; set; }

        public string Label { get; set; }

        public bool IsPathMarked { get; set; }

        public bool IsVisited
        {
            get
            {
                return Flow > 0;
            }
        }

        public bool IsFullUsed
        {
            get
            {
                return (Flow == Capacity);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is minimum cut.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is minimum cut; otherwise, <c>false</c>.
        /// </value>
        public bool IsMinCut { get; set; }

        /// <summary>
        /// Equalses the specified compare edge.
        /// </summary>
        /// <param name="compareEdge">The compare edge.</param>
        /// <returns></returns>
        public bool Equals(Edge compareEdge)
        {
            return this.ID.Equals(compareEdge.ID);
        }
    }
}