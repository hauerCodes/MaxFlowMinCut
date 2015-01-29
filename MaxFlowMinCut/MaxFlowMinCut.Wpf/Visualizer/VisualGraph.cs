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
                addedge.Attr.LineWidth = 1;

                if (edge.IsVisited)
                {
                    addedge.Attr.Color = Color.Blue;
                    if (edge.IsFullUsed)
                    {
                        addedge.Attr.LineWidth = 3;
                    }
                }
                //else
                //{
                //    addedge.Attr.Color = Color.Black;
                //}

                if (edge.IsMinCut)
                {
                    addedge.Attr.Color = Color.Red;
                    addedge.Attr.AddStyle(Style.Dashed);
                    addedge.SourceNode.Attr.Color = Color.Red;
                }
                //else
                //{
                //    addedge.Attr.Color = Color.Black;
                //}

                addedge.SourceNode.Attr.Shape = Shape.Circle;
                addedge.TargetNode.Attr.Shape = Shape.Circle;
            }
            return msaglGraph;
        }

        public Graph CreateResidualGraph()
        {
            var msaglGraph = this.CreateGraph();

            foreach (var edge in this.libGraph.Edges.Where(e => e.Capacity > 0))
            {
                var addedge = msaglGraph.AddEdge(edge.NodeFrom.Name, string.Format("{0}", edge.Capacity), edge.NodeTo.Name);

                if (edge.IsPathMarked)
                {
                    addedge.Attr.Color = Color.Orange;
                }
                else
                {
                    addedge.Attr.Color = Color.Black;
                }
                addedge.SourceNode.Attr.Shape = Shape.Circle;
                addedge.TargetNode.Attr.Shape = Shape.Circle;
            }

            return msaglGraph;
        }
    }
}
