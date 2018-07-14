using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitAddin
{
    [Transaction(TransactionMode.Manual)] 
    [Regeneration(RegenerationOption.Manual)] 
    public class RibbonCommand : IExternalCommand
    {
        /// <summary>
        /// The main Execute method (inherited from IExternalCommand) must be public
        /// </summary>
        /// <param name="commandData">The revit.</param>
        /// <param name="message">The message.</param>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements
        ){
            TaskDialog.Show("It's alive!", StringLocalizer.CallingAssembly["Localization: test string with spaces"]);
            return Result.Succeeded;
        }
    }
}
