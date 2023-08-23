namespace aleksandar_todorov_employees.Models
{
    public class DuoWorkPerProject
    {
        public EmployeeDuo Duo { get; private set; }
        public uint ProjectID { get; private set; }
        public uint DaysWorked { get; private set; }

        public DuoWorkPerProject(EmployeeDuo duo, uint projectID, uint daysWorked)
        {
            Duo = duo;
            ProjectID = projectID;
            DaysWorked = daysWorked;
        }
    }
}