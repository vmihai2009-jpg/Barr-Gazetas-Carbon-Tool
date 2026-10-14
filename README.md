# Barr Gazetas Carbon Tool

Embodied carbon add-in for **Revit 2025, 2026 and 2027**. It reads a carbon schedule in the Revit model,
then builds charts and a summary sheet. Derived from the open-source HBERT project (see [NOTICE.md](NOTICE.md)).

## Build

Needs the .NET SDK for your Revit version: .NET 8 for Revit 2025/2026, .NET 10 for Revit 2027 (Windows).

    dotnet build src/RevitAddIn/RevitAddIn.csproj -c Release -p:RevitVersion=2025

Repeat with 2026 or 2027. Revit API assemblies come from NuGet, so Revit does not need to be installed to compile.
GitHub Actions builds all three on every push (see the Actions tab for downloadable builds).

## Install

Run as administrator after building:

    powershell -ExecutionPolicy Bypass -File installer\Install.ps1 -Versions 2025,2026

Then start Revit: a **Barr Gazetas** tab appears with the tool button. `-Uninstall` removes it.

## Status

Ported and rebranded from a Revit 2018-2022 / .NET Framework 4.8 codebase. **Not yet tested inside Revit.**
Known follow-ups:
- `templates/BGCT_R2024.rvt` is the original sample template (Revit upgrades it on open). The tool looks up the schedule
  "Embodied Carbon (Do Not Delete)" and the title block "HB_Titleblock_A3_Landscape_Vertical" inside it, so those names still
  come from that file. Replace the template and update `ApplicationSettings.cs` to brand them.
- The window logo and ribbon icons are the original HBERT icons; swap in Barr Gazetas artwork in the `Resources` folders.
- The accent colour in `Styles/ColorStyles.xaml` is a placeholder.
