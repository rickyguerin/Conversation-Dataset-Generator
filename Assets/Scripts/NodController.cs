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

        // CoRoutine flags
        private bool recording;
        private bool pollingInteraction;
        private bool pollingResponse;

        // Flag that polling sets to start an interaction
        private bool beginInteraction;

        // Probability that an interaction will begin on any given second
        private float interactRate = 0.5f;

        // Probability that the listener will respond to the speaker
        private float responseRate = 0.5f;

        // Probability that the speaker will change after an interaction
        private float changeSpeakerRate = 0.5f;

        // Number of seconds of silence before polling
        private float silenceDuration;

        // Index into nodders to determine who is the active speaker
        private int speaker;

        // Amount of time listener has to respond to speaker
        private float responseWindow;

        void Start()
        {
                Reset();
        }

        // Re-initialize values to prepare to record a new video
        private void Reset()
        {
                recording = false;
                pollingInteraction = false;
                pollingResponse = false;
                beginInteraction = false;
                silenceDuration = 2.0f;

                speaker = Random.Range(0, 2);
                state = ConversationState.SILENCE;
                interactRate = NodSettings.InteractionRate(eggLevel);
                responseRate = NodSettings.ResponseRate(eggLevel);
                changeSpeakerRate = NodSettings.ChangeSpeakerChance(eggLevel);

                foreach (Nodder n in nodders)
                {
                        n.SnapToZero();
                        n.SetSeeds(NodSettings.Seed(), NodSettings.Seed());
                }
        }

        void Update()
        {
                // End program when there are no videos to record
                if (numVideos == 0) { Application.Quit(); }

                // If there are videos to record, begin 
                else if (!recording && NoSpeakers())
                {
                        Reset();
                        StartCoroutine("RecordVideo");
                }

                // Someone is actively talking 
                if (state == ConversationState.TALKING)
                {
                        if (NoSpeakers() && responseWindow <= 0.0f)
                        {
                                responseWindow = 0.0f;
                                silenceDuration = NodSettings.Silence();
                                state = ConversationState.SILENCE;
                        }

                        else
                        {
                                responseWindow -= Time.deltaTime;

                                if (!pollingResponse && responseWindow > 0)
                                {
                                        StartCoroutine("PollForResponse");
                                }
                        }
                }

                // After someone talks, there is an opportunity to respond
                else if (state == ConversationState.RESPONDING)
                {

                }

                // Between interactions, there is some amount of silence
                else if (state == ConversationState.SILENCE)
                {
                        if (silenceDuration <= 0.0f)
                        {
                                silenceDuration = 0.0f;
                                state = ConversationState.POLLING;
                        }

                        else
                        {
                                silenceDuration -= Time.deltaTime;
                        }
                }

                // After silence, we poll for more interactions
                else if (state == ConversationState.POLLING)
                {
                        if (beginInteraction)
                        {
                                beginInteraction = false;

                                // Change speakers?
                                if ((Random.Range(0.0f, 1.0f) < changeSpeakerRate))
                                {
                                        speaker = (speaker + 1) % 2;
                                }

                                float speakTime = NodSettings.SecondsToTalk();
                                responseWindow = speakTime + 3.0f;
                                nodders[speaker].AddTalkTime(speakTime);
                                state = ConversationState.TALKING;
                        }

                        else if (!pollingInteraction)
                        {
                                StartCoroutine("PollForInteraction");
                        }
                }
        }

        // After waiting a second, determine if an interaction should begin
        private IEnumerator PollForInteraction()
        {
                pollingInteraction = true;

                yield return new WaitForSeconds(1);

                beginInteraction = (Random.Range(0.0f, 1.0f) < interactRate);

                pollingInteraction = false;
        }

        // After waiting a second, determine if the listener responds
        private IEnumerator PollForResponse()
        {
                pollingResponse = true;

                yield return new WaitForSeconds(1);

                if (Random.Range(0.0f, 1.0f) < responseRate)
                {
                        nodders[(speaker + 1) % 2].AddTalkTime(NodSettings.SecondsToRespond());
                        responseWindow = 0.0f;
                }

                pollingResponse = false;
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

        // Determine if anyone is currently speaking/responding
        private bool NoSpeakers()
        {
                foreach (Nodder n in nodders)
                {
                        if (n.IsTalking()) return false;
                }

                return true;
        }
}
