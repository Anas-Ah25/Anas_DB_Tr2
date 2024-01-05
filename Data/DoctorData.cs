using Microsoft.EntityFrameworkCore;
using Anas_DB_Tr2.Models;

namespace Anas_DB_Tr2.Data
{
	public class MyDatabaseContext : DbContext
	{
		public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options)
			: base(options)
		{
		}

		// Rename the DbSet properties to match the convention (pluralized)
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Appointment> Appointments { get; set; } // Ensure the table name in the database is 'Appointments'
		public DbSet<Patient> Patients { get; set; } // Ensure the table name in the database is 'Patients'

		// The 'OnModelCreating' method can be overridden to configure the model further if necessary
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// If your table names do not follow EF Core conventions, you can configure them here
			// If they do follow the conventions (like your 'doctor' table does), this is not needed
			modelBuilder.Entity<Doctor>().ToTable("doctor"); // Only necessary if table names do not follow conventions
			modelBuilder.Entity<Appointment>().ToTable("appointment"); // Same as above
			modelBuilder.Entity<Patient>().ToTable("patient"); // Same as above

			// Add any additional model configuration here
		}
	}
}