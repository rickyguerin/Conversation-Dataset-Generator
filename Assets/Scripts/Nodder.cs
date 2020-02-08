using UnityEngine;

public class Nodder : MonoBehaviour
{
        // Track how long this head has been nodding
        private float timer = 0.0f;

        // Time in seconds to complete one nod
        private float nodTime = 0.5f;

        // Number of nods to do
        private int nodsRemaining = 5;

        // How far to nod
        private readonly float MAX_NOD_ANGLE = 10;

        void Update()
        {
                // Make the head float a little
                AmbientMotion();

                // Only nod if there are nods to do
                if (nodsRemaining == 0)
                {
                        return;
                }

                // Determine angle of nod for this frame
                float th = Mathf.Lerp(0, 2 * Mathf.PI, timer / nodTime);
                float nodAngle = MAX_NOD_ANGLE * Mathf.Sin(th);

                // Rotate head
                transform.rotation = Quaternion.Euler(nodAngle, transform.eulerAngles.y, Mathf.Sin(Time.time) * 2);

                // Increase timer
                timer += Time.deltaTime;

                // When nod is complete, reduce nodsRemaining
                if (timer >= nodTime)
                {
                        nodsRemaining--;
                        timer = 0;
                }
        }

        void AmbientMotion()
        {
                transform.Translate(0, Mathf.Sin(Time.time) / 1000, 0);
                transform.rotation = Quaternion.Euler(Mathf.Sin(Time.time) * 2, transform.eulerAngles.y, Mathf.Sin(Time.time) * 2);
        }
}
