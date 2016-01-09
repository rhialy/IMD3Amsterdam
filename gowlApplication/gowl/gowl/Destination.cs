using System;
using SQLite;

namespace GOWL
{
	public class Destination
	{

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string Name { get; set; }

		public int Standards { get; set; }

		public int IsActive { get; set; }

		public int InterestNature { get; set; }

		public int InterestCity { get; set; }

		public int InterestCulture { get; set; }

		public int InterestSportActivities { get; set; }

		public int InterestEvents { get; set; }

		public string ImagePath { get; set; }

		public string Description { get; set; }

		public string Time { get; set; }

		public override string ToString()
		{
			return string.Format("[User: ID={0}, Standards={1}, IsActive={2}, InterestNature={3}, InterestCity={4}, InterestCulture={5}, InterestSportActivites={6}, InterestEvents={7}, ImagePath={8}, Description={9}, Time={10}]", 
				ID, Standards, IsActive, InterestNature, InterestCity, InterestCulture, InterestSportActivities, InterestEvents, ImagePath, Description, Time);
		}

	}
}

