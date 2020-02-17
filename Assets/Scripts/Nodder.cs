using UnityEngine;

public class Nodder : MonoBehaviour
{
        public enum NodState { ACTIVE, RETURN_TO_ZERO, AMBIENT };

        // Robot parts to animate
        public Transform head;
        public Transform neck;

        private Vector3 headTotalRotation;
        private Vector3 neckTotalRotation;

        private readonly Vector3 HEAD_RANGE = new Vector3(15, 5, 5);
        private readonly Vector3 NECK_RANGE = new Vector3(15, 20, 15);

        // Time in seconds to complete one nod
        public float nodSpeed = 0.5f;

        // How far to nod
        public float maxNodAngle = 10;

        // Random seed
        public float headSeed = 0;
        public float neckSeed = 0;

        // Track how long this head has been nodding
        private float timer;        

        // Number of nods to do
        private int nodsRemaining;

        void Start()
        {
                headTotalRotation = new Vector3();
                neckTotalRotation = new Vector3();

                timer = 0.0f;
                nodsRemaining = 0;
        }

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
                float th = Mathf.Lerp(0, 2 * Mathf.PI, timer / nodSpeed);
                float nodAngle = maxNodAngle * Mathf.Sin(th);

                // Rotate head
                transform.rotation = Quaternion.Euler(nodAngle, transform.eulerAngles.y, Mathf.Sin(Time.time) * 2);

                // Increase timer
                timer += Time.deltaTime;

                // When nod is complete, reduce nodsRemaining
                if (timer >= nodSpeed)
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

        public void SetSpeed(float speed)
        {
                nodSpeed = speed;
        }

        public void SetAngle(float angle)
        {
                maxNodAngle = angle;
        }

        public void SetSeeds(float headSeed, float neckSeed)
        {
                this.headSeed = headSeed;
                this.neckSeed = neckSeed;
        }

        public void AddNods(int nods)
        {
                nodsRemaining += nods;
        }

        // Use Perlin noise to generate rotations for each axis
        public static Vector3 GetPerlinRotations(float seed)
        {
                return new Vector3(
                        Mathf.PerlinNoise(seed + Time.time * 2, seed) - 0.5f,
                        Mathf.PerlinNoise(seed - Time.time / 2, seed) - 0.5f,
                        Mathf.PerlinNoise(seed, seed + Time.time / 2) - 0.5f
                );
        }
}
