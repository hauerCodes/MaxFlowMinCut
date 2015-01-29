using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaxFlowMinCut.Wpf.UserControl
{
    using System.ComponentModel;
    using System.Diagnostics;
        using System.Windows;
        using System.Windows.Controls;
        using System.Windows.Forms.Integration;
        using MaxFlowMinCut.Lib;
        using MaxFlowMinCut.Wpf.Visualizer;

        using Microsoft.Msagl.Drawing;
        using Microsoft.Msagl.GraphViewerGdi;

        public partial class DependencyGraphWrapper : UserControl
        {
            public DependencyGraphWrapper()
            {
                this.InitializeComponent();

                DependencyPropertyDescriptor
                    .FromProperty(DependencyGraphWrapper.FlowGraphProperty, typeof(DependencyGraphWrapper))
                    .AddValueChanged(this, (s, e) =>
                    {
                        if (Viewer != null && Viewer.Child != null && Viewer.Child is GViewer)
                        {
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                (Viewer.Child as GViewer).Graph = new VisualGraph(FlowGraph).CreateFlowGraph();
                            });
                        }
                    });

                DependencyPropertyDescriptor
                   .FromProperty(DependencyGraphWrapper.ResidualGraphProperty, typeof(DependencyGraphWrapper))
                   .AddValueChanged(this, (s, e) =>
                   {
                       if (Viewer != null && Viewer.Child != null && Viewer.Child is GViewer)
                       {
                           App.Current.Dispatcher.Invoke(() =>
                            {
                                (Viewer.Child as GViewer).Graph = new VisualGraph(ResidualGraph).CreateFlowGraph();
                            });
                       }
                   });
            }

            /// <summary>
            /// Gets or sets the graph.
            /// </summary>
            /// <value>
            /// The graph.
            /// </value>
            public Lib.Graph FlowGraph
            {
                get { return (Lib.Graph)GetValue(FlowGraphProperty); }
                set
                {
                    SetValue(FlowGraphProperty, value);
                }
            }

            public static readonly DependencyProperty FlowGraphProperty =
               DependencyProperty.Register("FlowGraph", typeof(Lib.Graph), typeof(DependencyGraphWrapper), new PropertyMetadata());

            public Lib.Graph ResidualGraph
            {
                get { return (Lib.Graph)GetValue(ResidualGraphProperty); }
                set
                {
                    SetValue(ResidualGraphProperty, value);

                }
            }

            // Using a DependencyProperty as the backing store for Viewer.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty ResidualGraphProperty =
                DependencyProperty.Register("ResidualGraph", typeof(Lib.Graph), typeof(DependencyGraphWrapper), new PropertyMetadata());
        }
    }
