// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Graph.cs" company="FH Wr. Neustadt">
//   Christoph Hauer & Markus Zytek
// </copyright>
// <summary>
//   The graph.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MaxFlowMinCut.Lib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    using MaxFlowMinCut.Lib.Network;

    /// <summary>
    /// The graph.
    /// </summary>
    [Serializable]
    public class Graph : ICloneable
    {
        public List<Node> Nodes { get; set; }

        public List<Edge> Edges { get; set; }

        public object Clone()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, this);
            Graph graph = (Graph)binaryFormatter.Deserialize(memoryStream);

            return graph;
        }
    }
}