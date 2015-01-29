using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlowMinCut.Wpf.ViewModel
{
    using MaxFlowMinCut.Lib.History;

    public class HistoryViewModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryViewModel"/> class.
        /// </summary>
        /// <param name="history">The history.</param>
        public HistoryViewModel(GraphHistory history)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                this.History = history;
            });
        }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        /// <value>
        /// The history.
        /// </value>
        public GraphHistory History { get; set; }
    }
}
