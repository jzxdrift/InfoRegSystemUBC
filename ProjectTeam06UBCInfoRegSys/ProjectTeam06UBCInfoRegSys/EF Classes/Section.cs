using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeam06UBCInfoRegSys.EF_Classes
{
    /// <summary>
    /// Represents any section of particular course at UBC.
    /// </summary>
    [Table("Sections")]
    public class Section
    {
        //properties aka table columns
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Section ID")]
        public int SectionId { get; set; }

        //section belongs to one course
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Course")]
        [DisplayName("Course ID")]
        public int CourseId { get; set; }

        public Course Course { get; set; }

        //section belongs to one department
        [Key]
        [Column(Order = 2)]
        [ForeignKey("Course")]
        [DisplayName("Department ID")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        //section belongs to one instructor
        [ForeignKey("Instructor")]
        [DisplayName("Instructor ID")]
        public int InstructorId { get; set; }

        public Instructor Instructor { get; set; }

        //section has many registrations
        public virtual ICollection<Registration> Registrations { get; set; }

        public Section()
        {
            Registrations = new HashSet<Registration>();
        }
    }
}