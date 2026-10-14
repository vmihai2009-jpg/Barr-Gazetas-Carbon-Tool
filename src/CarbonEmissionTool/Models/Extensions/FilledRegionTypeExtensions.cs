using Autodesk.Revit.DB;

namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// Extension methods for <see cref="FilledRegionType"/>
    /// </summary>
    public static class FilledRegionTypeExtensions
    {
        /// <summary>
        /// Sets the color and fill pattern of the <see cref="FilledRegionType"/>.
        /// </summary>
        public static void SetColorAndPattern(this FilledRegionType filledRegionType, System.Windows.Media.Color newColor, FillPatternElement fillPattern)
        {
            var colorInt = ColorUtils.ConvertRBGToInt(newColor.R, newColor.G, newColor.B);
            filledRegionType.get_Parameter(BuiltInParameter.BACKGROUND_PATTERN_COLOR_PARAM).Set(colorInt);

            filledRegionType.get_Parameter(BuiltInParameter.BACKGROUND_DRAFT_PATTERN_ID_PARAM).Set(fillPattern.Id);

            // Set the foreground pattern to null so it is invisible.
            filledRegionType.get_Parameter(BuiltInParameter.FOREGROUND_ANY_PATTERN_ID_PARAM).Set(ElementId.InvalidElementId);
        }
    }
}
