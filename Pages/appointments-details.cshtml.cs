using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Anas_DB_Tr2.Models;

namespace Anas_DB_Tr2.Pages
{
	public class AppointmentDetailsModel : PageModel
	{
		private readonly string _connectionString =
			"Data Source=DESKTOP-0S0IFEA;Initial Catalog=ANAS_TR2;Integrated Security=True;TrustServerCertificate=True;";

		public Appointment AppointmentDetails { get; private set; }
		public Patient PatientDetails { get; private set; }
		public MedicalReport ExistingMedicalReport { get; private set; }

		[BindProperty]
		public MedicalReport InputMedicalReport { get; set; }

		public int appointmentId = 30;

		public async Task<IActionResult> OnGetAsync()
		{
			await LoadAppointmentDetailsAsync(appointmentId); // appointmentId is already set to 30
			return Page();
		}
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await SubmitMedicalReportAsync();
            return RedirectToPage(new { appointmentId = InputMedicalReport.Appointment_ID });
        }


        private async Task LoadAppointmentDetailsAsync(int appointmentId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				var command = new SqlCommand(
					"SELECT a.App_ID, a.App_Date, a.App_Time, a.Doc_ID, a.Pat_ID, " +
					"p.Patient_ID, p.Patient_Name, p.PhoneN, p.Email, p.Birthdate, " +
					"m.DateTime, m.Description, m.Conclusion " +
					"FROM appointment a " +
					"INNER JOIN patient p ON a.Pat_ID = p.Patient_ID " +
					"LEFT JOIN MedicalReport m ON a.App_ID = m.Appointment_ID " +
					"WHERE a.App_ID = @AppointmentId", connection);

				command.Parameters.AddWithValue("@AppointmentId", appointmentId);

				using (var reader = await command.ExecuteReaderAsync())
				{
					if (await reader.ReadAsync())
					{
						AppointmentDetails = new Appointment
						{
							// Assuming these properties exist in your Appointment model.
							App_ID = reader.GetInt32(reader.GetOrdinal("App_ID")),
							App_Date = reader.GetDateTime(reader.GetOrdinal("App_Date")),
							App_Time = reader.GetTimeSpan(reader.GetOrdinal("App_Time")),
							Doctor_ID = reader.GetInt32(reader.GetOrdinal("Doc_ID")),
							Patient_ID = reader.GetInt32(reader.GetOrdinal("Pat_ID")),
						};

						// Calculate the age based on the Birthdate.
						var birthdate = reader.GetDateTime(reader.GetOrdinal("Birthdate"));
						var age = DateTime.Today.Year - birthdate.Year;
						if (birthdate.Date > DateTime.Today.AddYears(-age)) age--;

						PatientDetails = new Patient
						{
							// Assuming these properties exist in your Patient model.
							Patient_ID = reader.GetInt32(reader.GetOrdinal("Patient_ID")),
							Patient_Name = reader.GetString(reader.GetOrdinal("Patient_Name")),
							PhoneN = reader.GetInt32(reader.GetOrdinal("PhoneN")), 
							Email = reader.GetString(reader.GetOrdinal("Email")),
							Birthdate = birthdate,
							Age = age, 
						};

						// Check if there is an existing medical report.
						if (!reader.IsDBNull(reader.GetOrdinal("DateTime")))
						{
							ExistingMedicalReport = new MedicalReport
							{
								// Assuming these properties exist in your MedicalReport model.
								Appointment_ID = appointmentId,
								DateTime = reader.GetDateTime(reader.GetOrdinal("DateTime")),
								Description = reader.GetString(reader.GetOrdinal("Description")),
								Conclusion = reader.IsDBNull(reader.GetOrdinal("Conclusion")) ? null : reader.GetString(reader.GetOrdinal("Conclusion")),
							};
						}
					}
				}
			}
		}


		private async Task SubmitMedicalReportAsync()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				var command = new SqlCommand(
					"INSERT INTO MedicalReport (Appointment_ID, Dr_ID, DateTime, Description, Conclusion) " +
					"VALUES (@Appointment_ID, @Dr_ID, @DateTime, @Description, @Conclusion)", connection);

				command.Parameters.AddWithValue("@Appointment_ID", InputMedicalReport.Appointment_ID);
				command.Parameters.AddWithValue("@Dr_ID", InputMedicalReport.Dr_ID);
				command.Parameters.AddWithValue("@DateTime", DateTime.Now);
				command.Parameters.AddWithValue("@Description", InputMedicalReport.Description);
				command.Parameters.AddWithValue("@Conclusion", DBNull.Value); // Assuming Conclusion is optional

				await command.ExecuteNonQueryAsync();
			}
		}

		

	}
}
