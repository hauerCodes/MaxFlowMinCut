using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlowMinCut.Wpf.Visualizer
{
    using Microsoft.Msagl.Drawing;
    using Microsoft.Msagl.Mds;

    public class VisualGraph
    {
        private Lib.Graph libGraph;

        public VisualGraph(Lib.Graph libGraph)
        {
            this.libGraph = libGraph;
        }

        private Graph CreateGraph()
        {
            Graph msaglGraph = new Graph();
            msaglGraph.Attr.OptimizeLabelPositions = true;
            msaglGraph.Attr.LayerDirection = LayerDirection.LR;

            return msaglGraph;
        }

        public Graph CreateFlowGraph()
        {
            var msaglGraph = this.CreateGraph();

            foreach (var edge in this.libGraph.Edges)
            {
                var addedge = msaglGraph.AddEdge(edge.NodeFrom.Name, string.Format("{0}/{1}", edge.Flow, edge.Capacity), edge.NodeTo.Name);
                addedge.Attr.Color = new Color(edge.Foreground.R, edge.Foreground.G, edge.Foreground.B);
                addedge.Attr.LineWidth = edge.Thickness;
                addedge.SourceNode.Attr.Shape = Shape.Circle;
                addedge.TargetNode.Attr.Shape = Shape.Circle;
            }
            
            return msaglGraph;
        }

        public Graph CreateResidualGraph()
        {
            var msaglGraph = this.CreateGraph();

            foreach (var edge in this.libGraph.Edges)
            {
                var addedge = msaglGraph.AddEdge(edge.NodeFrom.Name, string.Format("{0}", edge.Capacity), edge.NodeTo.Name);
                addedge.Attr.Color = new Color(edge.Foreground.R, edge.Foreground.G, edge.Foreground.B);
                addedge.SourceNode.Attr.Shape = Shape.Circle;
                addedge.TargetNode.Attr.Shape = Shape.Circle;
            }

            return msaglGraph;
        }
    }
}
