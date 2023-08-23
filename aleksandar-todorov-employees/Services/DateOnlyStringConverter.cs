using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace aleksandar_todorov_employees.Services
{
    public class DateOnlyStringConverter : DefaultTypeConverter
    {
        private readonly string _dateOnlyFormat;

        public DateOnlyStringConverter() : this("dd/MM/yyyy") { }

        public DateOnlyStringConverter(string dateOnlyFormat)
        {
            _dateOnlyFormat = dateOnlyFormat;
        }

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text) || text.ToLower() == "null")
                return DateOnly.FromDateTime(DateTime.Now);

            if (DateOnly.TryParseExact(text, _dateOnlyFormat, out DateOnly dateObj))
                return dateObj;

            throw new FormatException($"Date: '${text}' can not be parsed to valid date.");
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null) return string.Empty;
            if (DateOnly.TryParse(value.ToString(), out DateOnly dt))
                return dt.ToString();
            else
                return string.Empty;
        }
    }

}