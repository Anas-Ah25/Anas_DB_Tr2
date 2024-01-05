using System.ComponentModel.DataAnnotations;

namespace Anas_DB_Tr2.Models
{
	public class MedicalRecord
	{
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

	}
}
