namespace StudentEnrollementMicro.Domain.Models;

public class Enrollment
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public string CourseName { get; set; }
    public string TeacherName { get; set; }
}