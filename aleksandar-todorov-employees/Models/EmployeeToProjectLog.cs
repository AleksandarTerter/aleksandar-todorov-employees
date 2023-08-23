using aleksandar_todorov_employees.Services;
using CsvHelper.TypeConversion;
using System.ComponentModel;

namespace aleksandar_todorov_employees.Models
{
    public class EmployeeToProjectLog
    {
        public uint EmpID { get; set; }
        public uint ProjectID { get; set; }
        [TypeConverter(typeof(DateOnlyStringConverter))]
        public DateOnly DateFrom { get; set; }
        [TypeConverter(typeof(DateOnlyStringConverter))]
        public DateOnly DateTo { get; set; }
    }
}