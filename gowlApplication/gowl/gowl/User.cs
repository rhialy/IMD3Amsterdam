using System;
using SQLite;

namespace GOWL
{
	public class User
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Interests { get; set; }

		public string Standards { get; set; }

		public string Activities { get; set; }

		public override string ToString()
		{
			return string.Format("[User: ID={0}, FirstName={1}, LastName={2}, Interesets={3}, Standards={4}, Activities={5}]", 
								 ID, FirstName, LastName, Interests, Standards, Activities);
		}
	}
}

