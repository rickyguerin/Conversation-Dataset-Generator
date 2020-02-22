﻿using UnityEngine;

public abstract class NodSettings
{
	// Possible values for level of engagement
	public enum EngagementLevel { LOW, MEDIUM, HIGH }

	// Return a float 
	public static float InteractionRate(EngagementLevel level)
	{
		switch (level)
		{
			case EngagementLevel.LOW:
				return Random.Range(0.0f, 0.2f);
			case EngagementLevel.MEDIUM:
				return Random.Range(0.4f, 0.6f);
			case EngagementLevel.HIGH:
				return Random.Range(0.8f, 1.0f);
			default:
				break;
		}

		return 0;
	}


	// Return a duration to talk for
	public static float SecondsToTalk()
	{
		return Random.Range(2.0f, 15.0f);
	}

	// Return a nod speed within an appropriate range for the given engagement level
	public static float NodSpeed(EngagementLevel level)
	{
		switch (level)
		{
			case EngagementLevel.LOW:
				return Random.Range(5.0f, 8.0f);
			case EngagementLevel.MEDIUM:
				return Random.Range(8.0f, 10.0f);
			case EngagementLevel.HIGH:
				return Random.Range(10.0f, 12.0f);
			default:
				break;
		}

		return 0.0f;
	}
}