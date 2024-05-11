using System;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static bool TryEnum<T>(this string str, out T result) where T : struct
            => Enum.TryParse(str, true, out result);
    }
}