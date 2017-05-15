Visual Studio Revit Add-in Template
===================================

![Revit version](https://img.shields.io/badge/Revit-2014%20%E2%9E%9C%202019-blue.svg)
![Platform](https://img.shields.io/badge/platform-Windows-red.svg)
![License](https://img.shields.io/github/license/Equipple/vs-templates-revit-addin.svg)

Visual Studio C# (project) template for easy [Revit](https://en.wikipedia.org/wiki/Autodesk_Revit) add-in creation.

## Know issues

The new csproj format has known issues with [WinForms](https://en.wikipedia.org/wiki/Windows_Forms) / [WPF](https://en.wikipedia.org/wiki/Windows_Presentation_Foundation) files and classes showing and editing correctly in Visual Studio, Microsoft has already announced they will fix it in [.NET Core 3](https://blogs.msdn.microsoft.com/dotnet/2018/05/07/net-core-3-and-support-for-windows-desktop-applications/) release.

## License

This project is licensed under the terms of the [MIT License](LICENSE).

## Credits

This template has been inspired by both [Jeremy Tammik](https://github.com/jeremytammik)'s [project templates](https://github.com/jeremytammik/VisualStudioRevitAddinWizard) and [Sander Obdeijn](https://github.com/sanderobdeijn)'s [blog post](http://buildingknowledge.eu/custom-msbuild-targets-for-compiling-addins-for-multiple-revit-versions/) about multi-targeteting different versions of Revit from one project.