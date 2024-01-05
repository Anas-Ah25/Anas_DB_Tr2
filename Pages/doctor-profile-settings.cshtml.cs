using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Anas_DB_Tr2.Models;

namespace Anas_DB_Tr2.Pages
{
	public class DoctorProfileSettingsModel : PageModel
	{
		private readonly string _connectionString = "Data Source=DESKTOP-0S0IFEA;Initial Catalog=ANAS_TR2;Integrated Security=True;TrustServerCertificate=True;";

		[BindProperty]
		public Doctor CurrentDoctor { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			int doctorId = 1; // hardcoded 
			await LoadDoctorAsync(doctorId);
			return Page();
		}

		private async Task LoadDoctorAsync(int doctorId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand("SELECT Doctor_ID, Doc_Name, Doc_Specialization FROM doctor WHERE Doctor_ID = @DoctorId", connection);
				command.Parameters.AddWithValue("@DoctorId", doctorId); //example

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

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			await UpdateDoctorProfileAsync(CurrentDoctor);
			return RedirectToPage();
		}

		private async Task UpdateDoctorProfileAsync(Doctor doctor)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var command = new SqlCommand("UPDATE doctor SET Doc_Name = @Doc_Name, Email = @Email, Biography = @Biography WHERE Doctor_ID = @DoctorId", connection);
				command.Parameters.AddWithValue("@DoctorId", doctor.Doctor_ID);
				command.Parameters.AddWithValue("@Doc_Name", doctor.Doc_Name);
				command.Parameters.AddWithValue("@Email", doctor.Email);
				command.Parameters.AddWithValue("@Biography", doctor.Biography);
				
				await command.ExecuteNonQueryAsync();
			}
		}
	}
}
