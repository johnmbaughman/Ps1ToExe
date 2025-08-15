---
description: Information about anti-virus and packaged applications.
---

# Anti-Virus

PowerShell that is packaged as an executable can often trigger anti-virus software after packaged. The use of PowerShell within malicious executables has led to false positives being found in your own packages. This document contains information about anti-virus and PowerShell Pro Tools packaging.&#x20;

{% hint style="info" %}
These tests were completed with PowerShell Pro Tools 2021.9.2.&#x20;
{% endhint %}

## Default Package Detection

The packaging itself will not flag anti-virus for most providers. You will see that if you package an empty PS1 file and upload it to VirusTotal, most AV providers will not flag the executable.&#x20;

{% embed url="https://www.virustotal.com/gui/file/1a81a4317dc578cac298a22c3bd420b0b9c3bd19b2628adf5f2f10565dfaf899?nocache=1" %}



![](https://3667946160-files.gitbook.io/~/files/v0/b/gitbook-legacy-files/o/assets%2F-LNFE66tpE_51uobNA70%2F-MlVrrfqqSK26C3z0nGA%2F-MlVshA3VyjqSKopRM-M%2Fimage.png?alt=media\&token=aa1a9358-d1d3-45d1-950d-b29dfc30787f)

The package settings for this are the default ones provided by the Visual Studio Code extension.&#x20;

```
@{
    Root = 'c:\Users\adamr\Desktop\Tests\test.ps1'
    OutputPath = 'c:\Users\adamr\Desktop\Tests\out'
    Package = @{
        Enabled = $true
        Obfuscate = $false
        HideConsoleWindow = $false
        DotNetVersion = 'v4.6.2'
        FileVersion = '1.0.0'
        FileDescription = ''
        ProductName = ''
        ProductVersion = ''
        Copyright = ''
        RequireElevation = $false
        ApplicationIconPath = ''
        PackageType = 'Console'
    }
    Bundle = @{
        Enabled = $true
        Modules = $true
        # IgnoredModules = @()
    }
}
```

## Custom Script Results

As you can see when we start to include additional PowerShell script, you will begin to see certain AV providers begin to flag the results. This example uses the [Clean Windows 10 PowerShell](https://gist.github.com/halkyon/b73fb75e61c37b7ba5f65bb6f3979f00) script.&#x20;

Additionally, we enabled require elevation in the package.psd1 file to ensure that administrator access is allowed to modify the Windows features.&#x20;

```
@{
    Root       = 'c:\Users\adamr\Desktop\Tests\test.ps1'
    OutputPath = 'c:\Users\adamr\Desktop\Tests\out'
    Package    = @{
        Enabled             = $true
        Obfuscate           = $false
        HideConsoleWindow   = $false
        DotNetVersion       = 'v4.6.2'
        FileVersion         = '1.0.0'
        FileDescription     = ''
        ProductName         = ''
        ProductVersion      = ''
        Copyright           = ''
        RequireElevation    = $true
        ApplicationIconPath = ''
        PackageType         = 'Console'
    }
    Bundle     = @{
        Enabled = $true
        Modules = $true
        # IgnoredModules = @()
    }
}
```

This example yields another false positive.&#x20;

{% embed url="https://www.virustotal.com/gui/file/2f61c7f8bd7aaa2d5d3fed908b40b36f00fd6367a52e7c584641d33f6134ad8f?nocache=1" %}



![](https://3667946160-files.gitbook.io/~/files/v0/b/gitbook-legacy-files/o/assets%2F-LNFE66tpE_51uobNA70%2F-MlVrrfqqSK26C3z0nGA%2F-MlVtr6D6iaGtEyPoTjD%2Fimage.png?alt=media\&token=685c5df8-2200-45c4-9f34-8e305ca2a82d)

## Obfuscation

{% hint style="info" %}
Obfuscation does not work with PowerShell 7
{% endhint %}

Obfuscation of the assembly greatly increases the probability of the executable being marked as malicious. We are using the same Windows 10 clean up script but have now enabled obfuscation.

The package.psd1 has been updated to turn on obfuscation.&#x20;

```
@{
    Root       = 'c:\Users\adamr\Desktop\Tests\test.ps1'
    OutputPath = 'c:\Users\adamr\Desktop\Tests\out'
    Package    = @{
        Enabled             = $true
        Obfuscate           = $true
        HideConsoleWindow   = $true
        DotNetVersion       = 'v4.6.2'
        FileVersion         = '1.0.0'
        FileDescription     = ''
        ProductName         = ''
        ProductVersion      = ''
        Copyright           = ''
        RequireElevation    = $true
        ApplicationIconPath = ''
        PackageType         = 'Console'
    }
    Bundle     = @{
        Enabled = $true
        Modules = $true
        # IgnoredModules = @()
    }
}
        
```

As you can see, the number of AV vendors to flag the assembly has increased to 10.&#x20;

{% embed url="https://www.virustotal.com/gui/file/eaa796e33fc476ef8b1a168aab5f411fff58ec66610a54edfebd7e71f88e54fb?nocache=1" %}



![](https://3667946160-files.gitbook.io/~/files/v0/b/gitbook-legacy-files/o/assets%2F-LNFE66tpE_51uobNA70%2F-MlVrrfqqSK26C3z0nGA%2F-MlVuXoyJzXxTxK17_Do%2Fimage.png?alt=media\&token=55147b88-b2df-4b37-9638-1cc5e99ee3eb)

## Code Signing

While not directly supported within PowerShell Pro Tools, code signing can reduce the number of false positives. Using the same script and package settings as in the obfuscated example, we can use `signtool` to sign the resulting executable.&#x20;

```
signtool sign /f some.pfx /sha1 MYSHA1 /p MYPASSWORD .\text.exe
```

This results in an executable that is only flagged by 2 vendors.&#x20;

{% embed url="https://www.virustotal.com/gui/file/ba87f47b2fd6da78b7dc254c0ab408d5c4d0f95f04a5d46a2af7c2fe52a3fdd6" %}







![](https://3667946160-files.gitbook.io/~/files/v0/b/gitbook-legacy-files/o/assets%2F-LNFE66tpE_51uobNA70%2F-MlVrrfqqSK26C3z0nGA%2F-MlVve8HyhD8kykmtgQE%2Fimage.png?alt=media\&token=05b6ddb1-5a5c-49c3-8324-653b1262bb1e)

## Modules

Including modules also increases the probability of false positives. In this example, we are packaging the `ActiveDirectory` module and obfuscating the resulting binary. 12 vendors flag this executable.

{% embed url="https://www.virustotal.com/gui/file/d7a9a87a358cb5aedd3326c55f6bdd42e9575055602192e7d66553012fb3f23e?nocache=1" %}



![](https://3667946160-files.gitbook.io/~/files/v0/b/gitbook-legacy-files/o/assets%2F-LNFE66tpE_51uobNA70%2F-MlVrrfqqSK26C3z0nGA%2F-MlVx2S5EvPh6w_OHDFk%2Fimage.png?alt=media\&token=73451ef3-1b84-4666-8325-56062064a3c7)

Again, signing the binary greatly reduces the number of false positives. Only 3 vendors now flag that same binary.

{% embed url="https://www.virustotal.com/gui/file/b7df37aa077de8f38e73b9d1d4fc4bbf5908c12347fd16c5ca4b13408800c1a0?nocache=1" %}

![](https://3667946160-files.gitbook.io/~/files/v0/b/gitbook-legacy-files/o/assets%2F-LNFE66tpE_51uobNA70%2F-MlVrrfqqSK26C3z0nGA%2F-MlVxQBWYdpqUAtX-jzv%2Fimage.png?alt=media\&token=45adef1f-55d5-4c32-9960-bfaf80ffe900)

## PowerShell 7

PowerShell 7 executables are packaged differently. Rather than relying on the local PowerShell DLLs, the PowerShell SDK is packaged with the binary. This results in a larger binary but it's completely self-contained.&#x20;

I've changed the package settings to this. We are using the .NET Core 3.1 SDK and the 7.0.3 PowerShell SDK. This is again packaging the clean up script and the Active Directory module.&#x20;

```
@{
    Root       = 'c:\Users\adamr\Desktop\Tests\test.ps1'
    OutputPath = 'c:\Users\adamr\Desktop\Tests\out'
    Package    = @{
        Enabled             = $true
        Obfuscate           = $false
        HideConsoleWindow   = $true
        DotNetVersion       = 'netcoreapp3.1'
        PowerShellVersion   = '7.0.3'
        FileVersion         = '1.0.0'
        FileDescription     = ''
        ProductName         = ''
        ProductVersion      = ''
        Copyright           = ''
        RequireElevation    = $false
        ApplicationIconPath = ''
        PackageType         = 'Console'
    }
    Bundle     = @{
        Enabled = $true
        Modules = $true
        # IgnoredModules = @()
    }
}
        
```

In this example, the package is flagged by 1 AV vendor.&#x20;

{% embed url="https://www.virustotal.com/gui/file/e9245e9f1672b913eda01a20e684119fdb30d4ea1c7132d0a5707e2b9f1ab470?nocache=1" %}

![](https://3667946160-files.gitbook.io/~/files/v0/b/gitbook-legacy-files/o/assets%2F-LNFE66tpE_51uobNA70%2F-MlVrrfqqSK26C3z0nGA%2F-MlVyW2rkrKEop4xqvQc%2Fimage.png?alt=media\&token=642d6d5a-861c-41b8-a269-834e9768c8ae)

## Cmdlet Usage

Certain cmdlets will also cause AV engines to flag. For example, if you use the `Set-MpPreference` cmdlet within your script, it may flag several vendors. This cmdlet is used for disabling Windows Defender.&#x20;

```
Set-MpPreference -DisableRealtimeMonitoring $true
```

This script was flagged by 8 vendors.&#x20;

{% embed url="https://www.virustotal.com/gui/file/38d7bf811117c99652a4378661da7f307a98b2f946abe85867ca60fb9dc9423e?nocache=1" %}

![](https://3667946160-files.gitbook.io/~/files/v0/b/gitbook-legacy-files/o/assets%2F-LNFE66tpE_51uobNA70%2F-MlVrrfqqSK26C3z0nGA%2F-MlVzHysUMQLuZEOdIE7%2Fimage.png?alt=media\&token=45bc3f91-0d1f-4154-bf02-64062f9438d9)

Obfuscating and code signing this same assembly, reduces that number to 2.

{% embed url="https://www.virustotal.com/gui/file/17bb2ee29b1693ed62e030e13967ecfd996616bfda64909d7db473eb1f3ee641?nocache=1" %}

![](https://3667946160-files.gitbook.io/~/files/v0/b/gitbook-legacy-files/o/assets%2F-LNFE66tpE_51uobNA70%2F-MlVrrfqqSK26C3z0nGA%2F-MlVzoeZFcYdtQkWURbs%2Fimage.png?alt=media\&token=d6f284dc-e226-4ce7-bc8f-339102b4aafe)
