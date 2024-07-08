    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace T8MM.Utils;

public static class ModUtils
{
    public static bool ModPackageNameValid(string packageName)
    {
        var packageNameRegex = @".*-[0-9]+-[0-9]+-[0-9]+";
        return Regex.IsMatch(packageName, packageNameRegex);
    }
    
    
    public static void GetBasicInfoByPackage(string packageName, out string name)
    {
        name = string.Empty;
        var values = packageName.Split('-');
        if (values.Length != 0)
        {
            name = values[0];
        }
    }

    public static void GetBasicInfoByPackage(string packageName, out string name, out int id)
    {
        name = string.Empty;
        id = -1;
        var values = packageName.Split('-');
        if (values.Length != 0)
        {
            name = values[0];
            id = int.Parse(values[1]);
        }
    }
    
    public static void GetBasicInfoByPackage(string packageName, out string name, out int id, out string version)
    {
        name = string.Empty;
        id = -1;
        version = string.Empty;
        var values = packageName.Split('-');
        if (values.Length != 0)
        {
            name = values[0];
            id = int.Parse(values[1]);
            version = string.Join('.', values[2..^1]);
        }
    }

    public static void GetBasicInfoByPackage(string packageName, out string name, out int id, out string version, out string updateTime)
    {
        name = string.Empty;
        id = -1;
        version = string.Empty;
        updateTime = string.Empty;
        // test
        // GothGirl-255 -1-1-1718479841.rar
        // Majestic Dragons Glorious-118638-2-0-1719020526.rar
        // GothGirl                     -255    -1-1-   1718479841.rar 
        // Majestic Dragons Glorious    -118638 -2-0-   1719020526.rar
        //    0                         1       2~[l-1](la-1-2)        l
        var values = packageName.Split('-');
        if (values.Length != 0)
        {
            name = values[0];
            id = int.Parse(values[1]);
            version = string.Join('.', values[2..^1]);
            updateTime = values[^1];
        }
    }
}