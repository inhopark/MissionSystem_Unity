using System;
using UnityEngine;

namespace MissionSystem
{
    /// Facade/Mediator: the single hub NPCs and widgets talk to. Widgets and missions never
    /// reference each other directly - command flow goes down (this calls widget/mission methods),
    /// event flow goes up (mission/widget events, consumed here).
    public class MissionManager : MonoBehaviour, ICoroutineRunner
    {
        public static MissionManager Instance { get; private set; }

        public Transform playerSpawnPoint;

        public event Action<MissionState> OnAnyMissionStateChanged;

        public BaseMission CurrentMission { get; private set; }
        public NPC CurrentMissionNPC { get; private set; }

        private PlayerCharacter _player;
        private readonly MissionFactory _missionFactory = new MissionFactory();

        private MainController _mainController;
        private MainMissionWidget _mainMissionWidget;
        private MissionResultWidget _missionResultWidget;
        private HPWidget _hpWidget;
        private MissionTimerWidget _missionTimerWidget;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterPlayer(PlayerCharacter player)
        {
            _player = player;
            if (_hpWidget != null)
            {
                _hpWidget.Bind(_player);
            }
        }

        public void RegisterWidgets(MainController controller, MainMissionWidget main, MissionResultWidget result, HPWidget hp, MissionTimerWidget timer)
        {
            _mainController = controller;
            _mainMissionWidget = main;
            _missionResultWidget = result;
            _hpWidget = hp;
            _missionTimerWidget = timer;

            _mainMissionWidget.OnMissionButtonAction += HandleMissionButtonAction;
            _missionResultWidget.OnConfirmed += HandleMissionResultConfirmed;

            if (_player != null)
            {
                _hpWidget.Bind(_player);
            }
        }

        /// True when no mission is currently active (i.e. a new mission request can proceed).
        public bool IsMissionRequest() => CurrentMission == null;

        public void ShowMainMissionWidget(MissionUnique missionUnique, NPC npc)
        {
            if (CurrentMission != null && CurrentMission.State == MissionState.InProgress)
                return;

            CurrentMissionNPC = npc;
            SetUICursorMode(true);
            _mainMissionWidget.Show();

            if (IsMissionRequest())
            {
                RequestMission(missionUnique);
            }
        }

        public void HideMainMissionWidget()
        {
            _mainMissionWidget.Hide();
            SetUICursorMode(false);
        }

        public void HandleMissionButtonAction(MissionButtonAction action)
        {
            if (CurrentMission == null || CurrentMission.State != MissionState.Ready)
                return;

            if (action == MissionButtonAction.Left)
            {
                CurrentMissionNPC?.Hide();
                _player.SetPlayMode(PlayMode.Defense);
                CurrentMission.AgreeMission(_player);
                _mainController.SetDefenseHUDVisible(true);
                HideMainMissionWidget();
            }
            else
            {
                CurrentMission.DisagreeMission();
                ClearCurrentMission();
            }
        }

        public void HandlePlayerDefeated()
        {
            CurrentMission?.FailedMission();
        }

        public void HandleMissionResultConfirmed()
        {
            _missionResultWidget.Hide();
            _player.ResetHP();
            ClearCurrentMission();
        }

        public void SetUICursorMode(bool uiMode)
        {
            Cursor.lockState = uiMode ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = uiMode;
            _player?.SetMovementLocked(uiMode);
        }

        public float GetMissionRemainingTime() => CurrentMission?.GetMinigameRemainingTime() ?? -1f;
        public float GetMissionTotalDuration() => CurrentMission?.GetMinigameTotalDuration() ?? 0f;

        private void RequestMission(MissionUnique missionUnique)
        {
            if (IsMissionRequest() == false)
            {
                return;
            }

            CurrentMission = _missionFactory.CreateMission(missionUnique);
            CurrentMission.OnMissionStateChanged += HandleMissionStateChanged;
            CurrentMission.Initialize();
        }

        private void HandleMissionStateChanged(MissionState newState)
        {
            switch (newState)
            {
                case MissionState.Ready:
                case MissionState.InProgress:
                    _mainMissionWidget.SetWidgetState(newState);
                    break;
                case MissionState.Succeeded:
                case MissionState.Failed:
                    _mainController.SetDefenseHUDVisible(false);
                    ShowMissionResultWidget(newState);
                    break;
            }

            OnAnyMissionStateChanged?.Invoke(newState);
        }

        private void ShowMissionResultWidget(MissionState resultState)
        {
            SetUICursorMode(true);
            _missionResultWidget.SetResultState(resultState);
            _missionResultWidget.Show();
        }

        private void ClearCurrentMission()
        {
            if (CurrentMission != null)
            {
                CurrentMission.OnMissionStateChanged -= HandleMissionStateChanged;
                CurrentMission = null;
            }

            if (_player != null)
            {
                if (playerSpawnPoint != null)
                {
                    _player.WarpTo(playerSpawnPoint.position, playerSpawnPoint.rotation);
                }
                _player.SetPlayMode(PlayMode.Normal);
            }

            CurrentMissionNPC?.Show();
            CurrentMissionNPC = null;

            HideMainMissionWidget();
        }
    }
}
