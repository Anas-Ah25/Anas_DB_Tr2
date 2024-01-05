using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Anas_DB_Tr2.Models;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Anas_DB_Tr2.Pages
{
	public class MedicalRecordDetailsModel : PageModel
	{
		private readonly string _connectionString = "Data Source=DESKTOP-0S0IFEA;Initial Catalog=ANAS_TR2;Integrated Security=True;TrustServerCertificate=True;";

		public Patient PatientDetails { get; set; }
		public MedicalRecord MedicalRecordDetails { get; set; }
        public int idd = 30;

		public async Task<IActionResult> OnGetAsync(int idd)
		{
			
			await LoadMedicalRecordDetailsAsync(idd);

			if (PatientDetails == null || MedicalRecordDetails == null)
			{
				return NotFound();
			}

			return Page();
		}


		private async Task LoadMedicalRecordDetailsAsync(int patientId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand(
					"SELECT Description FROM MedicalRecord WHERE Patient_ID = @PatientId", connection);
				command.Parameters.AddWithValue("@PatientId", patientId); //hardcoded data

				try
				{
					await connection.OpenAsync();
					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							MedicalRecordDetails = new MedicalRecord
							{
								Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
							};
						}
					}
				}
				catch (SqlException ex)
				{
					// Handle exceptions
					Console.WriteLine(ex.ToString());
				}
			}
		}

	}
}