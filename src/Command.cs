#region Namespaces

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion

namespace RevitAddin
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var app = uiapp.Application;
            var doc = uidoc.Document;

            // Access current selection
            var selection = uidoc.Selection;

            // Retrieve elements from database
            var col
                = new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .OfCategory(BuiltInCategory.INVALID)
                    .OfClass(typeof(Wall));

            // Filtered element collector is iterable
            foreach (var el in col)
            {
                Debug.Print(el.Name);
            }

            // Modify document within a transaction
            using (var transaction = new Transaction(doc))
            {
                transaction.Start("Transaction Name");
                using (var subTransaction = new SubTransaction(doc))
                {
                    subTransaction.Start();
                    // Do something here
                    subTransaction.Commit();
                }
                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}