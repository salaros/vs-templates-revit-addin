using System.Collections.Generic;

namespace RevitAddin
{
    public static partial class RibbonHelper
    {
        /// <summary>
        /// Initializes the <see cref="RibbonHelper"/> class.
        /// </summary>
        static RibbonHelper()
        {
            // TODO declare your ribbon items here
            RibbonTitle = "Test ribbon";
            RibbonItems = new List<RibbonButton>
            {
                new RibbonButton<RibbonCommand> // One can reference commands defined in other assemblies
                {
                    Text = "Text 1",            // Text displayed on the command, can be stored in the resources
                    Tooltip = "Test tooltip",   // Tooltip and long description
                    IconName = "Resources.testCommand.png" // Path to the image, it's relative to the assembly where the command above is defined
                }
            };
        }
    }
}
