ReClass.NET UnrealEngine Classes Plugin
=================================

A ReClass.NET plugin which adds support for some Unreal Engine core classes.

## Supported Classes
- TArray
- TSharedPtr
- FGuid
- FString
- FQWord
- FDateTime

## Installation
- Compile or download from https://github.com/ReClassNET/ReClass.NET-UnrealEngineClassesPlugin/releases
- Copy the dll files in the appropriate Plugin folder (ReClass.NET/x86/Plugins or ReClass.NET/x64/Plugins)
- Start ReClass.NET and check the plugins form if the UnrealEngine Classes plugin is listed.

## Compiling
If you want to compile the ReClass.NET UnrealEngine Classes Plugin just clone the repository and create the following folder structure. If you don't use this structure you need to fix the project references.

```
..\ReClass.NET\
..\ReClass.NET\ReClass.NET\ReClass.NET.csproj
..\ReClass.NET-UnrealEngineClassesPlugin
..\ReClass.NET-UnrealEngineClassesPlugin\UnrealEngineClassesPlugin.csproj
```