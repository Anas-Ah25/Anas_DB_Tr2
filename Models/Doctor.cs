using System.ComponentModel.DataAnnotations;

namespace Anas_DB_Tr2.Models
{
	public class Doctor
	{
		[Key] // This marks Doctor_ID as the primary key
		public int Doctor_ID { get; set; }

		public string Doc_Name { get; set; }
		public string Email { get; set; }
		public string ImageURL { get; set; }
		public string Doc_Specialization { get; set; }
		public string Schedule { get; set; }
		public int? Curr_Clinic { get; set; } // Nullable to represent the FK relationship with SET NULL on delete
		public string Biography { get; set; }



	}
}