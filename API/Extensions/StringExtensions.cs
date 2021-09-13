using System;
using System.Linq;

namespace API.Extensions
{
    public static class StringExtensions
    {
        public static int WordCount(this string text)
        {
            return text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();
        }
        public static int UniqueCharCount(this string text)
        {
            return text.Distinct().Aggregate(string.Empty,(current, c) => current + c.ToString()).Count();
        }        
    }
}