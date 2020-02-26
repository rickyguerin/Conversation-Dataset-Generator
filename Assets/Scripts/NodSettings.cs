using UnityEngine;

public abstract class NodSettings
{
	// Possible values for level of engagement
	public enum EngagementLevel { LOW, MEDIUM, HIGH }

	// Return a float to indicate the rate at which interactions happen
	public static float InteractionRate(EngagementLevel level)
	{
		switch (level)
		{
			case EngagementLevel.LOW:
				return Random.Range(0.2f, 0.3f);
			case EngagementLevel.MEDIUM:
				return Random.Range(0.4f, 0.5f);
			case EngagementLevel.HIGH:
				return Random.Range(0.7f, 0.8f);
			default:
				break;
		}

		return 0;
	}

	// Return a float to indicate the rate at which the listener responds to the speaker
	public static float ResponseRate(EngagementLevel level)
	{
		switch (level)
		{
			case EngagementLevel.LOW:
				return Random.Range(0.2f, 0.3f);
			case EngagementLevel.MEDIUM:
				return Random.Range(0.4f, 0.5f);
			case EngagementLevel.HIGH:
				return Random.Range(0.7f, 0.8f);
			default:
				break;
		}

		return 0;
	}

	// Return a float to indicate the chance of the speaker changing each interaction
	public static float ChangeSpeakerChance(EngagementLevel level)
	{
		switch (level)
		{
			case EngagementLevel.LOW:
				return Random.Range(0.2f, 0.3f);
			case EngagementLevel.MEDIUM:
				return Random.Range(0.4f, 0.5f);
			case EngagementLevel.HIGH:
				return Random.Range(0.7f, 0.8f);
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

	// Return a duration to respond for
	public static float SecondsToRespond()
	{
		return Random.Range(2.0f, 5.0f);
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

	// Generate a seed number for Nodders to use
	public static float Seed()
	{
		return Random.Range(1.0f, 100000.0f);
	}

	// Return a number of seconds of silence
	public static float Silence()
	{
		return Random.Range(2.0f, 5.0f);
	}
}