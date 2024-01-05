using System.ComponentModel.DataAnnotations;

namespace Anas_DB_Tr2.Models
{
	public class Appointment
	{
		[Key]
		public int App_ID { get; set; }
		public DateTime App_Date { get; set; }
		// ... other properties that match the columns in your appointment table ...

		public int Doctor_ID { get; set; }
		public Doctor Doctor { get; set; }

		// If there's a relationship between Appointment and Patient
		public int Patient_ID { get; set; }

		public Patient Patient { get; set; }

        public TimeSpan App_Time { get; set; }
    }
}