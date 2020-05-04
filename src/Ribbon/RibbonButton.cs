using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace RevitAddin
{
    /// <summary>
    /// Ribbon button item abstract helper.
    /// </summary>
    /// <seealso cref="RibbonButton" />
    internal abstract class RibbonButton
    {
        private static readonly Regex CommandNonWordChars = new Regex(@"\W", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets or sets the text.
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
        /// Gets or sets the name of the icon.
        /// </summary>
        /// <value>
        /// The name of the icon.
        /// </value>
        public abstract string IconName { get; internal set; }

        /// <summary>
        /// Gets or sets the tooltip.
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
        /// Gets or sets the name of the tooltip.
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
            using var smallIcon = new Bitmap(item.Icon, 16, 16);
            using var midIcon = new Bitmap(item.Icon, 32, 32);
            using var bigIcon = (item.TooltipIcon == null) ? null : new Bitmap(item.TooltipIcon, 192, 192);
            return new PushButtonData(
                $"cmd{CommandNonWordChars.Replace(item.Text, string.Empty)}",
                item.Text,
                item.AssemblyPath,
                item.Command.FullName)
            {
                LongDescription = item.Tooltip,
                ToolTip = item.Tooltip,
                Image = (item.Icon == null) ? null : ConvertFromImage(smallIcon),
                LargeImage = (item.Icon == null) ? null : ConvertFromImage(midIcon),
                ToolTipImage = (item.TooltipIcon == null) ? null : ConvertFromImage(bigIcon),
                AvailabilityClassName = item.AvailabilityClassName,
            };
        }

        public PushButtonData ToPushButtonData()
        {
            return this;
        }

        /// <summary>
        /// Converts from image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns><see cref="BitmapSource"/> source of the given <see cref="Bitmap"/> image.</returns>
        private static BitmapSource ConvertFromImage(Bitmap image)
        {
            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return Imaging.CreateBitmapSourceFromHBitmap(
                image.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
