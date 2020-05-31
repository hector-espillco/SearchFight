using SearchFight.Domain.Enum;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SearchFight.Application.Helper
{
    public static class Extension
    {
        public static string GetValue(this SearchType searchType)
        {
            FieldInfo fi = searchType.GetType().GetField(searchType.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return searchType.ToString();
        }
    }
}
