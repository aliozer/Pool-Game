using AO;
using AO.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PoolGame
{
    public abstract class UIState
    {
        public abstract void Handle(UIManager manager);
    }

    public class InitializeState : UIState
    {
        public List<BasePlayerData> Players { get; }

        public InitializeState(List<BasePlayerData> players)
        {
            Players = players;
            Players.Clear();
        }


        public override void Handle(UIManager manager)
        {
            manager.UIInteractionPanel.gameObject.SetActive(false);
            manager.LastGamePanel.gameObject.SetActive(true);
            manager.GamePopupButton.gameObject.SetActive(false);
            manager.VolumePopupButton.gameObject.SetActive(false);
            manager.GameToggleMenu.gameObject.SetActive(true);
            manager.VolumeToggleMenu.gameObject.SetActive(false);
            manager.QuitButton.gameObject.SetActive(false);
            manager.CameraButton.gameObject.SetActive(false);
            manager.ReplayButton.gameObject.SetActive(false);
            manager.Music.gameObject.SetActive(true);
            manager.PowerBar.gameObject.SetActive(false);

            foreach (var item in manager.UIPlayers)
            {
                item.PlayerName.gameObject.SetActive(false);
                item.Point.gameObject.SetActive(false);
            }

            manager.PlayersPanel.gameObject.SetActive(false);
            manager.GameOverPanel.gameObject.SetActive(false);
        }
    }

    public class PlayingState : UIState
    {
        public override void Handle(UIManager manager)
        {
            manager.UIInteractionPanel.gameObject.SetActive(false);
            manager.LastGamePanel.gameObject.SetActive(false);
            manager.GameToggleMenu.gameObject.SetActive(false);
            manager.VolumeToggleMenu.gameObject.SetActive(true);
            manager.CameraButton.gameObject.SetActive(true);
            manager.CameraButton.interactable = true;
            manager.PowerBar.gameObject.SetActive(true);
            manager.Music.gameObject.SetActive(false);
            manager.QuitButton.gameObject.SetActive(true);
            manager.PlayersPanel.gameObject.SetActive(true);
            manager.ReplayButton.gameObject.SetActive(true);
            manager.ReplayButton.interactable = true;
        }
    }

    public class CompletedState : UIState
    {
        public CompletedState(BasePlayerData player)
        {
            Player = player;
        }

        public BasePlayerData Player { get; }

        public override void Handle(UIManager manager)
        {
            manager.WinPlayerNameText.text = Player.Name;
            manager.GameOverPanel.gameObject.SetActive(true);
        }
    }

    public class ReplayState : UIState
    {

        public override void Handle(UIManager manager)
        {
            manager.CameraButton.interactable = false;
        }
    }

    public class RoundPlayingState : UIState
    {

        public override void Handle(UIManager manager)
        {
            manager.ReplayButton.interactable = false;
            manager.CameraButton.interactable = true;
        }
    }

    [Serializable]
    public class UIPlayer
    {

        [SerializeField]
        private Text playerName;
        public Text PlayerName => playerName;

        [SerializeField]
        private Text point;
        public Text Point => point;
    }

    public class UIManager : BaseMonoBehaviour
    {

        public event Action CameraClick;
        public event Action ReplayClick;
        public event Action CloseClick;
        public event Action QuitClick;
        public event Action RetryClick;
        public event Action<bool> MusicValueChanged;
        public event Action<GameMode> GameModeClick;
        public event Action<float> VolumeChanged;
        public event Action PowerApply;
        public event Action<float> PowerChanged;

        [SerializeField]
        private Image uIInteractionPanel;
        public Image UIInteractionPanel => uIInteractionPanel;

        [SerializeField]
        private Toggle gameToggleMenu;
        public Toggle GameToggleMenu => gameToggleMenu;

        [SerializeField]
        private Toggle volumeToggleMenu;
        public Toggle VolumeToggleMenu => volumeToggleMenu;

        [SerializeField]
        private Toggle music;
        public Toggle Music => music;

        [SerializeField]
        private Button cameraButton;
        public Button CameraButton => cameraButton;

        [SerializeField]
        private Button gamePopupButton;
        public Button GamePopupButton => gamePopupButton;

        [SerializeField]
        private Button volumePopupButton;
        public Button VolumePopupButton => volumePopupButton;

        [SerializeField]
        private Button threeCushionsStartButton;
        public Button ThreeCushionsStartButton => threeCushionsStartButton;

        [SerializeField]
        private Button threeBallStartButton;
        public Button ThreeBallStartButton => threeBallStartButton;

        [SerializeField]
        private Button fourBallStartButton;
        public Button FourBallStartButton => fourBallStartButton;

        [SerializeField]
        private Button quitButton;
        public Button QuitButton => quitButton;

        [SerializeField]
        private Slider volumeSlider;
        public Slider VolumeSlider => volumeSlider;

        [SerializeField]
        private PowerBar powerBar;
        public PowerBar PowerBar => powerBar;

        [SerializeField]
        private Image playersPanel;
        public Image PlayersPanel => playersPanel;

        [SerializeField]
        private UIPlayer[] uiPlayers;
        public UIPlayer[] UIPlayers => uiPlayers;

        [SerializeField]
        private Text timer;
        public Text Timer => timer;

        [SerializeField]
        private Image gameOverPanel;
        public Image GameOverPanel => gameOverPanel;

        [SerializeField]
        private Button closeButton;
        public Button CloseButton => closeButton;

        [SerializeField]
        private Button retryButton;
        public Button RetryButton => retryButton;

        [SerializeField]
        private Button replayButton;
        public Button ReplayButton => replayButton;

        [SerializeField]
        private Text winPlayerNameText;
        public Text WinPlayerNameText => winPlayerNameText;

        [SerializeField]
        private Text lastGameText;
        public Text LastGameText => lastGameText;

        [SerializeField]
        private Image lastGamePanel;
        public Image LastGamePanel => lastGamePanel;


        private UIState state;

        public UIState State {
            get { return state; }
            set {
                state = value;

                state.Handle(this);
            }
        }

        public LastGameData LastGameData { get; private set; }

        private List<BasePlayerData> Players;

        public UIManager()
        {
            Players = new List<BasePlayerData>();
        }

        private void OnEnable()
        {
            CameraButton.onClick.AddListener(delegate { OnCameraButtonClick(); });
            ReplayButton.onClick.AddListener(delegate { OnReplayButtonClick(); });
            QuitButton.onClick.AddListener(delegate { OnQuitButtonClick(); });
            RetryButton.onClick.AddListener(delegate { OnRetryButtonClick(); });
            ThreeCushionsStartButton.onClick.AddListener(delegate { OnGameModeButtonClick(GameMode.ThreeCushions); });
            ThreeBallStartButton.onClick.AddListener(delegate { OnGameModeButtonClick(GameMode.ThreeBall); });
            FourBallStartButton.onClick.AddListener(delegate { OnGameModeButtonClick(GameMode.FourBall); });
            VolumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
            PowerBar.onPowerApply.AddListener(delegate { OnPowerApply(); });
            PowerBar.onValueChanged.AddListener(delegate { OnPowerChanged(); });
            Music.onValueChanged.AddListener(delegate { OnMusicValueChanged(); });
            CloseButton.onClick.AddListener(delegate { OnCloseButtonClick(); });

            GamePopupButton.onClick.AddListener(delegate { OnGamePopupButtonClick(); });
            VolumePopupButton.onClick.AddListener(delegate { OnVolumePopupButtonClick(); });

            GameToggleMenu.onValueChanged.AddListener(delegate { OnGameToggleMenuValueChanged(); });
            VolumeToggleMenu.onValueChanged.AddListener(delegate { OnVolumeToggleMenuValueChanged(); });

            Initialized();
        }

        private void OnCameraButtonClick()
        {
            CameraClick?.Invoke();
        }

        private void OnReplayButtonClick()
        {
            ReplayClick?.Invoke();
        }

        private void OnQuitButtonClick()
        {
            QuitClick?.Invoke();
        }

        private void OnRetryButtonClick()
        {
            RetryClick?.Invoke();
        }

        private void OnMusicValueChanged()
        {
            MusicValueChanged?.Invoke(Music.isOn);
        }

        private void OnGameModeButtonClick(GameMode mode)
        {
            SetActivateGamePopup(false);
            GameModeClick?.Invoke(mode);
        }

        private void OnGamePopupButtonClick()
        {
            SetActivateGamePopup(false);
        }

        private void OnVolumeChanged()
        {
            VolumeChanged?.Invoke(VolumeSlider.value);
        }

        private void OnPowerChanged()
        {
            PowerChanged?.Invoke(PowerBar.Value);
        }

        private void OnPowerApply()
        {
            PowerApply?.Invoke();
        }

        private void OnGameToggleMenuValueChanged()
        {
            if (GamePopupButton.IsActive())
                SetActivateGamePopup(false);
            else
                SetActivateGamePopup(true);
        }

        private void OnVolumeToggleMenuValueChanged()
        {
            if (VolumePopupButton.IsActive())
                SetActivateValumePopup(false);
            else
                SetActivateValumePopup(true);
        }

        private void SetActivateGamePopup(bool isActive)
        {
            GamePopupButton.gameObject.SetActive(isActive);

            if (State is InitializeState)
            {
                Music.gameObject.SetActive(!isActive);
            }
        }

        private void SetActivateValumePopup(bool isActive)
        {
            VolumePopupButton.gameObject.SetActive(isActive);

        }

        private void OnVolumePopupButtonClick()
        {
            SetActivateValumePopup(false);
        }


        private void OnCloseButtonClick()
        {
            CloseClick?.Invoke();
        }

        public void SetLastGameData(LastGameData lastGameData)
        {
            LastGameData = lastGameData;
            lastGameData.PropertyChanged += LastGameData_PropertyChanged;
            SetLastGameText();
        }

        private void SetLastGameText()
        {
            LastGameText.text = $"Score: {LastGameData.Score}, Number Of Strokes: {LastGameData.NumberOfStrokes}, Time: {LastGameData.Time}";
        }

        private void LastGameData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetLastGameText();
        }

        public void AddPlayer(BasePlayerData player)
        {
            Players.Add(player);
            player.PropertyChanged += Player_PropertyChanged;

            UIPlayers[Players.Count - 1].PlayerName.gameObject.SetActive(true);
            UIPlayers[Players.Count - 1].PlayerName.text = player.Name;

            UIPlayers[Players.Count - 1].Point.gameObject.SetActive(true);
            UIPlayers[Players.Count - 1].Point.text = "0";
        }

        private void Player_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BasePlayerData.Point))
            {
                BasePlayerData player = sender as BasePlayerData;

                int index = Players.IndexOf(player);
                int point = int.Parse(UIPlayers[index].Point.text);
                UIPlayers[index].Point.text = (point + player.Point).ToString();
            }
        }

        public void Initialized()
        {
            State = new InitializeState(Players);
        }

        public void GamePlayingState()
        {
            State = new PlayingState();
        }

        public void RoundPlayingState()
        {
            State = new RoundPlayingState();
        }

        public void ReplayState()
        {
            State = new ReplayState();
        }

        public void GameCompletedState(BasePlayerData player)
        {
            State = new CompletedState(player);
        }

        public void OnMouseDown()
        {
            UIInteractionPanel.gameObject.SetActive(true);
        }

        public void OnMouseUp()
        {
            UIInteractionPanel.gameObject.SetActive(false);

        }
    }
}

