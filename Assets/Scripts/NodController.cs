using UnityEngine;
using System.Collections;

public class NodController : MonoBehaviour
{
        // All of the heads to nod
        public Nodder[] nodders;

        // Number of videos to record
        public int numVideos = 0;

        // Is capt currently recording?
        private bool recording;

        void Start()
        {
                recording = false;

                foreach(Nodder n in nodders)
                {
                        n.SetAngle(Random.Range(5, 10));
                        n.SetSpeed(Random.Range(0.5f, 2.0f));
                }
        }

        void Update()
        {
                if (numVideos == 0)
                {
                        Application.Quit();
                }

                else if (!recording)
                {
                        StartCoroutine("RecordVideo");
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                        nodders[0].AddNods(Random.Range(1, 5));
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                        nodders[1].AddNods(Random.Range(1, 5));
                }
        }

        IEnumerator RecordVideo()
        {
                recording = true;

                RockVR.Video.VideoCaptureCtrl.instance.StartCapture();

                yield return new WaitForSeconds(60);

                RockVR.Video.VideoCaptureCtrl.instance.StopCapture();

                numVideos--;
                recording = false;
        }
}
