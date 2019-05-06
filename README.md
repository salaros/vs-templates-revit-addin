Visual Studio Revit Add-in Template [![Build status](https://ci.appveyor.com/api/projects/status/vxajagat45cfeeky?svg=true)](https://ci.appveyor.com/project/salaros/vs-templates-revit-addin)
===================================

![Revit version](https://img.shields.io/badge/Revit-2014%20%E2%9E%9C%202019-blue.svg)
![Github All Releases](https://img.shields.io/github/downloads/Equipple/vs-templates-revit-addin/total.svg)
![GitHub tag](https://img.shields.io/github/tag/Equipple/vs-templates-revit-addin.svg)
![Platform](https://img.shields.io/badge/platform-Windows-red.svg)
![License](https://img.shields.io/github/license/Equipple/vs-templates-revit-addin.svg)

Visual Studio C# (project) template for easy [Revit](https://en.wikipedia.org/wiki/Autodesk_Revit) add-in creation.

## Installation & Update

Just grab the latest release (.zip file) from [releases page](https://github.com/Equipple/vs-templates-revit-addin/releases) and copy it into 

```
"%USERPROFILE%\Documents\Visual Studio 2019\Templates\ProjectTemplates\Visual C#"
```

(use Visual Studio 2017 if you have that version)


## 🟊🟊🟊 Support this project 🟊🟊🟊

You can support us in a small way, please consider starring and sharing this repo! It helps us getting known and grow the community.
![star us](.github/assets/star_us.gif)


## Personalization

#### Add-in metadata

Unlike other add-in template this one doesn't require constantly updating .addin file. It takes all the information from project properties:

![Project properties](assets/properties-add-in-data.png "Project properties")

#### Revit versions

By default the newly created add-in will target Revit x64 2014-2019, you could target fewer versions by simply removing items from `TargetFrameworks`:

![Revit versions](assets/revit-versions.png "Revit versions")

## Debug

For your convenience in `Debug` configuration the add-in will copy its manifest to `%AppData%\Autodesk\Revit\Addins` folder with Assembly tags pointing to your compilation output folder.

Before starting the debugger make sure you have selected the proper Revit version:

![Debugger selection](assets/debugger-selection.png "Debugger selection")

If Revit won't start probably you have installed it to a non-default location and you have to modify [launchSettings.json](src/Properties/launchSettings.json) file.

You could also use [launchSettings.json](src/Properties/launchSettings.json) to tweak command line parameters passed to Revit.exe file, e.g. you might want Revit to open a project template / family file for every debug session you start.

## License

This project is licensed under the terms of the [MIT License](LICENSE).

## Credits

This template has been inspired by both [Jeremy Tammik](https://github.com/jeremytammik)'s [project templates](https://github.com/jeremytammik/VisualStudioRevitAddinWizard) and [Sander Obdeijn](https://github.com/sanderobdeijn)'s [blog post](http://buildingknowledge.eu/custom-msbuild-targets-for-compiling-addins-for-multiple-revit-versions/) about multi-targeteting different versions of Revit from one project.