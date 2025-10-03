
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(100)]
        public string Subject { get; set; }

       
        public virtual ICollection<Pupil> Pupils { get; set; }

        public Teacher()
        {
            Pupils = new HashSet<Pupil>();
        }
    }
}