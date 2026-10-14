using Autodesk.Revit.DB;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CarbonEmissionTool.Models
{
    class ResourceImporter
    {
        /// <summary>
        /// Imports the EC Schedule, library and title block into the active document from the sample template.
        /// </summary>
        public static void GetResources()
        {
            var doc = ApplicationServices.Document;

            //Returns the current version number of Revit as a string
            string revitVersionNumber = doc.Application.VersionNumber;

            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string filePath = new FileInfo(assemblyPath).Directory.FullName;

            var templateDocument = ApplicationServices.Application.OpenDocumentFile(GetTemplatePath(filePath, revitVersionNumber));

            var carbonSchedule = RevitScheduleFilter.GetScheduleByName(templateDocument, ApplicationSettings.EmbodiedCarbonScheduleName);
            var librarySchedule = RevitScheduleFilter.GetScheduleByName(templateDocument, ApplicationSettings.EmbodiedCarbonLibraryName);

            var titleBlock = TitleBlockFilter.GetAll(templateDocument).Find(t => t.Name == ApplicationSettings.TitleBlockName);

            var elementsToCopy = new List<ElementId> { carbonSchedule.Id, librarySchedule.Id?? ElementId.InvalidElementId, titleBlock.Id ?? ElementId.InvalidElementId };

            using (var transaction = new Transaction(doc, "Import carbon schedules and title block"))
            {
                transaction.Start();

                ElementTransformUtils.CopyElements(templateDocument, elementsToCopy, doc, Transform.Identity, new CopyPasteOptions());

                doc.Regenerate();

                transaction.Commit();
            }

            templateDocument.Close(false);
        }

        /// <summary>
        /// Finds the sample template for the running Revit version. If there is no template for that exact version,
        /// the newest template for an earlier version is used; Revit upgrades it in memory when it is opened, which
        /// is why a single template (BGCT_R2024.rvt) can serve Revit 2024 onwards.
        /// </summary>
        private static string GetTemplatePath(string assemblyDirectory, string revitVersionNumber)
        {
            var templateDirectory = Path.Combine(assemblyDirectory, "Revit Templates");
            int.TryParse(revitVersionNumber, out var revitVersion);

            var templatePath = Directory.GetFiles(templateDirectory, "BGCT_R*.rvt")
                .Select(path => new { Path = path, Version = GetTemplateVersion(path) })
                .Where(template => template.Version > 0 && template.Version <= revitVersion)
                .OrderByDescending(template => template.Version)
                .Select(template => template.Path)
                .FirstOrDefault();

            if (templatePath == null)
            {
                throw new FileNotFoundException($"No Barr Gazetas Carbon Tool Revit template found in '{templateDirectory}' for Revit {revitVersionNumber}.");
            }

            return templatePath;
        }

        /// <summary>
        /// Reads the Revit version from a template file name such as BGCT_R2024.rvt. Returns 0 if it can't be read.
        /// </summary>
        private static int GetTemplateVersion(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);

            return int.TryParse(name.Substring(name.LastIndexOf('R') + 1), out var version) ? version : 0;
        }
    }
}
