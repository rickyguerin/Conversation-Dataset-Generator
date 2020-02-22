using UnityEngine;
using System.Collections;

public class NodController : MonoBehaviour
{
        public enum ConversationState { SILENCE, POLLING, TALKING, RESPONDING };

        private ConversationState state;

        // The EngagementLevel to generate conversations with
        public NodSettings.EngagementLevel eggLevel;

        // All of the heads to nod
        public Nodder[] nodders;

        // Number of videos to record
        public int numVideos = 0;

        // Number of seconds to record for each video
        public int videoSeconds = 0;

        // Is capt currently recording?
        private bool recording;

        // Currently waiting for an interaction?
        private bool polling;

        // Flag that polling sets to start an interaction
        private bool beginInteraction;

        // Probability that an interaction will begin on any given second
        private float interactRate = 0.5f;

        void Start()
        {
                Reset();
        }

        private void Reset()
        {
                recording = false;
                polling = false;
                beginInteraction = false;

                state = ConversationState.SILENCE;

                interactRate = NodSettings.InteractionRate(eggLevel);

                foreach (Nodder n in nodders)
                {
                        n.SetSpeed(Random.Range(0.5f, 2.0f));
                        n.SetSeeds(NodSettings.Seed(), NodSettings.Seed());
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

                else if (state == ConversationState.SILENCE)
                {
                        if (!polling)
                        {
                                StartCoroutine("PollForInteraction");
                        }
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

                beginInteraction = (Random.Range(0.0f, 1.0f) < interactRate);

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
