
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.RecordSystem
{
    public class ReplayItem
    {
        public RecordingObject RecordingObject { get; }

        public List<BaseReplayData> Frames { get; private set; }

        public float ReplayTimescale { get; set; } = 1.0f;

        public bool IsReplaying { get; private set; }

        private float ReplayTime = 0.0f;
        private int FrameIndex = 0;

        public ReplayItem(RecordingObject recordingObject)
        {
            RecordingObject = recordingObject;
            Frames = new List<BaseReplayData>();
        }

        public bool IsFinished()
        {
            return !(FrameIndex < Frames.Count);
        }

        public void Reset()
        {
            FrameIndex = 0;
            ReplayTime = 0.0f;
        }

        public void Last()
        {
            FrameIndex = Frames.Count - 1;
            Frames[FrameIndex].Execute();
        }

        public void Next()
        {
            if (FrameIndex < Frames.Count)
            {
                var frame = Frames[FrameIndex];

                if (ReplayTime < frame.Time)
                {
                    if (FrameIndex == 0)
                    {
                        ReplayTime = frame.Time;
                        FrameIndex++;
                    }
                    else
                    {
                        frame.Execute();
                        ReplayTime += Time.smoothDeltaTime * 1000 * ReplayTimescale;
                    }
                }
                else
                {
                    FrameIndex++;
                }
            }

        }




    }

    public class Recorder : BaseMonoBehaviour
    {
        public event Action ReplayStopped;
        public event Action ReplayStarted;

        [SerializeField]
        private List<RecordingObject> recordingObjects;
        public List<RecordingObject> RecordingObjects => recordingObjects;

        [SerializeField]
        private float replayTimescale = 1.0f;

        public float ReplayTimescale {
            get { return replayTimescale; }
            set {
                replayTimescale = value;

                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].ReplayTimescale = replayTimescale;
                }
            }
        }


        private List<ReplayItem> Items;

        public bool IsRecording { get; private set; }
        public bool IsReplaying { get; private set; }

        private float RecordTime = 0.0f;

        public Recorder()
        {
            recordingObjects = new List<RecordingObject>();
            Items = GetInitialItems();
        }

        public void StartRecord()
        {
            if (RecordingObjects.Count == 0)
                return;

            RecordTime = 0.0f;

            Items = GetInitialItems();
            IsRecording = true;
        }

        public void StopRecord()
        {
            if (RecordingObjects.Count == 0)
                return;

            IsRecording = false;
        }

        public void StartReplay()
        {
            if (RecordingObjects.Count == 0 || Items.Count == 0)
                return;

            if (Items[0].Frames.Count == 0)
                return;

            StopRecord();
            SetRecordingObjectsIsReplaying(true);

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].RecordingObject.OnReplayStarted();
                Items[i].Reset();
            }

            IsReplaying = true;

            OnReplayStarted();

        }

        public void StopReplay()
        {
            if (RecordingObjects.Count == 0)
                return;

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].RecordingObject.OnReplayStopped();
                
            }

            SetRecordingObjectsIsReplaying(false);
            IsReplaying = false;

            OnReplayStopped();
        }
        protected virtual void OnReplayStarted()
        {
            ReplayStarted?.Invoke();
        }
        protected virtual void OnReplayStopped()
        {
            ReplayStopped?.Invoke();
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (IsRecording)
            {
                RecordFrame();
            }

            if (IsReplaying)
            {
                if (!IsAllReplayItemsFinished())
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        Items[i].Next();
                    }
                }
                else
                {
                    StopReplay();
                }
            }
        }

        private bool IsAllReplayItemsFinished()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (!Items[i].IsFinished())
                    return false;
            }

            return true;
        }

        private void RecordFrame()
        {
            
            for (int i = 0; i < Items.Count; i++)
            {
                RecordTime += Time.smoothDeltaTime * 1000;

                BaseReplayData replayData = Items[i].RecordingObject.GetReplayData();
                replayData.Time = RecordTime;

                Items[i].Frames.Add(replayData);
            }
        }

        private List<ReplayItem> GetInitialItems()
        {
            var items = new List<ReplayItem>();

            for (int i = 0; i < RecordingObjects.Count; i++)
            {
                items.Add(new ReplayItem(RecordingObjects[i])
                {
                    ReplayTimescale = ReplayTimescale
                });
            }

            return items;
        }

        public void Stop()
        {
            if (IsRecording)
            {
                StopRecord();
            }

            if (IsReplaying)
            {
                StopReplay();
            }


            Clear();
        }

        public void Clear()
        {
            RecordingObjects.Clear();
        }

        private void SetRecordingObjectsIsReplaying(bool value)
        {
            for (int i = 0; i < RecordingObjects.Count; i++)
            {
                RecordingObjects[i].IsReplaying = value;
            }
        }
    }
}
