namespace MultiDatabase.Models
{
    public class AddViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HomeAddress { get; set; }
        public string Designation { get; set; }
        public IFormFile File { get; set; }
     
        public string EmployeeSurname { get; set; }
        //public int Salary { get; set; }
    }
}
