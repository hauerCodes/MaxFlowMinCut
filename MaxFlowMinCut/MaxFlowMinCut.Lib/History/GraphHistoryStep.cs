using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlowMinCut.Lib.History
{
    public class GraphHistoryStep
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphHistoryStep"/> class.
        /// </summary>
        /// <param name="flowGraph">The flow graph.</param>
        public GraphHistoryStep(Graph flowGraph) : this(flowGraph, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphHistoryStep" /> class.
        /// </summary>
        /// <param name="flowGraph">The flow graph.</param>
        /// <param name="residualGraph">The residual graph.</param>
        public GraphHistoryStep(Graph flowGraph, Graph residualGraph)
        {
            this.FlowGraph = flowGraph;
            this.ResidualGraph = residualGraph; 
        }

        public Graph FlowGraph { get; private set; }

        public Graph ResidualGraph { get; private set; }
    }
}
