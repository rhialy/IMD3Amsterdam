using System;
using SQLite;

namespace GOWL
{
	public class Destination
	{

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string Name { get; set; }

		public string InterestTag { get; set; }

		public string ActivityTag { get; set; }

		public string StandardTag { get; set; }

		public string Time { get; set; }

	}
}

