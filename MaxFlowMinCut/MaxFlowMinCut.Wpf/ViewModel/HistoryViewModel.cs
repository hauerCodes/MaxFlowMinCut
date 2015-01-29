// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryViewModel.cs" company="FH Wr. Neustadt">
//   Christoph Hauer / Markus Zytek. All rights reserved.
// </copyright>
// <summary>
//   The history view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MaxFlowMinCut.Wpf.ViewModel
{
    using System.Windows;

    using MaxFlowMinCut.Lib.History;

    /// <summary>
    /// The history view model.
    /// </summary>
    public class HistoryViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryViewModel"/> class.
        /// </summary>
        /// <param name="history">
        /// The history.
        /// </param>
        public HistoryViewModel(GraphHistory history)
        {
            Application.Current.Dispatcher.Invoke(() => { this.History = history; });
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