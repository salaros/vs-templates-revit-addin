#region Namespaces

using Autodesk.Revit.UI;

#endregion

namespace RevitAddin
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
