using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitAddin
{
    public class ZeroDocStateAvailability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories) => true;
    }
}
