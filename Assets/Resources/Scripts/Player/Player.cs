using AO;
using AO.Extensions;
using AO.RecordSystem;
using AO.Utilities;
using PoolGame.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PoolGame
{

    public class Player : RecordingObject
    {
        public event Action<Player> ShotStarted;
        public event Action<Player> BallHit;
        public event Action<Player> DirectionChanged;
        public int TotalPoint { get; protected set; }
        public int LastPoint { get; protected set; }

        private BaseBall ball;

        public BaseBall Ball {
            get { return ball; }
            set {
                ball = value;
                OnBallChanged();
            }
        }

        public PlayerDataContext DataContext { get; private set; }
        public BaseCue Cue { get; protected set; }
        public BaseDirectionRule DirectionRule = new ZeroDirectionRule();
        public BaseDirectionRule SpinRule { get; set; } = new ZeroDirectionRule();


        private float power;

        public float Power {
            get { return power; }
            set {
                power = value;
                OnPowerChanged();
            }
        }


        public const float CUE_PULL_SPEED = 8.0f;
        public const float DISTANCE_CUE_FROM_BALL = 0.5f;
        public const float FORCE_FACTOR = 500.0f;

        private Vector3 OFFSET = new Vector3(0.0f, 1.0f, -9.0f);

        protected GameObject OffsetGO { get; set; }

        public Vector3 Offset {
            get {
                return OffsetGO.transform.position;
            }
        }

        public bool IsOrder { get; private set; }

        public void SetDataContext(PlayerDataContext dataContext)
        {
            if (DataContext == null)
            {
                DataContext = dataContext;
            }
            else
            {
                Debug.LogWarning("'DataContext' cannot be changed.");
            }
        }

        public void Shot()
        {
            if (Power != 0)
            {
                OnShotStarted(this);

                SetCueIsTrigger(false);

                Vector3 FirstCuePosition = Ball.transform.position + (Cue.Direction.normalized * (Ball.Radius + DISTANCE_CUE_FROM_BALL));

                System.Collections.Hashtable ht = new System.Collections.Hashtable();

                ht.Add("position", FirstCuePosition + Cue.Direction.normalized * 0.3f);
                ht.Add("easeType", iTween.EaseType.easeInBack);
                ht.Add("time", 0.5f);
                ht.Add("oncompletetarget", gameObject);
                ht.Add("oncomplete", "OnShotCompleted");


                iTween.MoveTo(Cue.gameObject, ht);

                Cue.DirectionLineCalculator.SetEnabled(false);
            }
        }

        private void SetCueIsTrigger(bool value)
        {
            Cue.GetComponent<CapsuleCollider>().isTrigger = value;
        }

        private void OnShotCompleted()
        {
            Wait();
        }

        protected virtual void OnBallHit(Player player)
        {
            BallHit?.Invoke(player);
        }

        protected virtual void OnShotStarted(Player player)
        {
            ShotStarted?.Invoke(player);
        }

        protected virtual void OnPowerChanged()
        {

            Vector3 FirstCuePosition = Ball.transform.position - (Cue.Direction.normalized * (Ball.Radius + DISTANCE_CUE_FROM_BALL));
            Cue.transform.position = Vector3.MoveTowards(Cue.transform.position, FirstCuePosition - Cue.Direction.normalized * Power, Power);

        }

        public void AddPoint(int point)
        {
            DataContext.Point = point;
            LastPoint = point;
            TotalPoint += point;
        }

        public static Player Create(PlayerDataContext dataContext)
        {
            GameObject go = new GameObject(dataContext.UserName);
            Player player = go.AddComponent<Player>();
            player.SetDataContext(dataContext);

            player.OffsetGO = new GameObject("offset");
            player.OffsetGO.transform.parent = player.transform;
            player.OffsetGO.transform.localPosition = player.OFFSET;

            GameObject cue = Instantiate(PrefabUtil.Create<GameObject>(dataContext.CuePrefabName, "cue"));
            cue.transform.parent = player.transform;
            player.Cue = cue.transform.Find("Spin").Find("Cue").GetComponent<BaseCue>();
            player.Cue.TriggerStay += player.Cue_TriggerStay;

            return player;
        }

        private  void Cue_TriggerStay(Collider obj)
        {
            Cue.Parent.localRotation = Quaternion.Euler(Cue.Parent.localRotation.eulerAngles.x + Time.deltaTime * 25.0f, 0.0f, 0.0f);

        }
        

        protected virtual void OnBallChanged()
        {
            Ball.Hit += Ball_Hit;

            Cue.DirectionLineCalculator.Projection = PrefabUtil.Create<GameObject>("Projection", "Ball");
        }


        private void Ball_Hit(GameObject sender, Collision collision)
        {
            // ıstakayı aynı açıya geri getir.
            Cue.Parent.localRotation = Quaternion.identity;

            if (collision.gameObject.tag.Equals(Tags.Cue.GetStringValue()))
            {
                OnBallHit(this);
                Ball.Rigidbody.AddForce(Cue.Direction.normalized * Power * FORCE_FACTOR);

            }
        }

        public void Wait()
        {
            Passive();
            IsOrder = false;
            Cue.DirectionLineCalculator.SetEnabled(false);
        }

        public void Prepare()
        {


            Active();
            IsOrder = true;

            SetCueIsTrigger(true);

            // burada ıstakanın posizyonu ayarlanıyor
            UpdateCueTransform();

            Cue.DirectionLineCalculator.DirectionObject = Cue;
            Cue.DirectionLineCalculator.StartingPosition = Ball.transform.position;
            Cue.DirectionLineCalculator.SetEnabled(true);
            
        }


        private void UpdateCueTransform()
        {
            transform.position = Ball.transform.position;
            Cue.transform.position = Ball.transform.position + (Offset - Ball.transform.position).normalized * (Ball.Radius + DISTANCE_CUE_FROM_BALL);
            Cue.transform.localRotation = Quaternion.FromToRotation(Vector3.up, transform.InverseTransformPoint(Offset));

        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

          
            if (!UIUtil.IsMouseOverUI() && DirectionRule.Execute())
            {
                DirectionChanged?.Invoke(this);
            }

            if (!UIUtil.IsMouseOverUI() && SpinRule.Execute())
            {

            }


        }
        

        public override void OnReplayStarted()
        {
            //Cue.DirectionLineCalculator.SetEnabled(false);
        }

        public override void OnReplayStopped()
        {
            //Cue.DirectionLineCalculator.SetEnabled(true);
        }

        public override BaseReplayData GetReplayData()
        {
            return new PlayerReplayData(this);
        }
    }
}
