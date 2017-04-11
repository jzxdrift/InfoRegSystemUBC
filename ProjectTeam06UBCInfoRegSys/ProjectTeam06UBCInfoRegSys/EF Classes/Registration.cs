using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeam06UBCInfoRegSys.EF_Classes
{
    /// <summary>
    /// Represents any student registration for any section at UBC.
    /// </summary>
    [Table("Registrations")]
    public class Registration
    {
        //properties aka table columns
        [DisplayName("Registration ID")]
        public int RegistrationId { get; set; }

        //registration belongs to one student
        [ForeignKey("Student")]
        [DisplayName("Student ID")]
        public int StudentId { get; set; }

        public Student Student { get; set; }

        //registration belongs to one section
        [Column(Order = 2)]
        [ForeignKey("Section")]
        [DisplayName("Section ID")]
        public int SectionId { get; set; }

        public Section Section { get; set; }

        //registration belongs to one course
        [Column(Order = 3)]
        [ForeignKey("Section")]
        [DisplayName("Course ID")]
        public int CourseId { get; set; }

        public Course Course { get; set; }

        //registration belongs to one department
        [Column(Order = 4)]
        [ForeignKey("Section")]
        [DisplayName("Department ID")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}