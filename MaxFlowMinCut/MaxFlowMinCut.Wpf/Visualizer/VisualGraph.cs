using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlowMinCut.Wpf.Visualizer
{
    using Microsoft.Msagl.Drawing;

    public class VisualGraph
    {
        private Lib.Graph graph;

        public VisualGraph(Lib.Graph graph)
        {
            this.graph = graph;
        }

        public Graph Create()
        {
            return new Graph();
        }
    }
}
