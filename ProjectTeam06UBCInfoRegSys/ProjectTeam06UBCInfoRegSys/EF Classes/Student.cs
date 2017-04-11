using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeam06UBCInfoRegSys.EF_Classes
{
    /// <summary>
    /// Represents any student at UBC.
    /// </summary>
    [Table("Students")]
    public class Student
    {
        //properties aka table columns
        [DisplayName("Student ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StudentId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Student First Name")]
        public string StudentFirstName { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Student Last Name")]
        public string StudentLastName { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Student Address")]
        public string StudentAddress { get; set; }

        //student belongs to one city
        [ForeignKey("City")]
        [DisplayName("City ID")]
        public int CityId { get; set; }

        public City City { get; set; }

        //student belongs to one department
        [ForeignKey("Department")]
        [DisplayName("Department ID")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        //student has many registrations
        public virtual ICollection<Registration> Registrations { get; set; }

        public Student()
        {
            Registrations = new HashSet<Registration>();
        }
    }
}