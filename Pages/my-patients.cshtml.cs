using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Anas_DB_Tr2.Models;


namespace Anas_DB_Tr2.Pages
{
    public class MyPatientsModel : PageModel
    {
        private readonly string _connectionString = "Data Source=DESKTOP-0S0IFEA;Initial Catalog=ANAS_TR2;Integrated Security=True;TrustServerCertificate=True;";
        public List<Patient> Patients { get; private set; } = new List<Patient>();
        public Doctor CurrentDoctor { get; private set; }


		public async Task OnGetAsync()
		{
			int currentDoctorId = 1; // Hardcoded exampleeeeeeeeeeeee
			await LoadDoctorAsync(currentDoctorId);
			await LoadPatientsAsync(currentDoctorId);
		}


		// patient data
		private async Task LoadPatientsAsync(int doctorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "SELECT DISTINCT p.Patient_ID, p.Patient_Name, p.PhoneN " +
                    "FROM patient p " +
                    "INNER JOIN appointment a ON p.Patient_ID = a.Pat_ID " +
                    "WHERE a.Doc_ID = @DoctorId", connection);

                command.Parameters.AddWithValue("@DoctorId", doctorId); // doctor ID from the user session 

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var patient = new Patient
                        {
                            Patient_ID = reader.GetInt32(reader.GetOrdinal("Patient_ID")),
                            Patient_Name = reader.GetString(reader.GetOrdinal("Patient_Name")),
                            PhoneN = reader.GetInt32(reader.GetOrdinal("PhoneN")),
                            // ... Add other properties as needed
                        };
                        Patients.Add(patient);
                    }
                }
            }
        }

        // doctor data
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
    }
}