using System.ComponentModel.DataAnnotations;

namespace Anas_DB_Tr2.Models
{
	public class Patient
	{
		public int Patient_ID { get; set; }
		public string Patient_Name { get; set; }
		public int PhoneN { get; set; } 

		public string Email { get; set; }
		public DateTime Birthdate { get; set; }
		public int Age { get; set; }
		// Add other properties as per your database structure
	}
}
