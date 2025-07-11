using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.App.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Please, Surname is required!")]
        [StringLength(50, ErrorMessage = "FirstName must be a maximum length 50")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please, FirstName is required!")]
        [StringLength(50, ErrorMessage ="FirstName must be a maximum length 50")]
        public string FirstMidName { get; set; } 
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
