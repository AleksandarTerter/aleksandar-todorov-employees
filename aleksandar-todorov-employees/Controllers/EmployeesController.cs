using aleksandar_todorov_employees.Models;
using aleksandar_todorov_employees.Services;
using CsvHelper.TypeConversion;
using Microsoft.AspNetCore.Mvc;

namespace aleksandar_todorov_employees.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class EmployeesController : ControllerBase
    {
        private readonly ICSVService _csvService;

        public EmployeesController(ICSVService csvService)
        {
            _csvService = csvService;
        }

        [HttpPost("read-employees-csv")]
        public IActionResult GetEmployeeCSV([FromForm] IFormFileCollection file, [FromQuery] string datePattern)
        {
            Stream csv = file[0].OpenReadStream();
            Dictionary<Type, ITypeConverter> typeConverters = new() { { typeof(DateOnly), new DateOnlyStringConverter(datePattern) } };
            List<EmployeeToProjectLog> employeeToProjectLogs = _csvService.ReadCSV<EmployeeToProjectLog>(csv, typeConverters).ToList();

            DuoWorkPerProject[] duosWorkePerProject = employeeToProjectLogs
                .GroupBy(r => r.ProjectID)
                .Select(logs => GetDuoMostWorkedForProject(logs.ToArray()))
                .Aggregate((resA, resB) => resA.Concat(resB))
                .OfType<DuoWorkPerProject>()
                .ToArray();

            DuoWorkPerProject? duoMostExpInAnyProj = duosWorkePerProject
                .OrderByDescending(coop => coop.DaysWorked)
                .FirstOrDefault();

            return duoMostExpInAnyProj != null && duoMostExpInAnyProj.DaysWorked > 0
                ? Ok(duosWorkePerProject.Where(dw => dw.Duo == duoMostExpInAnyProj.Duo))
                : NotFound();
        }

        private static IEnumerable<DuoWorkPerProject> GetDuoMostWorkedForProject(EmployeeToProjectLog[] projectLogs)
        {
            Dictionary<EmployeeDuo, uint> duosWorkForProj = new();
            for (int a = 0; a < projectLogs.Length; a++)
            {
                EmployeeToProjectLog empA = projectLogs[a];
                for (int b = a + 1; b < projectLogs.Length; b++)
                {
                    EmployeeToProjectLog empB = projectLogs[b];
                    if (empA.EmpID == empB.EmpID)
                        continue;

                    uint overlapedDays = GetOverlapedDays(empA.DateFrom, empA.DateTo, empB.DateFrom, empB.DateTo);
                    EmployeeDuo duo = new(empA.EmpID, empB.EmpID);
                    if (duosWorkForProj.ContainsKey(duo))
                    {
                        duosWorkForProj[duo] += overlapedDays;
                    }
                    else
                    {
                        duosWorkForProj.Add(duo, overlapedDays);
                    }
                }
            }

            return duosWorkForProj.Select(w => new DuoWorkPerProject(w.Key, projectLogs[0].ProjectID, w.Value));
        }

        private static uint GetOverlapedDays(DateOnly startA, DateOnly endA, DateOnly startB, DateOnly endB)
        {
            var startInteception = startA > startB ? startA : startB;
            var endInteception = endA < endB ? endA : endB;

            var overlap = endInteception.DayNumber - startInteception.DayNumber;

            return overlap > 0 ? (uint)overlap : 0;
        }
    }
}