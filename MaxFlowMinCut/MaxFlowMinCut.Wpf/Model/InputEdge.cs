using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlowMinCut.Wpf.Model
{
    public class InputEdge
    {
        public InputEdge(string nodeFrom, string nodeTo, int capacity)
        {
            this.NodeFrom = nodeFrom;
            this.NodeTo = nodeTo;
            this.Capacity = capacity;
        }

        public string NodeFrom { get; set; }

        public string NodeTo { get; set; }

        public int Capacity { get; set; }
    }
}
