using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Wpm.Clinic.DataAccess;

public class ClinicDbContext(DbContextOptions<ClinicDbContext> options) : DbContext(options)
{
    public DbSet<Consulation> Consulations { get; set; }
}


public record Consulation
    (Guid Id, 
    int PatientId,
    string PatientName,
    int PatientAge,
    DateTime StartTime);


