using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace RevitAddin
{
    /// <summary>
    /// In absence of <see cref="T:Microsoft.Extensions.Localization"/> and <see cref="T:Microsoft.Extensions.DependencyInjection"/>
    /// on .NET Framework &lt; 4.6.1 this class provides an easy access to localized resources
    /// </summary>
    /// <seealso cref="IDisposable" />
    /// <inheritdoc />
    public class StringLocalizer : IDisposable
    {
        public static StringLocalizer ExecutingAssembly { get; }

        public static StringLocalizer CallingAssembly { get; }

        static StringLocalizer()
        {
            var execAssembly = Assembly.GetExecutingAssembly();
            ExecutingAssembly = new StringLocalizer(execAssembly);

            var callAssembly = Assembly.GetExecutingAssembly();
            CallingAssembly = Equals(execAssembly, callAssembly)
                ? ExecutingAssembly
                : new StringLocalizer(callAssembly);
        }

        protected ResourceManager resourceManager;

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
            ReleaseUnmanagedResources();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <inheritdoc />
        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources.
        /// </summary>
        private void ReleaseUnmanagedResources()
        {
            resourceManager?.ReleaseAllResources();
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified string.
        /// </summary>
        /// <value>
        /// The <see cref="System.String"/>.
        /// </value>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public string this[string str]
        {
            get
            {
                try
                {
                    return resourceManager.GetString(str);
                }
                catch
                {
                    return str;
                }
            }
        }
    }
}
