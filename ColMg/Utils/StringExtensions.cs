using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ColMg.Utils
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
            Regex.Replace(
                str,
                @"(\P{Ll})(\P{Ll}\p{Ll})",
                "$1 $2"
            ),
            @"(\p{Ll})(\P{Ll})",
            "$1 $2");
        }

        public static string Capitalize(this string str)
        {
            switch(str)
            {
                case null:
                case "":
                    return str;
                default:
                    return string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1));
            }
        }
    }
}
