using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class Pupil
    {
        [Key]
        public int PupilId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(50)]
        public string Class { get; set; }

       
        public virtual ICollection<Teacher> Teachers { get; set; }

        public Pupil()
        {
            Teachers = new HashSet<Teacher>();
        }
    }
}