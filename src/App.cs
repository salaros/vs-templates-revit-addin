#region Namespaces

using System;
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
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class App : IExternalApplication
    {

        protected UIControlledApplication uiControlledApplication;

        /// <summary>
        /// Called when [startup].
        /// </summary>
        /// <param name="uiCtrlApp">The UI control application.</param>
        /// <returns></returns>
        /// ReSharper disable once ParameterHidesMember
        public Result OnStartup(UIControlledApplication uiCtrlApp)
        {
            uiControlledApplication = uiCtrlApp;

#if REVIT2017
            // A workaround for a bug with UI culture in Revit 2017.1.1
            // More info here: https://forums.autodesk.com/t5/revit-api-forum/why-the-language-key-switches-currentculture-instead-of/m-p/6843557/highlight/true#M20779
            var language = uiCtrlApp.ControlledApplication.Language.ToString();
            Thread.CurrentThread.CurrentUICulture = CultureInfo
                                                        .GetCultures(CultureTypes.SpecificCultures)
                                                        .FirstOrDefault(c => language.Contains(c.EnglishName)) ?? Thread.CurrentThread.CurrentUICulture;
#endif

            RibbonHelper.AddButtons(uiCtrlApp);

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
