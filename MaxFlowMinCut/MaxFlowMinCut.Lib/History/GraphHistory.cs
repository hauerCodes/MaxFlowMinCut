using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlowMinCut.Lib.History
{
    using System.Collections;

    public class GraphHistory : IEnumerable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphHistory"/> class.
        /// </summary>
        public GraphHistory()
        {
            this.Steps = new List<GraphHistoryStep>();
        }

        /// <summary>
        /// Gets the <see cref="Graph"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Graph"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public GraphHistoryStep this[int index]
        {
            get
            {
                return Steps[index];
            }
        }

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        public List<GraphHistoryStep> Steps { get; private set; }

        /// <summary>
        /// Gets the maximum step.
        /// </summary>
        /// <value>
        /// The maximum step.
        /// </value>
        public int MaxStep
        {
            get
            {
                return Steps.Count;
            }
        }

        /// <summary>
        /// Adds the graph step.
        /// </summary>
        /// <param name="flowGraph">The flow graph.</param>
        public void AddGraphStep(Graph flowGraph)
        {
            this.AddGraphStep(flowGraph, null);
        }

        /// <summary>
        /// Adds the graph step.
        /// </summary>
        /// <param name="flowGraph">The flow graph.</param>
        /// <param name="residualGraph">The residual graph.</param>
        public void AddGraphStep(Graph flowGraph, Graph residualGraph)
        {
            if (residualGraph != null)
            {
                this.Steps.Add(new GraphHistoryStep((Graph)flowGraph.Clone(), (Graph)residualGraph.Clone()));
            }
            else
            {
                this.Steps.Add(new GraphHistoryStep((Graph)flowGraph.Clone()));
            }
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.IEnumerator" />-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return Steps.GetEnumerator();
        }
    }
}
