using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

#if REVIT2017
using System.Globalization;
using System.Linq;
using System.Threading;
#endif

namespace RevitAddin
{
    /// <summary>
    /// The main application defined in this add-in.
    /// </summary>
    /// <seealso cref="IExternalApplication" />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class App : IExternalApplication
    {
        /// <summary>
        /// Initializes static members of the <see cref="App"/> class.
        /// </summary>
        static App()
        {
#if WINFORMS
            global::System.Windows.Forms.Application.EnableVisualStyles();
            global::System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
#endif
        }

        /// <summary>
        /// Called when [startup].
        /// </summary>
        /// <param name="application">The UI control application.</param>
        /// <returns>Status of application execution.</returns>
        /// ReSharper disable once ParameterHidesMember
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Gotta catch'em all")]
        public Result OnStartup(UIControlledApplication application)
        {
            if (application is null)
                throw new ArgumentNullException(nameof(application));

#if REVIT2017
            // A workaround for a bug with UI culture in Revit 2017.1.1
            // More info here: https://forums.autodesk.com/t5/revit-api-forum/why-the-language-key-switches-currentculture-instead-of/m-p/6843557/highlight/true#M20779
            var language = application.ControlledApplication.Language.ToString();
            Thread.CurrentThread.CurrentUICulture = CultureInfo
                                                        .GetCultures(CultureTypes.SpecificCultures)
                                                        .FirstOrDefault(c => language.Contains(c.EnglishName)) ?? Thread.CurrentThread.CurrentUICulture;
#endif

            InitializeRibbon(application);

            try
            {
                // Idling and initialization
                application.Idling += OnIdling;
                application.ControlledApplication.ApplicationInitialized += OnApplicationInitialized;

                // Open / change
                application.ControlledApplication.DocumentOpened += OnDocumentOpened;
                application.ControlledApplication.DocumentChanged += OnDocumentChanged;

                // Save / SaveAs
                application.ControlledApplication.DocumentSaved += OnDocumentSaved;
                application.ControlledApplication.DocumentSavedAs += OnDocumentSavedAs;

                // Progress & Failure
                application.ControlledApplication.ProgressChanged += OnProgressChanged;
                application.ControlledApplication.FailuresProcessing += OnFailuresProcessing;

                // Closing
                application.ControlledApplication.DocumentClosing += OnDocumentClosing;
                application.ControlledApplication.DocumentClosed += OnDocumentClosed;

                // Views
                application.ViewActivated += OnViewActivated;

                // add you code here
            }
            catch (Exception ex)
            {
                TaskDialog.Show($"Error in {nameof(OnStartup)} method", ex.ToString());
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        /// <summary>
        /// Called when [shutdown].
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>Status of application execution.</returns>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Gotta catch'em all")]
        public Result OnShutdown(UIControlledApplication application)
        {
            if (application is null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            try
            {
                // Idling and initialization
                application.Idling -= OnIdling;
                application.ControlledApplication.ApplicationInitialized -= OnApplicationInitialized;

                // Open / change
                application.ControlledApplication.DocumentOpened -= OnDocumentOpened;
                application.ControlledApplication.DocumentChanged -= OnDocumentChanged;

                // Save / SaveAs
                application.ControlledApplication.DocumentSaved -= OnDocumentSaved;
                application.ControlledApplication.DocumentSavedAs -= OnDocumentSavedAs;

                // Progress & Failure
                application.ControlledApplication.ProgressChanged -= OnProgressChanged;
                application.ControlledApplication.FailuresProcessing -= OnFailuresProcessing;

                // Closing
                application.ControlledApplication.DocumentClosing -= OnDocumentClosing;
                application.ControlledApplication.DocumentClosed -= OnDocumentClosed;

                // Views
                application.ViewActivated -= OnViewActivated;

                // add you code here
            }
            catch (Exception ex)
            {
                TaskDialog.Show($"Error in {nameof(OnShutdown)} method", ex.ToString());
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private static void InitializeRibbon(UIControlledApplication application)
        {
            if (application is null)
                throw new ArgumentNullException(nameof(application));

            // TODO declare your ribbon items here
            var ribbonItems = new List<RibbonButton>
            {
                new RibbonButton<RibbonCommand> // One can reference commands defined in other assemblies
                {
                    // You could make your ribbon buttons active with no documenent open/active
                    // Try to create your own class with complex rules on when the given button is active and when it's not
                    AvailabilityClassName = typeof(ZeroDocStateAvailability).FullName,
                    Text = StringLocalizer.CallingAssembly["Always active"],        // Text displayed on the command, can be stored in the resources
                    Tooltip = StringLocalizer.CallingAssembly["I'm always active"], // Tooltip and long description
                    IconName = "Resources.trollFace.png",                           // Path to the image, it's relative to the assembly where the command above is defined
                },
                new RibbonButton<RibbonCommand> // One can reference commands defined in other assemblies
                {
                    Text = StringLocalizer.CallingAssembly["Text 1"],               // Text displayed on the command, can be stored in the resources
                    Tooltip = StringLocalizer.CallingAssembly["My test tooltip"],   // Tooltip and long description
                    IconName = "Resources.testCommand.png",                         // Path to the image, it's relative to the assembly where the command above is defined
                },
            };

            RibbonHelper.AddButtons(
                application,
                ribbonItems,
                ribbonPanelName: StringLocalizer.CallingAssembly["Test panel"],     // The title of the ribbot panel
                ribbonTabName: StringLocalizer.CallingAssembly["Test ribbon"]); // The title of the ribbon tab
        }

        /// <summary>
        /// Called when [idling].
        /// This event is raised only when the Revit UI is in a state
        /// when there is no active document.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Autodesk.Revit.UI.Events.IdlingEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private static void OnIdling(object sender, IdlingEventArgs args)
        {
            // add you code here
        }

        /// <summary>
        /// Called when [application initialized].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private static void OnApplicationInitialized(object sender, ApplicationInitializedEventArgs args)
        {
            // add you code here
        }

        /// <summary>
        /// Called when [document opened].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Autodesk.Revit.DB.Events.DocumentChangedEventArgs" /> instance containing the event data.</param>
        private static void OnDocumentOpened(object sender, DocumentOpenedEventArgs args)
        {
            // TODO: this is just an example, remove or change code below
            var doc = args.Document;
            Debug.Assert(doc != null, $"Expected a valid Revit {nameof(Document)} instance");

            // TODO: this is just an example, remove or change code below
            var app = args.Document?.Application;
            using var uiapp = new UIApplication(app);
            Debug.Assert(uiapp != null, $"Expected a valid Revit {nameof(UIApplication)} instance");
        }

        /// <summary>
        /// Called when [document changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Autodesk.Revit.DB.Events.DocumentChangedEventArgs"/> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private static void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            // add you code here
        }

        /// <summary>
        /// Called when [document saved].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Autodesk.Revit.DB.Events.DocumentSavedEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private static void OnDocumentSaved(object sender, DocumentSavedEventArgs args)
        {
            // add you code here
        }

        /// <summary>
        /// Called when [document saved as].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private static void OnDocumentSavedAs(object sender, DocumentSavedAsEventArgs args)
        {
            // add you code here
        }

        /// <summary>
        /// Called when [failures processing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Autodesk.Revit.DB.Events.FailuresProcessingEventArgs"/> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private static void OnFailuresProcessing(object sender, FailuresProcessingEventArgs e)
        {
            // add you code here
        }

        /// <summary>
        /// Called when [progress changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="progressChangedEventArgs">The <see cref="Autodesk.Revit.DB.Events.ProgressChangedEventArgs"/> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private static void OnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            // add you code here
        }

        /// <summary>
        /// Called when [document closing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Autodesk.Revit.DB.Events.DocumentClosingEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private static void OnDocumentClosing(object sender, DocumentClosingEventArgs args)
        {
            // add you code here
        }

        /// <summary>
        /// Called when [document closed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Autodesk.Revit.DB.Events.DocumentClosedEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private static void OnDocumentClosed(object sender, DocumentClosedEventArgs args)
        {
            // add you code here
        }

        /// <summary>
        /// Called when [view activated].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Autodesk.Revit.UI.Events.ViewActivatedEventArgs"/> instance containing the event data.</param>
        private static void OnViewActivated(object sender, ViewActivatedEventArgs e)
        {
            // add you code here
        }
    }
}
