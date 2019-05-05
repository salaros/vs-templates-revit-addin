#region Namespaces

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

#endregion

namespace RevitAddin
{
    /// <summary>
    /// The main application defined in this add-in
    /// </summary>
    /// <seealso cref="T:Autodesk.Revit.UI.IExternalApplication" />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class App : IExternalApplication
    {
        protected UIControlledApplication uiControlledApplication;

        /// <summary>
        /// Initializes the <see cref="App"/> class.
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
        /// <param name="uiControlledApplication">The UI control application.</param>
        /// <returns></returns>
        /// ReSharper disable once ParameterHidesMember
        public Result OnStartup(UIControlledApplication uiControlledApplication)
        {
            this.uiControlledApplication = uiControlledApplication;

#if REVIT2017
            // A workaround for a bug with UI culture in Revit 2017.1.1
            // More info here: https://forums.autodesk.com/t5/revit-api-forum/why-the-language-key-switches-currentculture-instead-of/m-p/6843557/highlight/true#M20779
            var language = uiControlledApplication.ControlledApplication.Language.ToString();
            Thread.CurrentThread.CurrentUICulture = CultureInfo
                                                        .GetCultures(CultureTypes.SpecificCultures)
                                                        .FirstOrDefault(c => language.Contains(c.EnglishName)) ?? Thread.CurrentThread.CurrentUICulture;
#endif

            InitializeRibbon();

            try
            {
                // Idling and initialization
                uiControlledApplication.Idling += OnIdling;
                uiControlledApplication.ControlledApplication.ApplicationInitialized += OnApplicationInitialized;
                // Open / change
                uiControlledApplication.ControlledApplication.DocumentOpened += OnDocumentOpened;
                uiControlledApplication.ControlledApplication.DocumentChanged += OnDocumentChanged;
                // Save / SaveAs
                uiControlledApplication.ControlledApplication.DocumentSaved += OnDocumentSaved;
                uiControlledApplication.ControlledApplication.DocumentSavedAs += OnDocumentSavedAs;
                // Progress & Failure
                uiControlledApplication.ControlledApplication.ProgressChanged += OnProgressChanged;
                uiControlledApplication.ControlledApplication.FailuresProcessing += OnFailuresProcessing;
                // Closing
                uiControlledApplication.ControlledApplication.DocumentClosing += OnDocumentClosing;
                uiControlledApplication.ControlledApplication.DocumentClosed += OnDocumentClosed;

                // TODO: add you code here
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
        /// <param name="uiCtrlApp">The application.</param>
        /// <returns></returns>
        public Result OnShutdown(UIControlledApplication uiCtrlApp)
        {
            try
            {
                // Idling and initialization
                uiControlledApplication.Idling -= OnIdling;
                uiControlledApplication.ControlledApplication.ApplicationInitialized -= OnApplicationInitialized;
                // Open / change
                uiControlledApplication.ControlledApplication.DocumentOpened -= OnDocumentOpened;
                uiControlledApplication.ControlledApplication.DocumentChanged -= OnDocumentChanged;
                // Save / SaveAs
                uiControlledApplication.ControlledApplication.DocumentSaved -= OnDocumentSaved;
                uiControlledApplication.ControlledApplication.DocumentSavedAs -= OnDocumentSavedAs;
                // Progress & Failure
                uiControlledApplication.ControlledApplication.ProgressChanged -= OnProgressChanged;
                uiControlledApplication.ControlledApplication.FailuresProcessing -= OnFailuresProcessing;
                // Closing
                uiControlledApplication.ControlledApplication.DocumentClosing -= OnDocumentClosing;
                uiControlledApplication.ControlledApplication.DocumentClosed -= OnDocumentClosed;

                // TODO: add you code here
            }
            catch (Exception ex)
            {
                TaskDialog.Show($"Error in {nameof(OnShutdown)} method", ex.ToString());
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private void InitializeRibbon()
        {
            // TODO declare your ribbon items here
            var ribbonItems = new List<RibbonHelper.RibbonButton>
            {
                new RibbonHelper.RibbonButton<RibbonCommand>                        // One can reference commands defined in other assemblies
                {
                    // You could make your ribbon buttons active with no documenent open/active
                    // Try to create your own class with complex rules on when the given button is active and when it's not
                    AvailabilityClassName = typeof(ZeroDocStateAvailability).FullName,
                    Text = StringLocalizer.CallingAssembly["Always active"],        // Text displayed on the command, can be stored in the resources
                    Tooltip = StringLocalizer.CallingAssembly["I'm always active"], // Tooltip and long description
                    IconName = "Resources.trollFace.png",                           // Path to the image, it's relative to the assembly where the command above is defined
                },
                new RibbonHelper.RibbonButton<RibbonCommand>                        // One can reference commands defined in other assemblies
                {
                    Text = StringLocalizer.CallingAssembly["Text 1"],               // Text displayed on the command, can be stored in the resources
                    Tooltip = StringLocalizer.CallingAssembly["My test tooltip"],   // Tooltip and long description
                    IconName = "Resources.testCommand.png",                         // Path to the image, it's relative to the assembly where the command above is defined
                }
            };

            RibbonHelper.AddButtons(
                uiControlledApplication,
                ribbonItems,
                ribbonPanelName: StringLocalizer.CallingAssembly["Test panel"],     // The title of the ribbot panel
                ribbonTabName: StringLocalizer.CallingAssembly["Test ribbon"]       // The title of the ribbon tab
            );
        }

        /// <summary>
        /// Called when [idling].
        /// This event is raised only when the Revit UI is in a state
        /// when there is no active document.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="T:Autodesk.Revit.UI.Events.IdlingEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private void OnIdling(object sender, IdlingEventArgs args)
        {
            // TODO: add you code here
        }

        /// <summary>
        /// Called when [application initialized].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="T:Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private void OnApplicationInitialized(object sender, ApplicationInitializedEventArgs args)
        {
            // TODO: add you code here
        }

        /// <summary>
        /// Called when [document opened].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DocumentOpenedEventArgs" /> instance containing the event data.</param>
        private void OnDocumentOpened(object sender, DocumentOpenedEventArgs args)
        {
            // TODO: this is just an example, remove or change code below
            var doc = args.Document;
            Debug.Assert(null != doc, $"Expected a valid Revit {nameof(Document)} instance");

            // TODO: this is just an example, remove or change code below
            var app = args.Document?.Application;
            var uiapp = new UIApplication(app);
            Debug.Assert(null != uiapp, $"Expected a valid Revit {nameof(UIApplication)} instance");
        }

        /// <summary>
        /// Called when [document changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:Autodesk.Revit.DB.Events.DocumentChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            // TODO: add you code here
        }

        /// <summary>
        /// Called when [document saved].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DocumentSavedEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private void OnDocumentSaved(object sender, DocumentSavedEventArgs args)
        {
            // TODO: add you code here
        }

        /// <summary>
        /// Called when [document saved as].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DocumentSavedAsEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private void OnDocumentSavedAs(object sender, DocumentSavedAsEventArgs args)
        {
            // TODO: add you code here
        }

        /// <summary>
        /// Called when [failures processing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FailuresProcessingEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private void OnFailuresProcessing(object sender, FailuresProcessingEventArgs e)
        {
            // TODO: add you code here
        }

        /// <summary>
        /// Called when [progress changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="progressChangedEventArgs">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private void OnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            // TODO: add you code here
        }

        /// <summary>
        /// Called when [document closing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DocumentClosingEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private void OnDocumentClosing(object sender, DocumentClosingEventArgs args)
        {
            // TODO: add you code here
        }

        /// <summary>
        /// Called when [document closed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DocumentClosedEventArgs" /> instance containing the event data.</param>
        /// ReSharper disable once MemberCanBeMadeStatic.Local
        private void OnDocumentClosed(object sender, DocumentClosedEventArgs args)
        {
            // TODO: add you code here
        }
    }
}
