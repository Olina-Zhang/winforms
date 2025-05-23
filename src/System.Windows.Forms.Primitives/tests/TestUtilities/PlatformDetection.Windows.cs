﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32;

namespace System;

public static partial class PlatformDetection
{
    public static bool IsNetFramework => RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);
    public static Version OSXVersion => throw new PlatformNotSupportedException();
    public static Version OpenSslVersion => throw new PlatformNotSupportedException();
    public static bool IsSuperUser => throw new PlatformNotSupportedException();
    public static bool IsCentos6 => false;
    public static bool IsOpenSUSE => false;
    public static bool IsUbuntu => false;
    public static bool IsDebian => false;
    public static bool IsAlpine => false;
    public static bool IsDebian8 => false;
    public static bool IsUbuntu1404 => false;
    public static bool IsUbuntu1604 => false;
    public static bool IsUbuntu1704 => false;
    public static bool IsUbuntu1710 => false;
    public static bool IsUbuntu1710OrHigher => false;
    public static bool IsUbuntu1804 => false;
    public static bool IsTizen => false;
    public static bool IsNotFedoraOrRedHatFamily => true;
    public static bool IsFedora => false;
    public static bool IsWindowsNanoServer => (IsNotWindowsIoTCore && GetInstallationType().Equals("Nano Server", StringComparison.OrdinalIgnoreCase));
    public static bool IsWindowsServerCore => GetInstallationType().Equals("Server Core", StringComparison.OrdinalIgnoreCase);
    public static int WindowsVersion => GetWindowsVersion();
    public static bool IsMacOsHighSierraOrHigher { get; }
    public static Version ICUVersion => new(0, 0, 0, 0);
    public static bool IsRedHatFamily => false;
    public static bool IsNotRedHatFamily => true;
    public static bool IsRedHatFamily6 => false;
    public static bool IsRedHatFamily7 => false;
    public static bool IsNotRedHatFamily6 => true;

    public static bool IsWindows10Version1607OrGreater =>
        GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 14393;
    public static bool IsWindows10Version1703OrGreater =>
        GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 15063;
    public static bool IsWindows10Version1709OrGreater =>
        GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 16299;
    public static bool IsWindows10Version1803OrGreater =>
        GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 17134;
    public static bool IsWindows11OrHigher =>
        GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 22000;

    // Windows OneCoreUAP SKU doesn't have httpapi.dll
    public static bool IsNotOneCoreUAP =>
        File.Exists(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "httpapi.dll"));

    public static bool IsWindowsIoTCore => GetWindowsProductType() is PRODUCT_IOTUAPCOMMERCIAL or PRODUCT_IOTUAP;

    public static bool IsWindowsHomeEdition => GetWindowsProductType() switch
    {
        PRODUCT_CORE
            or PRODUCT_CORE_COUNTRYSPECIFIC
            or PRODUCT_CORE_N
            or PRODUCT_CORE_SINGLELANGUAGE
            or PRODUCT_HOME_BASIC
            or PRODUCT_HOME_BASIC_N
            or PRODUCT_HOME_PREMIUM
            or PRODUCT_HOME_PREMIUM_N => true,
        _ => false,
    };

    public static bool IsWindows => true;
    public static bool IsWindows7 => GetWindowsVersion() == 6 && GetWindowsMinorVersion() == 1;
    public static bool IsNotWindows7 => !IsWindows7;
    public static bool IsWindows8x => GetWindowsVersion() == 6 && (GetWindowsMinorVersion() == 2 || GetWindowsMinorVersion() == 3);

    public static string LibcRelease => "glibc_not_found";
    public static string LibcVersion => "glibc_not_found";

    public static string GetDistroVersionString() => $"WindowsProductType={GetWindowsProductType()} WindowsInstallationType={GetInstallationType()}";

    private static int s_isInAppContainer = -1;

    public static bool IsInAppContainer
    {
        // This actually checks whether code is running in a modern app.
        // Currently this is the only situation where we run in app container.
        // If we want to distinguish the two cases in future,
        // EnvironmentHelpers.IsAppContainerProcess in desktop code shows how to check for the AC token.
        get
        {
            if (s_isInAppContainer != -1)
            {
                return s_isInAppContainer == 1;
            }

            if (!IsWindows || IsWindows7)
            {
                s_isInAppContainer = 0;
                return false;
            }

            byte[] buffer = [];
            uint bufferSize = 0;
            try
            {
                int result = GetCurrentApplicationUserModelId(ref bufferSize, buffer);
                s_isInAppContainer = result switch
                {
                    // APPMODEL_ERROR_NO_APPLICATION
                    15703 => 0,
                    // ERROR_SUCCESS
                    0 or 122 => 1, // Success is actually insufficent buffer as we're really only looking for
                                   // not NO_APPLICATION and we're not actually giving a buffer here. The
                                   // API will always return NO_APPLICATION if we're not running under a
                                   // WinRT process, no matter what size the buffer is.
                    _ => throw new InvalidOperationException($"Failed to get AppId, result was {result}."),
                };
            }
            catch (Exception e)
            {
                // We could catch this here, being friendly with older portable surface area should we
                // desire to use this method elsewhere.
                if (e.GetType().FullName.Equals("System.EntryPointNotFoundException", StringComparison.Ordinal))
                {
                    // API doesn't exist, likely pre Win8
                    s_isInAppContainer = 0;
                }
                else
                {
                    throw;
                }
            }

            return s_isInAppContainer == 1;
        }
    }

    private static int s_isWindowsElevated = -1;

    public static bool IsWindowsAndElevated
    {
        get
        {
            if (s_isWindowsElevated != -1)
            {
                return s_isWindowsElevated == 1;
            }

            if (!IsWindows || IsInAppContainer)
            {
                s_isWindowsElevated = 0;
                return false;
            }

            Assert.True(OpenProcessToken(PInvoke.GetCurrentProcess(), TOKEN_READ, out IntPtr processToken));

            try
            {
                Assert.True(GetTokenInformation(
                    processToken, TokenElevation, out uint tokenInfo, sizeof(uint), out uint returnLength));

                s_isWindowsElevated = tokenInfo == 0 ? 0 : 1;
            }
            finally
            {
                PInvoke.CloseHandle((HANDLE)processToken);
            }

            return s_isWindowsElevated == 1;
        }
    }

    private static string GetInstallationType()
    {
        string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
        string value = string.Empty;

        try
        {
            value = (string)Registry.GetValue(key, "InstallationType", defaultValue: "");
        }
        catch (Exception e) when (e is SecurityException or InvalidCastException or PlatformNotSupportedException /* UAP */)
        {
        }

        return value;
    }

    private static int GetWindowsProductType()
    {
        Assert.True(GetProductInfo(Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor, 0, 0, out int productType));
        return productType;
    }

    private static unsafe int GetWindowsMinorVersion()
    {
        RTL_OSVERSIONINFOEX osvi = new()
        {
            dwOSVersionInfoSize = (uint)sizeof(RTL_OSVERSIONINFOEX)
        };
        Assert.Equal(0, RtlGetVersion(ref osvi));
        return (int)osvi.dwMinorVersion;
    }

    private static unsafe int GetWindowsBuildNumber()
    {
        RTL_OSVERSIONINFOEX osvi = new()
        {
            dwOSVersionInfoSize = (uint)sizeof(RTL_OSVERSIONINFOEX)
        };
        Assert.Equal(0, RtlGetVersion(ref osvi));
        return (int)osvi.dwBuildNumber;
    }

    private const uint TokenElevation = 20;
    private const uint STANDARD_RIGHTS_READ = 0x00020000;
    private const uint TOKEN_QUERY = 0x0008;
    private const uint TOKEN_READ = STANDARD_RIGHTS_READ | TOKEN_QUERY;

    [DllImport("advapi32.dll", SetLastError = true, ExactSpelling = true)]
    private static extern bool GetTokenInformation(
        IntPtr TokenHandle,
        uint TokenInformationClass,
        out uint TokenInformation,
        uint TokenInformationLength,
        out uint ReturnLength);

    private const int PRODUCT_IOTUAP = 0x0000007B;
    private const int PRODUCT_IOTUAPCOMMERCIAL = 0x00000083;
    private const int PRODUCT_CORE = 0x00000065;
    private const int PRODUCT_CORE_COUNTRYSPECIFIC = 0x00000063;
    private const int PRODUCT_CORE_N = 0x00000062;
    private const int PRODUCT_CORE_SINGLELANGUAGE = 0x00000064;
    private const int PRODUCT_HOME_BASIC = 0x00000002;
    private const int PRODUCT_HOME_BASIC_N = 0x00000005;
    private const int PRODUCT_HOME_PREMIUM = 0x00000003;
    private const int PRODUCT_HOME_PREMIUM_N = 0x0000001A;

    [DllImport("kernel32.dll", SetLastError = false)]
    private static extern bool GetProductInfo(
        int dwOSMajorVersion,
        int dwOSMinorVersion,
        int dwSpMajorVersion,
        int dwSpMinorVersion,
        out int pdwReturnedProductType);

    [DllImport("ntdll.dll", ExactSpelling = true)]
    private static extern int RtlGetVersion(ref RTL_OSVERSIONINFOEX lpVersionInformation);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private unsafe struct RTL_OSVERSIONINFOEX
    {
#pragma warning disable IDE1006 // Naming Styles - matching OS
        internal uint dwOSVersionInfoSize;
        internal uint dwMajorVersion;
        internal uint dwMinorVersion;
        internal uint dwBuildNumber;
        internal uint dwPlatformId;
        internal fixed char szCSDVersion[128];
#pragma warning restore IDE1006
    }

    private static unsafe int GetWindowsVersion()
    {
        RTL_OSVERSIONINFOEX osvi = new()
        {
            dwOSVersionInfoSize = (uint)sizeof(RTL_OSVERSIONINFOEX)
        };
        Assert.Equal(0, RtlGetVersion(ref osvi));
        return (int)osvi.dwMajorVersion;
    }

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern int GetCurrentApplicationUserModelId(ref uint applicationUserModelIdLength, byte[] applicationUserModelId);

    [DllImport("advapi32.dll", SetLastError = true, ExactSpelling = true)]
    private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
}
