using System;

public abstract class NodSettings
{
	// Random number generator
	public static Random rand = new Random();

	// Possible values for level of engagement
	public enum EngagementLevel { LOW, MEDIUM, HIGH }

	// Return a number of nods within an appropriate range for the given engagement level
	public static int Nods(EngagementLevel level)
	{
		return level switch
		{
			EngagementLevel.LOW => rand.Next(0, 3),
			EngagementLevel.MEDIUM => rand.Next(3, 6),
			EngagementLevel.HIGH => rand.Next(6, 10),
			_ => 0,
		};
	}
}
