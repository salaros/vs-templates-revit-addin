using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;

namespace RevitAddin
{
    /// <summary>
    /// A set of helpers for Revit Ribbon.
    /// </summary>
    public static partial class RibbonHelper
    {
        /// <summary>
        /// Adds buttons to the Revit ribbon.
        /// </summary>
        /// <param name="uiApp">The UI application.</param>
        /// <param name="items">The items.</param>
        /// <param name="ribbonPanelName">Name of the ribbon panel.</param>
        /// <param name="ribbonTabName">Name of the ribbon tab.</param>
        internal static void AddButtons(
            UIControlledApplication uiApp,
            IEnumerable<RibbonButton> items,
            string ribbonPanelName = "",
            string ribbonTabName = "")
        {
            if (uiApp is null)
                throw new ArgumentNullException(nameof(uiApp));

            ribbonPanelName = string.IsNullOrWhiteSpace(ribbonPanelName)
                ? nameof(RevitAddin)
                : ribbonPanelName;

            RibbonPanel ribbonPanel = null;
            try
            {
                uiApp.CreateRibbonTab(ribbonTabName);

                // Add a new ribbon panel
                ribbonPanel = string.IsNullOrWhiteSpace(ribbonTabName)
                    ? uiApp.CreateRibbonPanel(ribbonPanelName)
                    : uiApp.CreateRibbonPanel(ribbonTabName, ribbonPanelName);
            }
            finally
            {
                foreach (var ribbonButton in items)
                {
                    ribbonPanel?.AddItem(ribbonButton);
                }
            }
        }
    }
}
