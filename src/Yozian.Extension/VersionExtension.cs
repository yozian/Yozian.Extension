using System;

namespace Yozian.Extension;

/// <summary>
/// 
/// </summary>
public static class VersionExtension
{
    extension(Version @this)
    {
        public Version IncreaseMajor()
        {
            return new Version(
                @this.Major + 1,
                0,
                0
            );
        }

        public Version IncreaseMinor()
        {
            return new Version(
                @this.Major,
                @this.Minor + 1,
                0
            );
        }

        public Version IncreaseBuild()
        {
            return new Version(
                @this.Major,
                @this.Minor,
                @this.Build + 1
            );
        }
    }
}