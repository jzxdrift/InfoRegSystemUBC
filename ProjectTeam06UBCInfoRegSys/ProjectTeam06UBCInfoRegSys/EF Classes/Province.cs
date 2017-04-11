using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeam06UBCInfoRegSys.EF_Classes
{
    /// <summary>
    /// Represents any province in Canada.
    /// </summary>
    [Table("Provinces")]
    public class Province
    {
        //properties aka table columns
        [DisplayName("Province ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProvinceId { get; set; }

        [Required]
        [StringLength(5)]
        [DisplayName("Province Name")]
        public string ProvinceName { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Province Description")]
        public string ProvinceDescription { get; set; }

        //province has many cities
        public virtual ICollection<City> Cities { get; set; }

        public Province()
        {
            Cities = new HashSet<City>();
        }
    }
}