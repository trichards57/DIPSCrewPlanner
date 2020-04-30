namespace DIPSCrewPlanner.Model
{
    internal class Person
    {
        public DipsContext DipsContext { get; set; }
        public int DipsId { get; set; }

        public string DisplayName
        {
            get
            {
                if (UnitName.Contains("Employee") || UnitName.Contains("Ambulance Services"))
                    return $"{FirstName} {LastName} (Employee)";
                else
                    return $"{FirstName} {LastName}";
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
    }
}
