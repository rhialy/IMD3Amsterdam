using System;
using SQLite;

namespace GOWL
{
	public class User
	{
		[PrimaryKey]
		public int ID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Standards { get; set; }

		public int IsActive { get; set; }

		public int InterestNature { get; set; }

		public int InterestCity { get; set; }

		public int InterestCulture { get; set; }

		public int InterestSportActivities { get; set; }

		public int InterestEvents { get; set; }

		public int Persons { get; set; }

		public string StartingDate { get; set; }

		public string EndDate { get; set; }

		public bool ExistingJourney { get; set; }

		public override string ToString()
		{
			return string.Format("[User: ID={0}, FirstName={1}, LastName={2}, Standards={4}, IsActive={5}, InterestNature={6}, InterestCity={7}, InterestCulture={8}, InterestSportActivites={9}, InterestEvents={10}, Persons={11}, StartingDate={12}, EndDate={13}, ExistingJourney={14}]", 
				ID, FirstName, LastName, Standards, IsActive, InterestNature, InterestCity, InterestCulture, InterestSportActivities, InterestEvents, Persons, StartingDate, EndDate, ExistingJourney);
		}
	}
}

