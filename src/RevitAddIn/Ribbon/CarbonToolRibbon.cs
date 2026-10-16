using Autodesk.Revit.UI;
using CarbonEmissionTool.Services;
using RevitAddIn.ButtonData;

namespace RevitAddIn.Ribbon
{
    /// <remarks>
    /// This application's main class to create the ribbon in Revit. 
    /// </remarks>
    public class CarbonToolRibbon : IExternalApplication
    {
        // Both OnStartup and OnShutdown must be implemented as public method
        public Result OnStartup(UIControlledApplication application)
        {
            ApplicationServices.RevitVersion = int.Parse(application.ControlledApplication.VersionNumber);

            // Shared Barr Gazetas tab: other BG add-ins (e.g. MeshyRFA) may have created it already.
            RibbonPanel ribbonPanel = RibbonUtils.GetOrCreateBgPanel(application, "Building Analysis");

            RibbonUtils.AddButton(ribbonPanel, new CarbonToolButtonData());

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case

            return Result.Succeeded;
        }
    }
}
