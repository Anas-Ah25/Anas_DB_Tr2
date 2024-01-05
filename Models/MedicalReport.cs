using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anas_DB_Tr2.Models
{
	public class MedicalReport
	{
		[ForeignKey("Appointment")]
		[Key, Column(Order = 0)]
		public int Appointment_ID { get; set; }

		[ForeignKey("Doctor")]
		[Key, Column(Order = 1)]
		public int Dr_ID { get; set; }

		public DateTime DateTime { get; set; }

		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		[DataType(DataType.MultilineText)]
		public string Conclusion { get; set; }

		public virtual Appointment Appointment { get; set; }
		public virtual Doctor Doctor { get; set; }
	}
}