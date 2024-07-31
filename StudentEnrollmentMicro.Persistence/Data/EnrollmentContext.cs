using Microsoft.EntityFrameworkCore;
using StudentEnrollementMicro.Domain.Models;

namespace StudentEnrollementMicro.Persistence.Data;

public class EnrollmentContext : DbContext
{
    public EnrollmentContext(DbContextOptions<EnrollmentContext> options)
        : base(options) { }

    public DbSet<Enrollment> Enrollments { get; set; }
}