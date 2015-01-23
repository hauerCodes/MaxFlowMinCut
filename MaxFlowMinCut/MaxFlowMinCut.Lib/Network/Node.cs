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

    /// <summary>
    /// The graph node.
    /// </summary>
    [Serializable]
    public class Node
    {
        public List<Edge> EdgesFrom { get; set; }

        public string Name { get; set; }

        public Node ParentNode { get; set; }
    }
}