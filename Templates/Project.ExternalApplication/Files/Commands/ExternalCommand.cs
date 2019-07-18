#region Namespaces

using System.Diagnostics;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

#endregion

namespace $safeprojectname$.Commands
{
    /// <summary>
    /// A simple example of an external command, usually it's used for batch processing
    /// files loaded when Revit is idling
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExternalCommand : IExternalCommand
    {
        /// <summary>
        /// Executes the specified Revit command <see cref="ExternalCommand"/>.
        /// The main Execute method (inherited from IExternalCommand) must be public.
        /// </summary>
        /// <param name="commandData">The command data / context.</param>
        /// <param name="message">The message.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>The result of command execution.</returns>
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements
        )
        {
            var uiapp = commandData?.Application;
            var uidoc = uiapp?.ActiveUIDocument;
            var app = uiapp?.Application;
            var doc = uidoc?.Document;

            if (null == app || null == doc)
            {
                // TODO it's just an example, an external command can open a document if needed
                MessageBox.Show("This command can be called only when a document is opened and ready!");
                return Result.Cancelled;
            }

            // Example 1: pick one object from Revit.
            var selection = uidoc.Selection;
            var hasPickOne = selection?.PickObject(ObjectType.Element);
            if (hasPickOne != null)
            {
                Debug.Print(doc.GetElement(hasPickOne.ElementId).Name);
            }

            // Example 2: retrieve elements from database
            var walls
                = new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .OfCategory(BuiltInCategory.INVALID)
                    .OfClass(typeof(Wall));
            if (null != walls)
            {
                foreach (var wall in walls)
                {
                    Debug.Print(wall.Name);
                }
            }

            // Example 3: Modify document within a transaction
            using (var transaction = new Transaction(doc, "Transaction name goes here, change it"))
            {
                if (TransactionStatus.Started != transaction.Start())
                    return Result.Failed; // TODO do something if transaction didn't start

                using (var subTransaction = new SubTransaction(doc))
                {
                    subTransaction.Start();
                    // TODO: add you code here
                    subTransaction.Commit();
                }
                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
