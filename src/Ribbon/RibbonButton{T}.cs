using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;

namespace RevitAddin
{
    /// <summary>
    /// Ribbon button item helper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="RibbonButton" />
    /// <inheritdoc />
    internal class RibbonButton<T> : RibbonButton
        where T : class, IExternalCommand
    {
        private readonly Assembly commandAssemly;
        private readonly string rootNameSpace;

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
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1515:Single-line comment should be preceded by blank line", Justification = "Needed by Resharper")]
        public override Bitmap Icon => string.IsNullOrWhiteSpace(IconName)
            ? null
            // ReSharper disable once AssignNullToNotNullAttribute
            : new Bitmap(commandAssemly.GetManifestResourceStream($"{rootNameSpace}.{IconName}"));

        /// <inheritdoc />
        public override string IconName { get; internal set; }

        /// <inheritdoc />
        public override string Tooltip { get; internal set; } = string.Empty;

        /// <inheritdoc />
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1515:Single-line comment should be preceded by blank line", Justification = "Needed by Resharper")]
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
}
