using System.ComponentModel.DataAnnotations.Schema;



namespace MultiDatabase.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; } 
        public string? Address { get; set; } 
        public string? Phone { get; set; } 
        public string? Email { get; set; } 
        [NotMapped]
        public IFormFile File { get; set; } 
        public byte[]? FileData { get; set; } 
        public string? FileName { get; set; } 
    }
}

