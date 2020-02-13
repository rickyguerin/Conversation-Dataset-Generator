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
		switch (level)
		{
			case EngagementLevel.LOW:
				return rand.Next(0, 3);
			case EngagementLevel.MEDIUM:
				return rand.Next(3, 6);
			case EngagementLevel.HIGH:
				return rand.Next(6, 10);
			default:
				break;
		}

		return 0;
	}
}
