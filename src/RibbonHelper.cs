using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace RevitAddin
{
    /// <summary>
    /// A set of helpers for Revit Ribbon
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
        [SuppressMessage("Design", "CC0003:Your catch should include an Exception", Justification = "Catch all exception types.")]
        public static void AddButtons(
            UIControlledApplication uiApp,
            IEnumerable<RibbonButton> items,
            string ribbonPanelName = "",
            string ribbonTabName = "")
        {
            ribbonPanelName = string.IsNullOrWhiteSpace(ribbonPanelName)
                ? nameof(RevitAddin)
                : ribbonPanelName;

            RibbonPanel ribbonPanel = null;
            try
            {
                uiApp.CreateRibbonTab(ribbonTabName);
            }
            catch
            {
                TaskDialog.Show("Error", $"Failed to create the following '{ribbonTabName}' ribbon.");
            }

            try
            {
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

        /// <summary>
        /// Converts from image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static BitmapSource ConvertFromImage(Bitmap image)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                image.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        /// Ribbon button item helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <seealso cref="RibbonButton" />
        /// <inheritdoc />
        /// <seealso cref="Equipple.Revit.FamilyBrowser.Ribbon.RibbonHelper.RibbonButton" />
        public class RibbonButton<T> : RibbonButton
            where T : class, IExternalCommand
        {
            protected Assembly commandAssemly;
            protected string rootNameSpace;

            /// <summary>
            /// Initializes a new instance of the <see cref="RibbonButton{T}"/> class.
            /// </summary>
            public RibbonButton()
            {
                commandAssemly = typeof(T).Assembly;
                rootNameSpace = typeof(T).Assembly.ManifestModule.GetTypes().Min(t => t.Namespace);
            }

            /// <inheritdoc />
            public override string Text { get; internal set; }

            /// <inheritdoc />
            public override Bitmap Icon => string.IsNullOrWhiteSpace(IconName)
                ? null

                // ReSharper disable once AssignNullToNotNullAttribute
                : new Bitmap(commandAssemly.GetManifestResourceStream($"{rootNameSpace}.{IconName}"));

            /// <inheritdoc />
            public override string IconName { get; internal set; }

            /// <inheritdoc />
            public override string Tooltip { get; internal set; } = string.Empty;

            /// <inheritdoc />
            public override Bitmap TooltipIcon => string.IsNullOrWhiteSpace(TooltipName)
                ? null

                // ReSharper disable once AssignNullToNotNullAttribute
                : new Bitmap(commandAssemly.GetManifestResourceStream($"{rootNameSpace}.{TooltipName}"));

            /// <inheritdoc />
            public override string TooltipName { get; internal set; }

            /// <inheritdoc />
            public override Type Command { get; } = typeof(T);

            /// <inheritdoc />
            public override string AssemblyPath { get; } = typeof(T).Assembly.Location;
        }

        /// <summary>
        /// Ribbon button item abstract helper
        /// </summary>
        /// <seealso cref="RibbonButton" />
        public abstract class RibbonButton
        {
            private static readonly Regex CommandNonWordChars = new Regex(@"\W", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            /// <summary>
            /// Gets the text.
            /// </summary>
            /// <value>
            /// The text.
            /// </value>
            public abstract string Text { get; internal set; }

            /// <summary>
            /// Gets the icon.
            /// </summary>
            /// <value>
            /// The icon.
            /// </value>
            public abstract Bitmap Icon { get; }

            /// <summary>
            /// Gets the name of the icon.
            /// </summary>
            /// <value>
            /// The name of the icon.
            /// </value>
            public abstract string IconName { get; internal set; }

            /// <summary>
            /// Gets the tooltip.
            /// </summary>
            /// <value>
            /// The tooltip.
            /// </value>
            public abstract string Tooltip { get; internal set; }

            /// <summary>
            /// Gets the tooltip icon.
            /// </summary>
            /// <value>
            /// The tooltip icon.
            /// </value>
            public abstract Bitmap TooltipIcon { get; }

            /// <summary>
            /// Gets the name of the tooltip.
            /// </summary>
            /// <value>
            /// The name of the tooltip.
            /// </value>
            public abstract string TooltipName { get; internal set; }

            /// <summary>
            /// Gets the type/class behind command.
            /// </summary>
            /// <value>
            /// The command.
            /// </value>
            public abstract Type Command { get; }

            /// <summary>
            /// Gets the path of the assembly where the command is defined.
            /// </summary>
            /// <value>
            /// The assembly path.
            /// </value>
            public abstract string AssemblyPath { get; }

            /// <summary>
            /// Gets or sets the name of the availability class.
            /// </summary>
            /// <value>
            /// The name of the availability class.
            /// </value>
            public string AvailabilityClassName { get; set; }

            /// <summary>
            /// Performs an implicit conversion from <see cref="RibbonButton"/> to <see cref="PushButtonData"/>.
            /// </summary>
            /// <param name="item">The item.</param>
            /// <returns>
            /// The result of the conversion.
            /// </returns>
            public static implicit operator PushButtonData(RibbonButton item)
            {
                return new PushButtonData(
                    $"cmd{CommandNonWordChars.Replace(item.Text, string.Empty)}",
                    item.Text,
                    item.AssemblyPath,
                    item.Command.FullName)
                {
                    LongDescription = item.Tooltip,
                    ToolTip = item.Tooltip,
                    Image = (item.Icon is null) ? null : ConvertFromImage(new Bitmap(item.Icon, 16, 16)),
                    LargeImage = (item.Icon is null) ? null : ConvertFromImage(new Bitmap(item.Icon, 32, 32)),
                    ToolTipImage = (item.TooltipIcon is null) ? null : ConvertFromImage(new Bitmap(item.TooltipIcon, 192, 192)),
                    AvailabilityClassName = item.AvailabilityClassName,
                };
            }
        }
    }
}
