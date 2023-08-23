using CsvHelper.TypeConversion;

namespace aleksandar_todorov_employees.Services
{
    public interface ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file);
        public IEnumerable<T> ReadCSV<T>(Stream file, IDictionary<Type, ITypeConverter> typeConverters);
    }
}