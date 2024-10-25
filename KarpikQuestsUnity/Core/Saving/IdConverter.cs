using System.ComponentModel;
using System.Globalization;
using NewKarpikQuests.ID;

namespace NewKarpikQuests.Saving
{
    public class IdConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string to)
            {
                return new Id(to[4..]);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}