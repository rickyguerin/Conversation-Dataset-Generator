using UnityEngine;

public class HeadBob : MonoBehaviour
{
        // Track how long this head has been nodding
        private float timer = 0;

        // Time in seconds to complete one nod
        private float nodTime = 1;

        // Number of nods to do
        private int nodsRemaining = 5;

        // How far up to nod
        private readonly int MIN_NOD_ANGLE = -15;

        // How far down to nod
        private readonly int MAX_NOD_ANGLE = 15;

        // Update is called once per frame
        void Update()
        {
                transform.Rotate(Mathf.Cos(Mathf.PI * Time.time) * 20, 0, 0);
        }
}
