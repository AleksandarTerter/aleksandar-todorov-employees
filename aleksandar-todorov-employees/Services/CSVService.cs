using CsvHelper;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace aleksandar_todorov_employees.Services
{
    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<T>();
        }

        public IEnumerable<T> ReadCSV<T>(Stream file, IDictionary<Type, ITypeConverter> typeConverters)
        {
            var reader = new StreamReader(file);

            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            foreach (var converter in typeConverters)
            {
                csv.Context.TypeConverterCache.AddConverter(converter.Key, converter.Value);
            }

            return csv.GetRecords<T>();
        }
    }

}