using System;
using System.Collections.Generic;
using AO;
using AO.Extensions;
using AO.Media;
using AO.RecordSystem;
using PoolGame.Rules;
using UnityEngine;

namespace PoolGame
{

    [RequireComponent(typeof(ClockTimer))]
    [RequireComponent(typeof(Recorder))]
    public class GameManager : BaseMonoBehaviour
    {

        [SerializeField]
        private UIManager UIManager;

        [SerializeField]
        private CameraManager CameraManager;

        [SerializeField]
        private Recorder Recorder;

        [SerializeField]
        private GameObject InitialObject;

        public ClockTimer ClockTimer { get; private set; }

        public BasePoolGame Game { get; private set; }


        public const float POWER_FACTOR = 3.0f;


        public GameManager()
        {

        }

        private void OnEnable()
        {
            ClockTimer = GetComponent<ClockTimer>();
            ClockTimer.Tick += ClockTimer_Tick;



            UIManager.CameraClick += UIManager_CameraClick;
            UIManager.ReplayClick += UIManager_ReplayClick;
            UIManager.QuitClick += UIManager_QuitClick;
            UIManager.RetryClick += UIManager_RetryClick;
            UIManager.GameModeClick += UIManager_GameModeClick;
            UIManager.VolumeChanged += UIManager_VolumeChanged;
            UIManager.PowerApply += UIManager_PowerApply;
            UIManager.PowerChanged += UIManager_PowerChanged;
            UIManager.MusicValueChanged += UIManager_MusicValueChanged;
            UIManager.CloseClick += UIManager_CloseClick;


            CameraManager.PositionChanged += CameraManager_PositionChanged;

            Recorder.ReplayStopped += Recorder_ReplayStopped;
            Recorder.ReplayStarted += Recorder_ReplayStarted;

            SetPrefsData();

        }


        private void SetPrefsData()
        {

            UIManager.VolumeSlider.value = Prefs.Settings.Volume;
            SoundManager.instance.Volume = Prefs.Settings.Volume;

            UIManager.Music.isOn = !Prefs.Settings.MuteMusic;
            SoundManager.instance.MuteMusic = Prefs.Settings.MuteMusic;

            UIManager.SetLastGameData(Prefs.Settings.LastGame);
        }

        private void UIManager_CloseClick()
        {
            Stop();
        }

        private void UIManager_MusicValueChanged(bool isOn)
        {
            SoundManager.instance.MuteMusic = !isOn;
            Prefs.Settings.MuteMusic = !isOn;

            Prefs.Save();
        }

        private void UIManager_PowerChanged(float value)
        {
            Game.Player.Power = value * POWER_FACTOR;
        }

        private void UIManager_PowerApply()
        {
            Game.Player.Shot();
        }

        private void UIManager_VolumeChanged(float value)
        {
            SoundManager.instance.Volume = value;
            Prefs.Settings.Volume = value;

            Prefs.Save();
        }

        private void UIManager_GameModeClick(GameMode mode)
        {
            InitialObject.SetActive(false);
            Play(mode);
        }

        private void UIManager_RetryClick()
        {
            Stop();
            InitialObject.SetActive(false);
            Play(Game.GetMode());
        }

        private void UIManager_QuitClick()
        {
            ClockTimer.StopTime();
            
            Stop();
        }

        private void UIManager_ReplayClick()
        {
            if (Recorder.IsReplaying)
            {
                Recorder.StopReplay();
                FocusedCameraState(Game.Player);
            }
            else
            {
                if (Recorder.RecordingObjects.Count > 0)
                {
                    UIManager.ReplayState();
                    CameraManager.ReplayState();
                    Recorder.StartReplay(); 
                }
            }

        }

        private void Recorder_ReplayStarted()
        {
            ClockTimer.Pause();
        }

        private void Recorder_ReplayStopped()
        {
            ClockTimer.Play();

            UIManager.GamePlayingState();
            FocusedCameraState(Game.Player);

            SetPlayersDirectionRule();
        }

        private void UIManager_CameraClick()
        {
            if (CameraManager.State is TournamentCameraState || CameraManager.State is TopCameraState)
            {
                FocusedCameraState(Game.Player);
            }
            else
            {
                CameraManager.TopState();
            }

            SetPlayersDirectionRule();
        }

        private void FocusedCameraState(Player player)
        {
            CameraManager.FocusedState(player.Ball.gameObject, player.gameObject.transform.rotation);

        }


        private void CameraManager_PositionChanged(PoolCamera camera)
        {
            if (CameraManager.State is FocusCameraState)
            {
                Game.Player.transform.position = camera.transform.parent.position;
                Game.Player.transform.rotation = camera.transform.parent.rotation;
            }
        }

        private void ClockTimer_Tick(float hours, float minutes, float seconds)
        {
            UIManager.Timer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }



