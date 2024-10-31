using System.ComponentModel.DataAnnotations.Schema;

namespace KahaTiev.Data.DTOs
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public List<Result> StudentResults { get; set; }

    }

    public class Result
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
        public char Grade { get; set; }
    }
}
