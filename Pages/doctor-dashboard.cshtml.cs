using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Anas_DB_Tr2.Models;

namespace Anas_DB_Tr2.Pages
{
	public class DoctorDashboardModel : PageModel
	{
		private readonly string _connectionString =
			"Data Source=DESKTOP-0S0IFEA;Initial Catalog=ANAS_TR2;Integrated Security=True;TrustServerCertificate=True;";

		public Doctor CurrentDoctor { get; private set; }
		public List<AppointmentViewModel> Appointments { get; private set; } = new List<AppointmentViewModel>();
		public int TotalPatientsToday { get; private set; }
		public List<AppointmentViewModel> TodaysAppointments { get; private set; } = new List<AppointmentViewModel>();

		// This would normally be DateTime.Today; however, we are hardcoding it for this example
		private  DateTime today = new DateTime(2024, 1, 20);

		public async Task OnGetAsync()
		{
			int currentDoctorId = 1; // Hardcoded exampleeeeeeeeeeeeee
			await LoadDoctorAsync(currentDoctorId);
			await LoadAppointmentsForTodayAsync(currentDoctorId);
			await LoadTotalPatientsForTodayAsync();
		}

		private async Task LoadDoctorAsync(int doctorId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand("SELECT Doctor_ID, Doc_Name, Doc_Specialization FROM doctor WHERE Doctor_ID = @DoctorId", connection);
				command.Parameters.AddWithValue("@DoctorId", doctorId);

				try
				{
					await connection.OpenAsync();
					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							CurrentDoctor = new Doctor
							{
								Doctor_ID = reader.GetInt32(reader.GetOrdinal("Doctor_ID")),
								Doc_Name = reader.GetString(reader.GetOrdinal("Doc_Name")),
								Doc_Specialization = reader.GetString(reader.GetOrdinal("Doc_Specialization")),
								// Add other fields as required.
							};
						}
					}
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		
		}

		private async Task LoadTotalPatientsForTodayAsync()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand(
					"SELECT COUNT(*) FROM appointment WHERE CONVERT(date, App_Date) = @Today", connection);
				command.Parameters.AddWithValue("@Today", today);

				try
				{
					await connection.OpenAsync();
					TotalPatientsToday = (int)await command.ExecuteScalarAsync();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}

		private async Task LoadAppointmentsForTodayAsync(int doctorId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand(
					"SELECT a.App_ID, p.Patient_Name, a.App_Date, p.Patient_ID " +
					"FROM appointment a " +
					"INNER JOIN patient p ON a.Pat_ID = p.Patient_ID " +
					"WHERE a.Doc_ID = @DoctorId AND CONVERT(date, a.App_Date) = @Today", connection);
				command.Parameters.AddWithValue("@DoctorId", doctorId);
				command.Parameters.AddWithValue("@Today", today);

				try
				{
					await connection.OpenAsync();
					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							var appointment = new AppointmentViewModel
							{
								Appointment_ID = reader.GetInt32(reader.GetOrdinal("App_ID")),
								Patient_Name = reader.GetString(reader.GetOrdinal("Patient_Name")),
								App_Date = reader.GetDateTime(reader.GetOrdinal("App_Date")),
								Patient_ID = reader.GetInt32(reader.GetOrdinal("Patient_ID"))
							};
							TodaysAppointments.Add(appointment);
						}
					}
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}

		public class AppointmentViewModel
		{
			public int Appointment_ID { get; set; }
			public string Patient_Name { get; set; }
			public int Patient_ID { get; set; }
			public DateTime App_Date { get; set; }
		}
	}
}
