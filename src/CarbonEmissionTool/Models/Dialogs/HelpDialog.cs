using Autodesk.Revit.UI;

namespace CarbonEmissionTool.Models
{
    public class HelpDialog
    {
        /// <summary>
        /// Displays if the carbon tool successfully runs, providing feedback to the user that the process has completed.
        /// </summary>
        public static void CarbonToolSuccessfullyRun()
        {
            TaskDialog td = new TaskDialog("Process Completed")
            {
                MainInstruction = @"The Barr Gazetas Carbon Tool has successfully run. Please locate your sheet to view the results!",
                AllowCancellation = false,
                CommonButtons = TaskDialogCommonButtons.Ok
            };
            td.Show();
        }
    }
}
