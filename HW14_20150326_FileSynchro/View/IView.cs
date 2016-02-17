using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW14_20150326_FileSynchro.View
{
    /// <summary>
    /// Interface to be inherited by View - Form
    /// </summary>
    interface IView
    {
        /// <summary>
        /// Get-set Source folder
        /// </summary>
        string SourceFolder { get; set; }
        /// <summary>
        /// Get-set Destination folder
        /// </summary>
        string DestinationFolder { get; set; }
        /// <summary>
        /// Get a synchronization direction
        /// </summary>
        int Direction { get; }

        /// <summary>
        /// Synchronize event
        /// </summary>
        event EventHandler<EventArgs> Synchronize;

        /// <summary>
        /// Show message functionality
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="MessageName"></param>
        void ShowMessage(string Text, string MessageName);

        void SetProgress(string TooltipText);
    }
}
