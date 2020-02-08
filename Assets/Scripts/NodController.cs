using UnityEngine;

public class NodController : MonoBehaviour
{
        public Nodder[] nodders;

        // Start is called before the first frame update
        void Start()
        {
                foreach(Nodder n in nodders)
                {
                        n.SetAngle(Random.Range(5, 10));
                        n.SetSpeed(Random.Range(0.5f, 2.0f));
                }
        }

        // Update is called once per frame
        void Update()
        {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                        nodders[0].AddNods(Random.Range(1, 5));
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                        nodders[1].AddNods(Random.Range(1, 5));
                }
        }
}
