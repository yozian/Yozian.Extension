using System;

namespace Yozian.Extension;

/// <summary>
/// 
/// </summary>
public static class VersionExtension
{
    public static Version IncreaseMajor(this Version @this)
    {
        return new Version(@this.Major + 1, 0, 0);
    }

    public static Version IncreaseMinor(this Version @this)
    {
        return new Version(@this.Major, @this.Minor + 1, 0);
    }

    public static Version IncreaseBuild(this Version @this)
    {
        return new Version(@this.Major, @this.Minor, @this.Build + 1);
    }
}
