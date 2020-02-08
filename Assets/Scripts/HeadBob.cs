using UnityEngine;

public class HeadBob : MonoBehaviour
{
        public int seed = 0;

        // Update is called once per frame
        void Update()
        {
                transform.Rotate(Mathf.Cos(Mathf.PI * Time.time) * 20, 0, 0);
        }
}
