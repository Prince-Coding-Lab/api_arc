using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ShnoFeeh.API.Core.Enums
{
    public enum ResponseStatus
    {
        [EnumMember(Value = "OK")]
        [Description("OK")]
        OK = 200,

        [EnumMember(Value = "Bad Request")]
        [Description("Bad Request")]
        BadRequest = 400,

        [EnumMember(Value = "Forbidden")]
        [Description("Forbidden")]
        Forbidden = 403,


        [EnumMember(Value = "Server Error")]
        [Description("Internal Server Error")]
        ServerError = 500,
    }
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            }
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }
    }
}
