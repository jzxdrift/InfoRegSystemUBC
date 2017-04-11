using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeam06UBCInfoRegSys.EF_Classes
{
    /// <summary>
    /// Represents any department at UBC.
    /// </summary>
    [Table("Departments")]
    public class Department
    {
        //properties aka table columns
        [DisplayName("Department ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(5)]
        [DisplayName("Department Name")]
        public string DepartmentName { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Department Description")]
        public string DepartmentDescription { get; set; }

        //department has many courses
        public virtual ICollection<Course> Courses { get; set; }

        //department has many students
        public virtual ICollection<Student> Students { get; set; }

        //department has many instructors
        public virtual ICollection<Instructor> Instructors { get; set; }

        public Department()
        {
            Courses = new HashSet<Course>();
            Students = new HashSet<Student>();
            Instructors = new HashSet<Instructor>();
        }
    }
}