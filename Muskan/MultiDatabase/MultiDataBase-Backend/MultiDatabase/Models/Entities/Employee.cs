using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiDatabase.Models.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string HomeAddress { get; set; }
        public string Designation { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public byte[]? FileData { get; set; }  
        public string? FileName { get; set; }
        public DateTime DateOfJoin { get; set; } = DateTime.Now;
        public string EmployeeSurname { get; set; }
        //public int Salary { get; set; }
    }

}
