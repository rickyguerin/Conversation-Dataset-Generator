using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class NodController : MonoBehaviour
{
        public enum ConversationState { SILENCE, POLLING, TALKING };

        private ConversationState state;

        // The EngagementLevel to generate conversations with
        public NodSettings.EngagementLevel eggLevel;

        // All of the heads to nod
        public Nodder[] nodders;

        public GameObject[] dots;

        // CoRoutine flags
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

        // Log directory
        public string logDirectory;

        // Log writer
        private StreamWriter fileWriter;

        // Frame counter for log
        private int frameCount;

        // Current timestamp for log
        private DateTime timestamp;

        void Start()
        {
                timestamp = new DateTime(0);
                frameCount = 0;


                DateTime now = DateTime.Now;

                string filename = now.Year.ToString() + "-";
                filename += now.Month.ToString("D2") + "-";
                filename += now.Day.ToString("D2") + "-";
                filename += now.Hour.ToString("D2") + "-";
                filename += now.Minute.ToString("D2") + "-";
                filename += now.Second.ToString("D2");

                fileWriter = File.CreateText(logDirectory + "\\" + filename + "-LOG.txt");
                Reset();
        }

        // Re-initialize values to prepare to record a new video
        private void Reset()
        {
                pollingInteraction = false;
                pollingResponse = false;
                beginInteraction = false;
                silenceDuration = 2.0f;

                speaker = UnityEngine.Random.Range(0, 2);
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
                                if ((UnityEngine.Random.Range(0.0f, 1.0f) < changeSpeakerRate))
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

                WriteToLogFile();
                timestamp = timestamp.AddSeconds(Time.deltaTime);
        }

        void WriteToLogFile()
        {
                Vector3 L = dots[0].transform.position;
                Vector3 R = dots[1].transform.position;

                string line = timestamp.Minute.ToString("D2") + ":" + timestamp.Second.ToString("D2") + ":" + timestamp.Millisecond.ToString("D3") + ",";
                line += frameCount + ",";
                line += "[" + L.x.ToString("F8") + ":" + L.y.ToString("F8") + ":" + L.z.ToString("F8") + "],[";
                line += R.x.ToString("F8") + ":" + R.y.ToString("F8") + ":" + R.z.ToString("F8") + "]";

                fileWriter.WriteLine(line);

                frameCount++;
        }

        private void OnApplicationQuit()
        {
                fileWriter.Flush();
                fileWriter.Close();
        }

        // After waiting a second, determine if an interaction should begin
        private IEnumerator PollForInteraction()
        {
                pollingInteraction = true;

                yield return new WaitForSeconds(1);

                beginInteraction = (UnityEngine.Random.Range(0.0f, 1.0f) < interactRate);

                pollingInteraction = false;
        }

        // After waiting a second, determine if the listener responds
        private IEnumerator PollForResponse()
        {
                pollingResponse = true;

                yield return new WaitForSeconds(1);

                if (UnityEngine.Random.Range(0.0f, 1.0f) < responseRate)
                {
                        nodders[(speaker + 1) % 2].AddTalkTime(NodSettings.SecondsToRespond());
                        responseWindow = 0.0f;
                }

                pollingResponse = false;
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
