# Packaging on Linux

Packaging is supported on Linux systems. Packaged executables will contain the entire PowerShell and .NET runtime so destination systems will not need either of these installed.&#x20;

## Prerequisites&#x20;

You will need to install the following in order to package on Linux

* [.NET Core SDK 3.1 or later](https://docs.microsoft.com/en-us/dotnet/core/install/linux)
* [PowerShell 7 or later](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-linux?view=powershell-7.1)

Once you have them installed, you can setup your script for packaging.&#x20;

## Configuration

You will need to create a [Package.psd1](package.psd1) file in order to package. Here is an example configuration that will package the `test.ps1` script and output it to the `desktop` of the mounted Windows drive in WSL2. You need to ensure that you set the .NET framework version to `netcoreapp31` and the platform to `linux-x64`.&#x20;

```
@{
    Root = '/mnt/c/Users/adamr/desktop/test.ps1' # Root script to package. This is the main entry point for the package. 
    OutputPath = '/mnt/c/Users/adamr/desktop/out' # The output directory for the packaging process. 
    Package = @{
        Enabled = $true # Whether to package as an executable. 
        DotNetVersion = 'netcoreapp31'
        PackageType = 'Console' # The type of executable to generate. Valid values are Service or Console. 
        PowerShellArguments = '' # Sets the arguments for the PowerShell process that is hosted within the executable. You can use arguments like -NoExit, -ExecutionPolicy and -NoProfile.
        Platform = 'x64' # Sets the architecture of the executable. Can be either 'x86' or 'x64'
        PowerShellVersion = '7.0.3' # You can specify Windows PowerShell or PowerShell 7 or later versions version (e.g. 7.0.0)
        RuntimeIdentifier = 'linux-x64' # You can specify other runtimes like linux-x64 (See .NET Core runtime identifiers)
    }
    Bundle = @{
        Enabled = $true # Whether to bundle multiple PS1s into a single PS1. Always enabled when Package is enabled. 
        Modules = $true # Whether to bundle modules into the package
    }
}
```

By default, some core modules are included. Additional modules will also be included when enabling the Modules bundle.&#x20;

## Running the Packager

You can run the packager by using the `Merge-Script` cmdlet of the PowerShell Pro Tools module. If you include the `-Verbose` flag, you will see output from the packaging process.&#x20;

In this example, we have a script named `test.ps1` with the following content.&#x20;

```
"Hello. I'm running on $($PSVersionTable.Platform)"
```

You can install the PowerShell Pro Tools module and then run merge script against the package.psd1 file we created earlier.&#x20;

```
Install-Module PowerShellProTools
Merge-Script -ConfigFile ./package.psd1 -Verbose
VERBOSE: Checking license
VERBOSE: OutputPath is /mnt/c/Users/adamr/desktop/out
VERBOSE: Bundling /mnt/c/Users/adamr/desktop/test.ps1
VERBOSE: Packaging /tmp/test.ps1
VERBOSE: Creating temp directory: /tmp/259a5b5f8e164250af2fb04c10e1b829
VERBOSE: Packaging modules...
VERBOSE: Checking dotnet version.
VERBOSE: Checking dotnet version.
VERBOSE: 5.0.102

VERBOSE: 5.0.102

VERBOSE: Creating package project.
VERBOSE: Using .NET Framework version: netcoreapp31
VERBOSE:   Determining projects to restore...
  Restored /tmp/259a5b5f8e164250af2fb04c10e1b829/test.csproj (in 1.22 sec).

VERBOSE:   Determining projects to restore...
  Restored /tmp/259a5b5f8e164250af2fb04c10e1b829/test.csproj (in 1.22 sec).

VERBOSE: Packaging /tmp/test.ps1 -> /mnt/c/Users/adamr/desktop/out/test
VERBOSE: Microsoft (R) Build Engine version 16.8.3+39993bd9d for .NET
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  Restored /tmp/259a5b5f8e164250af2fb04c10e1b829/test.csproj (in 566 ms).
  test -> /tmp/259a5b5f8e164250af2fb04c10e1b829/bin/Debug/netcoreapp3.1/linux-x64/test.dll
  test -> /mnt/c/Users/adamr/desktop/out/

VERBOSE: Microsoft (R) Build Engine version 16.8.3+39993bd9d for .NET
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  Restored /tmp/259a5b5f8e164250af2fb04c10e1b829/test.csproj (in 566 ms).
  test -> /tmp/259a5b5f8e164250af2fb04c10e1b829/bin/Debug/netcoreapp3.1/linux-x64/test.dll
  test -> /mnt/c/Users/adamr/desktop/out/
```

After the packaging process is done, you can run your executable.&#x20;

```
PS /mnt/c/Users/adamr> ./Desktop/out/test
Hello. I'm running on Unix
```
