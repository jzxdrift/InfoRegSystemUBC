using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeam06UBCInfoRegSys.EF_Classes
{
    /// <summary>
    /// Represents any city in Canada.
    /// </summary>
    [Table("Cities")]
    public class City
    {
        //properties aka table columns
        [DisplayName("City ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CityId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("City Name")]
        public string CityName { get; set; }

        //city belongs to one province
        [ForeignKey("Province")]
        [DisplayName("Province ID")]
        public int ProvinceId { get; set; }

        public Province Province { get; set; }

        //city has many students
        public virtual ICollection<Student> Students { get; set; }

        //city has many instructors
        public virtual ICollection<Instructor> Instructors { get; set; }

        public City()
        {
            Students = new HashSet<Student>();
            Instructors = new HashSet<Instructor>();
        }
    }
}