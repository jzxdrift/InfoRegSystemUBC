using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeam06UBCInfoRegSys.EF_Classes
{
    /// <summary>
    /// Represents any instructor at UBC.
    /// </summary>
    [Table("Instructors")]
    public class Instructor
    {
        //properties aka table columns
        [DisplayName("Instructor ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InstructorId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Instructor First Name")]
        public string InstructorFirstName { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Instructor Last Name")]
        public string InstructorLastName { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Instructor Address")]
        public string InstructorAddress { get; set; }

        //instructor belongs to one city
        [ForeignKey("City")]
        [DisplayName("City ID")]
        public int CityId { get; set; }

        public City City { get; set; }

        //instructor belongs to one department
        [ForeignKey("Department")]
        [DisplayName("Department ID")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        //instructor has many sections
        public virtual ICollection<Section> Sections { get; set; }

        public Instructor()
        {
            Sections = new HashSet<Section>();
        }
    }
}