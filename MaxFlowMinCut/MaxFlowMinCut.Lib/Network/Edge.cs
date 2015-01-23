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

    /// <summary>
    /// The edge.
    /// </summary>
    [Serializable]
    public class Edge
    {
        public Node From { get; set; }

        public Node To { get; set; }

        public int Capacity { get; set; }

        public int Flow { get; set; }

        public int Label { get; set; }
    }
}