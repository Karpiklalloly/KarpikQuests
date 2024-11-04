using System.ComponentModel;
using System.Globalization;
using Karpik.Quests.ID;

namespace Karpik.Quests
{
    public class IdConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(Id) || base.CanConvertTo(context, destinationType);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            return ((Id)value).Value;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string to)
            {
                return new Id(to);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}