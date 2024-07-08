    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System;

namespace T8MM.Utils;

public static class Debug
{
    public static void Log(string value)
    {
#if DEBUG
        Console.WriteLine($"LOG: {value}");
#endif
    }

    public static void Warning(string value)
    {
#if DEBUG
        Console.WriteLine($"WARN: {value}");
#endif
    }
    
    public static void Error(string value)
    {
#if DEBUG
        Console.WriteLine($"ERROR: {value}");
#endif
    }

    public static void Assert(bool target, string value)
    {
#if DEBUG
        if (!target)
            Console.WriteLine($"ERROR: {value}");
#endif
    }
}