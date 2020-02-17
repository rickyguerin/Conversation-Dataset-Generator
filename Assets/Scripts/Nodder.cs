using UnityEngine;

public class Nodder : MonoBehaviour
{
        public enum NodState { ACTIVE, RETURN_TO_ZERO, AMBIENT };

        private NodState state;

        // Robot parts to animate
        public Transform head;
        public Transform neck;

        // Progress through each body part's Sin wave
        private Vector3 headTheta;
        private Vector3 neckTheta;

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
                state = NodState.AMBIENT;

                headTheta = new Vector3();
                neckTheta = new Vector3();

                timer = 0.0f;
                nodsRemaining = 0;
        }

        void Update()
        {
                AmbientMotion();

                // Only nod if there are nods to do
                if (nodsRemaining == 0) { return; }

                NodMotion();

                // Increase timer
                timer += Time.deltaTime;

                // When nod is complete, reduce nodsRemaining
                if (timer >= nodSpeed)
                {
                        nodsRemaining--;
                        timer = 0;
                }
        }

        // How to move when the Robot is not nodding
        void AmbientMotion()
        {
                headTheta += GetPerlinValues(headSeed) / 100;
                neckTheta += GetPerlinValues(neckSeed) / 100;

                // Set rotations
                UpdateBodyParts();
        }

        // How to move while the Robot is nodding
        void NodMotion()
        {
                headTheta += GetPerlinValues(headSeed) / 5;
                neckTheta += GetPerlinValues(neckSeed) / 5;

                // Set rotations
                UpdateBodyParts();
        }

        // Rotate body parts according to current thetas
        void UpdateBodyParts()
        {
                head.localRotation = Quaternion.Euler(GetRotationFromTheta(headTheta, HEAD_RANGE));
                neck.localRotation = Quaternion.Euler(GetRotationFromTheta(neckTheta, NECK_RANGE));
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

        // Use Perlin noise to generate [-0.5, 0.5] values for each axis
        public static Vector3 GetPerlinValues(float seed)
        {
                return new Vector3(
                        Mathf.PerlinNoise(seed + Time.time, seed) - 0.5f,
                        Mathf.PerlinNoise(seed - Time.time, seed) - 0.5f,
                        Mathf.PerlinNoise(seed, seed + Time.time) - 0.5f
                );
        }

        // Use the theta as input to a Sin wave to generate a rotation
        public static Vector3 GetRotationFromTheta(Vector3 theta, Vector3 range)
        {
                Vector3 rot = new Vector3();

                for (int i = 0; i < 3; i++)
                {
                        float clamp = Mathf.PingPong(theta[i], 360);
                        rot[i] = range[i] * Mathf.Sin(clamp);
                }

                return rot;
        }

        public void ReturnToZero()
        {
                headTheta = Vector3.Lerp(headTheta, Vector3.zero, Time.deltaTime * 2);
                neckTheta = Vector3.Lerp(neckTheta, Vector3.zero, Time.deltaTime * 2);

                UpdateBodyParts();
        }
}
