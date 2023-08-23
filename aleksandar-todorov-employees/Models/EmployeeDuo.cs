namespace aleksandar_todorov_employees.Models
{
    public class EmployeeDuo
    {
        public uint EmpAID { get; private set; }
        public uint EmpBID { get; private set; }

        public EmployeeDuo(uint id1, uint id2)
        {
            if (id1 == id2)
                throw new ArgumentException("Invalid duo ids.");

            EmpAID = Math.Max(id1, id2);
            EmpBID = Math.Min(id1, id2);
        }

        public override bool Equals(object obj) => Equals(obj as EmployeeDuo);

        public bool Equals(EmployeeDuo other) => other != null && EmpAID == other.EmpAID && EmpBID == other.EmpBID;

        public static bool operator ==(EmployeeDuo obj1, EmployeeDuo obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (obj1 is null || obj2 is null)
                return false;

            return obj1.Equals(obj2);
        }

        public static bool operator !=(EmployeeDuo obj1, EmployeeDuo obj2) => !(obj1 == obj2);

        public override int GetHashCode() => HashCode.Combine(EmpAID, EmpBID);
    }
}