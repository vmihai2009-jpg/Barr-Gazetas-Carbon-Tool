using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CarbonEmissionTool.Models;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Views;
using RevitAddIn.Interfaces;

namespace RevitAddIn.ButtonData
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CarbonToolButtonData : IButtonData
    {
        public string ToolTip { get; }

        public string VisibleButtonName { get; }

        public string InternalButtonName { get; }

        public string IconName { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CarbonToolButtonData()
        {
            this.VisibleButtonName = "Barr Gazetas";
            this.InternalButtonName = "cmdBarrGazetasCarbonTool";
            this.ToolTip = "Launch the Barr Gazetas Carbon Tool.";

            this.IconName = "BGCT_icon_Revit.png";
        }

        /// <summary>
        /// The process which executes when the button is clicked by the user.
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document; // Update the static field to hold current document

            var dataCapture = new DataCapture();

            ApplicationServices.OnStartup(doc, dataCapture);

            try
            {
                if (!ApplicationServices.CarbonDataCache.IsEmpty)
                {
                    var mainWindow = new MainWindow();
                    mainWindow.ShowDialog();
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.StackTrace);
            }
            finally
            {
                ApplicationServices.OnShutdown();
            }

            return Result.Succeeded;
        }
    }
}