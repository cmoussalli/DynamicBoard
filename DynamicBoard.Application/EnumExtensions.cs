using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DynamicBoard.Application
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName() ?? enumValue.ToString();
        }
        public static (T EnumValue, string DisplayName) ParseEnumValue<T>(int intValue) where T : Enum
        {
            T enumValue = (T)Enum.ToObject(typeof(T), intValue);
            string displayName = enumValue.GetDisplayName();
            return (enumValue, displayName);
        }
    }
}
