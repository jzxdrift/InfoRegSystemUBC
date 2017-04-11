namespace ProjectTeam06UBCInfoRegSys.GridView_Classes
{
    /// <summary>
    /// Represents Course entity in grid view.
    /// </summary>
    public class GridViewCourse
    {
        //properties
        public int CourseId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string CourseName { get; set; }

        public GridViewCourse() { }
    }

    /// <summary>
    /// Represents Registration entity in grid view.
    /// </summary>
    public class GridViewRegistration
    {
        //properties
        public int RegistrationId { get; set; }
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int CourseId { get; set; }
        public int SectionId { get; set; }
        public string CourseName { get; set; }

        public GridViewRegistration() { }
    }

    /// <summary>
    /// Represents Section entity in grid view.
    /// </summary>
    public class GridViewSection
    {
        //properties
        public int SectionId { get; set; }
        public int CourseId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int InstructorId { get; set; }
        public string InstructorFirstName { get; set; }
        public string InstructorLastName { get; set; }

        public GridViewSection() { }
    }
}