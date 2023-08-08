using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudWithDapper.Models
{
    [Table("StudentInfoTable",Schema ="Master")]
    public class Students 
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RollNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
    }
}
