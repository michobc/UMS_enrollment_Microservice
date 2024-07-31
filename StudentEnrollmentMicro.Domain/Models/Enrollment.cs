namespace StudentEnrollementMicro.Domain.Models;

public class Enrollment
{
    public long Id { get; set; }
    public long ClassId { get; set; }
    public long StudentId { get; set; }
    public string StudentName { get; set; }
    public string CourseName { get; set; }
    public string? TeacherName { get; set; }
}