        private void Play(GameMode mode)
        {
            Game = PoolGameFactory.Create(mode);
            Game.transform.parent = transform;

            // başka oyuncular da eklenebilir.
            var playerData = new PlayerDataContext("aliozer")
            {
                Name = "Ali"
            };

            UIManager.AddPlayer(playerData);

            Game.AddPlayer(playerData);

            playerData = new PlayerDataContext("ezgi")
            {
                Name = "Ezgi",
                CuePrefabName = "BlueCue"
            };

            UIManager.AddPlayer(playerData);
            Game.AddPlayer(playerData);

            foreach (var player in Game.GetPlayers())
            {
                player.BallHit += Player_BallHit;
                player.ShotStarted += Player_ShotStarted;
            }


            Game.AddFinishingRule(new TotalPointFinishingRule(Game.GetPlayers(), 5));
            // aşağıdaki gibi başka bitirme kuralları da eklenebilir.
            // Game.AddFinishingRule(new PlayerFirstMaxPointFinishingRule(CurrentGame.GetPlayers(), 25));

            Game.ShotCompleted += Game_ShotCompleted;
            Game.Completed += Game_Completed;
            Game.Started += Game_Started;
            Game.ChangePlayer += Game_ChangePlayer;


            Game.Play();

            ClockTimer.StartTime();

            UIManager.GamePlayingState();
        }

        private void Player_ShotStarted(Player player)
        {
            FitRecorder();

            Recorder.StartRecord();
        }

        private void FitRecorder()
        {
            Recorder.Clear();

            Recorder.RecordingObjects.Add(Game);
        }


        private void Stop()
        {
            Recorder.Stop();

            Game.Stop();


            UIManager.Initialized();

            InitialObject.SetActive(true);

            CameraManager.Reset();

            transform.Clear();
        }

        private void Player_BallHit(Player value)
        {

            Prefs.Settings.LastGame.NumberOfStrokes++;

            UIManager.RoundPlayingState();

            // camera top değil ise extra turnuva camerasına çevir
            if (!(CameraManager.State is TopCameraState))
                CameraManager.TournamentState();
        }

        private TransformData GetFocusCameraTransformData()
        {
            Vector3 direction = Game.Player.Offset;
            return new TransformData(new Vector3(direction.x, direction.y + 5.0f, direction.z));
        }


        private void Game_Started(Player value)
        {
            FocusedCameraState(Game.Player);

            SetPlayersDirectionRule();
        }
        

        private void SetPlayersDirectionRule()
        {
            foreach (var player in Game.GetPlayers())
            {
                player.DirectionRule = GetDirectionRule();
                player.SpinRule = GetSpinRule(player);
            }
        }

        private BaseDirectionRule GetSpinRule(Player player)
        {
            if (CameraManager.State is FocusCameraState)
            {
                return new FocusCameraSpinRule(CameraManager.State.Camera.main, player.Cue.Spin.transform, player.Ball.Radius - 0.2f);
            }
            else if (CameraManager.State is TopCameraState)
            {
                return new TopCameraSpinRule(CameraManager.State.Camera.main, player.Cue.Spin.transform, player.Ball.Radius - 0.2f);
            }

            return new ZeroDirectionRule();
        }

        private BaseDirectionRule GetDirectionRule()
        {
            // Burayı kameraya bağladığımdan dolayı kapadım.

            //if (CameraManager.State is FocusCameraState)
            //{
            //    return new FocusCameraDirectionRule(Game.Player.transform);
            //}
            if (CameraManager.State is TopCameraState)
            {
                return new TopCameraDirectionRule(CameraManager.State.Camera.main, Game.Player.transform);
            }

            return new ZeroDirectionRule();
        }

        private void Game_ChangePlayer(Player player)
        {
            TournamentCameraChanged();
            Recorder.StopRecord();

            player.DirectionRule = GetDirectionRule();
        }

        private void TournamentCameraChanged()
        {
            if (CameraManager.State is TournamentCameraState || CameraManager.State is FocusCameraState)
                FocusedCameraState(Game.Player);
            else
                CameraManager.TopState();
        }


        private void Game_Completed(Player player)
        {
            // oyun bitti kazanan player

            Prefs.Settings.LastGame.Time = string.Format("{0:0}:{1:00}", ClockTimer.Minutes, ClockTimer.Seconds);
            Prefs.Settings.LastGame.Score = player.TotalPoint;

            Prefs.Save();

            ClockTimer.StopTime();
            UIManager.GameCompletedState(player.DataContext);
        }

        private void Game_ShotCompleted(Player player)
        {
            // player puan almadı ise turnuva kamerasını player değişikliğinde ayarla
            if (player.DataContext.Point != 0)
            {
                Recorder.StopRecord();
                TournamentCameraChanged();
            }


            UIManager.GamePlayingState();
        }



        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (Game != null && Game.IsPlaying)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    UIManager.PowerBar.OnPointerUp(null);
                }

                float power = GetInputPower();
                UIManager.PowerBar.Value += power;
            }


        }

        private float GetInputPower()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                return 0.01f;
            }

            return 0.0f;
        }

    }
}
