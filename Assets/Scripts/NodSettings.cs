﻿using UnityEngine;

public abstract class NodSettings
{
	// Possible values for level of engagement
	public enum EngagementLevel { LOW, MEDIUM, HIGH }

	// Return a number of nods within an appropriate range for the given engagement level
	public static int NodsToDo(EngagementLevel level)
	{
		switch (level)
		{
			case EngagementLevel.LOW:
				return Random.Range(0, 3);
			case EngagementLevel.MEDIUM:
				return Random.Range(3, 6);
			case EngagementLevel.HIGH:
				return Random.Range(6, 10);
			default:
				break;
		}

		return 0;
	}

	// Return a nod angle within an appropriate range for the given engagement level
	public static float NodAngle(EngagementLevel level)
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