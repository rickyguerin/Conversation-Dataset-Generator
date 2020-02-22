using UnityEngine;
using System.Collections;

public class NodController : MonoBehaviour
{
        // All of the heads to nod
        public Nodder[] nodders;

        // Number of videos to record
        public int numVideos = 0;

        // Number of seconds to record for each video
        public int videoSeconds = 0;

        // Is capt currently recording?
        private bool recording;

        void Start()
        {
                recording = false;

                foreach(Nodder n in nodders)
                {
                        n.SetAngle(Random.Range(5, 10));
                        n.SetSpeed(Random.Range(0.5f, 2.0f));
                        n.SetSeeds(Random.Range(1.0f, 100000.0f), Random.Range(1.0f, 100000.0f));
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
                        nodders[0].AddTalkTime(Random.Range(1, 15));
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                        nodders[1].AddTalkTime(Random.Range(1, 15));
                }
        }

        // After waiting a second, determine if an interaction should begin
        private IEnumerator PollForInteraction()
        {
                polling = true;

                yield return new WaitForSeconds(1);

                float f = Random.Range(0.0f, 1.0f);

                if (f < interactRate)
                {
                        beginInteraction = true;
                }

                polling = false;
        }

        // Capture one video of the desired length
        private IEnumerator RecordVideo()
        {
                // Prevents recording a new video until the current recording has finished
                recording = true;

                RockVR.Video.VideoCaptureCtrl.instance.StartCapture();

                yield return new WaitForSeconds(videoSeconds);

                RockVR.Video.VideoCaptureCtrl.instance.StopCapture();

                numVideos--;
                recording = false;
        }
}
