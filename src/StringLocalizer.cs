using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace RevitAddin
{
    /// <summary>
    /// In absence of <see cref="Microsoft.Extensions.Localization"/> and <see cref="Microsoft.Extensions.DependencyInjection"/>
    /// on .NET Framework &lt; 4.6.1 this class provides an easy access to localized resources.
    /// </summary>
    /// <seealso cref="IDisposable" />
    /// <inheritdoc />
    public class StringLocalizer : IDisposable
    {
        private readonly ResourceManager resourceManager;
        private bool disposedValue;

        /// <summary>Initializes static members of the <see cref="StringLocalizer"/> class.</summary>
        static StringLocalizer()
        {
            var execAssembly = Assembly.GetExecutingAssembly();
            ExecutingAssembly = new StringLocalizer(execAssembly);

            var callAssembly = Assembly.GetExecutingAssembly();
            CallingAssembly = Equals(execAssembly, callAssembly)
                ? ExecutingAssembly
                : new StringLocalizer(callAssembly);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringLocalizer" /> class.
        /// </summary>
        /// <param name="assembly">The assembly containing localized string resources.</param>
        /// <param name="resourceLocation">The resource location, defaults to 'Properties.Resources'.</param>
        public StringLocalizer(Assembly assembly = null, string resourceLocation = "Properties.Resources")
        {
            var callingAssembly = assembly ??
                                  new StackTrace().GetFrame(1).GetMethod()?.ReflectedType?.Assembly ??
                                  Assembly.GetEntryAssembly();
            var rootNameSpace = callingAssembly.ManifestModule.GetTypes().Min(t => t.Namespace);
            resourceManager = new ResourceManager($"{rootNameSpace}.{resourceLocation}", callingAssembly);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="StringLocalizer" /> class.
        /// </summary>
        ~StringLocalizer()
        {
            Dispose(disposing: false);
        }

        public static StringLocalizer CallingAssembly { get; }

        public static StringLocalizer ExecutingAssembly { get; }

        /// <summary>
        /// Gets the <see cref="string"/> with the specified string.
        /// </summary>
        /// <value>
        /// The <see cref="string"/>.
        /// </value>
        /// <param name="str">The string.</param>
        [SuppressMessage("Design", "CC0003:Your catch should include an Exception", Justification = "Have to catch'em all.")]
        public string this[string str]
        {
            get
            {
                try
                {
                    return resourceManager.GetString(str) ?? str;
                }
                catch
                {
                    return str;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing">
        ///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    resourceManager?.ReleaseAllResources();
                }

                disposedValue = true;
            }
        }
    }
}
