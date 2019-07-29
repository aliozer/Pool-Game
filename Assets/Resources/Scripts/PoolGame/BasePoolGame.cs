using AO;
using AO.Extensions;
using AO.RecordSystem;
using PoolGame.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    [Serializable]
    public class BallDataContext
    {
        [SerializeField]
        private BaseBall ballPrefab;
        public BaseBall BallPrefab => ballPrefab;

        [SerializeField]
        private Transform transform;
        public Transform Transform => transform;
    }

    public abstract class BasePoolGame : RecordingObject
    {
        public event Action<Player> ShotCompleted;
        public event Action<Player> ChangePlayer;
        public event Action<Player> Completed;
        public event Action<Player> Started;

        [SerializeField]
        private BallDataContext[] BallDataContexts;

        [SerializeField]
        private BasePoolTable PoolTablePrefab;

        public BasePoolTable Table { get; private set; }
        public List<BaseBall> Balls { get; private set; }
        public bool IsRoundPlaying { get; protected set; }
        public bool IsPlaying { get; private set; } = false;

        public Player Player { get; protected set; }
        public Player PreviousPlayer { get; protected set; }
        
        protected HashSet<GameObject> HitObjectList { get; set; }
        protected List<Player> Players { get; set; }
        protected List<BaseFinishingRule> FinishingRuleList { get; set; }

        public BasePoolGame()
        {
            FinishingRuleList = new List<BaseFinishingRule>();
            Players = new List<Player>();
            HitObjectList = new HashSet<GameObject>();
            BallDataContexts = new BallDataContext[GetBallCount()];
            Balls = new List<BaseBall>();
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            if (PoolTablePrefab == null)
                throw new Exception("PoolTablePrefab cannot be null.");

            for (int i = 0; i < BallDataContexts.Length; i++)
                if (BallDataContexts[i] == null)
                    throw new Exception("'BallDataContexts' all must be filled");


            CreatePoolTable();
            CreatePoolBalls();


        }

        private void CreatePoolBalls()
        {
            for (int i = 0; i < BallDataContexts.Length; i++)
            {
                if (BallDataContexts[i].BallPrefab == null)
                    throw new Exception("'BallPrefab' cannot be null.");

                if (BallDataContexts[i].Transform == null)
                    throw new Exception("'Transform' cannot be null.");

                BaseBall ball = Instantiate<BaseBall>(BallDataContexts[i].BallPrefab, BallDataContexts[i].Transform.position, BallDataContexts[i].Transform.rotation);
                ball.transform.parent = transform;


                Balls.Add(ball);
            }

        }

        private void Player_BallHit(Player value)
        {
            IsRoundPlaying = true;
        }

        private void PayerBall_Hit(GameObject sender, Collision collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Wall.GetStringValue()) || collision.gameObject.tag.Equals(Tags.Ball.GetStringValue()))
            {
                Debug.Log(collision.gameObject);
                HitObjectList.Add(collision.gameObject);
            }

        }


        protected virtual void CreatePoolTable()
        {
            Table = Instantiate<BasePoolTable>(PoolTablePrefab, transform);
        }

        protected virtual void OnValidate()
        {
            if (BallDataContexts.Length != GetBallCount())
            {
                Debug.LogWarning("Don't change the 'BallDataContexts' field's array size!");
                Array.Resize(ref BallDataContexts, GetBallCount());
            }

        }

        private bool IsBallMoving()
        {
            for (int i = 0; i < Balls.Count; i++)
            {
                if (Balls[i].GetComponent<Rigidbody>().velocity.magnitude > 0.0001f)
                    return true;

            }

            return false;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (IsRoundPlaying)
            {
                if (!IsBallMoving())
                {
                    IsRoundPlaying = false;

                    int point = GetPointRule().Execute();

                    HitObjectList.Clear();

                    Player.AddPoint(point);

                    if (IsGameCompleted())
                    {
                        OnCompleted(Player);
                    }
                    else
                    {
                        OnShotCompleted(Player);

                        if (point == 0)
                        {
                            PlayerWait();

                            Player = GetNextPlayer();
                            OnChangePlayer(Player);
                        }

                        PreviousPlayer = Player;
                        PlayerPrepare();

                    }

                }
            }
        }

        protected virtual void OnShotCompleted(Player player)
        {
            ShotCompleted?.Invoke(player);
        }

        protected virtual void OnChangePlayer(Player player)
        {

            ChangePlayer?.Invoke(player);
        }

        protected virtual void OnCompleted(Player player)
        {
            Completed?.Invoke(player);
        }

        protected virtual void OnStarted(Player player)
        {
            Started?.Invoke(player);
        }


        protected Player GetNextPlayer()
        {
            Player player = null;

            if (Player == null)
            {
                player = Players[0];
            }
            else
            {
                for (int i = 0; i < Players.Count; i++)
                {
                    if (Players[i].Equals(Player))
                    {
                        player = Players[i + 1 == Players.Count ? 0 : i + 1];
                        break;
                    }
                }
            }

            return player;

        }

        private bool IsGameCompleted()
        {
            bool result = true;

            foreach (var rule in FinishingRuleList)
            {
                result &= rule.Execute();
            }

            return result;
        }

        public Player[] GetPlayers()
        {
            return Players.ToArray();
        }

        public virtual void AddPlayer(PlayerDataContext playerDataContext)
        {
            if (Players.Count == GetMaxPlayerCount())
            {
                throw new Exception("The maximum number of players has been reached.");
            }

            Players.Add(GetPlayer(playerDataContext));

            Player player = Players.Last();
            player.Ball = GetPlayerBall();
        }


        private Player GetPlayer(PlayerDataContext playerDataContext)
        {
            Player player = Player.Create(playerDataContext);
            player.transform.parent = transform;
            player.Wait();

            return player;

        }

        public virtual void AddFinishingRule(BaseFinishingRule rule)
        {
            FinishingRuleList.Add(rule);
        }

        public virtual void Play()
        {
            if (Players.Count == 0)
                throw new Exception("Player must be added.");

            if (FinishingRuleList.Count == 0)
                throw new Exception("FinishingRuleList must be added.");

            IsPlaying = true;

            // ilk oyuncu ile oyuna başla
            Player = GetNextPlayer();
            PlayerPrepare();

            OnStarted(Player);
            
        }

        private void PlayerPrepare()
        {
            Player.Prepare();
            Player.Ball.Hit += PayerBall_Hit;
            Player.BallHit += Player_BallHit;
        }

        private void PlayerWait()
        {
            Player.Wait();
            Player.Ball.Hit -= PayerBall_Hit;
            Player.BallHit -= Player_BallHit;
        }

        public void Stop()
        {
            IsPlaying = false;
            transform.Clear();
        }


        public override BaseReplayData GetReplayData()
        {
            return new PoolGameReplayData(this);
        }

        public override void OnReplayStarted()
        {
            PreviousPlayer.OnReplayStarted();
            Player.Wait();

            for (int i = 0; i < Balls.Count; i++)
            {
                Balls[i].OnReplayStarted();
            }

        }

        public override void OnReplayStopped()
        {
            if (PreviousPlayer != null)
            {
                PreviousPlayer.OnReplayStopped(); 
            }

            Player.Prepare();

            for (int i = 0; i < Balls.Count; i++)
            {
                Balls[i].OnReplayStopped();
            }
            
        }



        public abstract int GetBallCount();
        public abstract BasePointRule GetPointRule();
        public abstract BaseBall GetPlayerBall();
        public abstract int GetMaxPlayerCount();
        public abstract GameMode GetMode();

    }
}
