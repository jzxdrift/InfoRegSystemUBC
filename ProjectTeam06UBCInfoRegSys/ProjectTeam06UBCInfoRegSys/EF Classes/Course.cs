using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeam06UBCInfoRegSys.EF_Classes
{
    /// <summary>
    /// Represents any course at UBC.
    /// </summary>
    [Table("Courses")]
    public class Course
    {
        //properties aka table columns
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Course ID")]
        public int CourseId { get; set; }

        //course belongs to one department
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Department")]
        [DisplayName("Department ID")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Course Name")]
        public string CourseName { get; set; }

        //course has many sections
        public virtual ICollection<Section> Sections { get; set; }

        public Course()
        {
            Sections = new HashSet<Section>();
        }
    }
}