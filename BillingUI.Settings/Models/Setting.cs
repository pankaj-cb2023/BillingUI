using System;
using System.Runtime.Serialization;

namespace BillingUI.Settings.Models
{
	[DataContract]
	[Serializable]
	public class Setting
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public int GroupId { get; set; }

		[DataMember]
		public string TypeCode { get; set; }

		[DataMember]
		public string Code { get; set; }

		[DataMember]
		public string Value { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public string CreatedBy { get; set; }

		[DataMember]
		public string ModifiedBy { get; set; }
	}
}
