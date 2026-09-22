using System;

namespace MissionSystem
{
    /// State machine: Ready -> InProgress -> Succeeded/Failed. All transitions funnel through SetState.
    public abstract class BaseMission
    {
        public event Action<MissionState> OnMissionStateChanged;

        public MissionState State { get; private set; } = MissionState.Ready;
        protected PlayerCharacter MissionPlayer { get; private set; }

        public void Initialize()
        {
            SetState(MissionState.Ready);
        }

        public virtual void AgreeMission(PlayerCharacter player)
        {
            MissionPlayer = player;
            SetState(MissionState.InProgress);
        }

        public virtual void DisagreeMission()
        {
        }

        public virtual void SuccessMission()
        {
            SetState(MissionState.Succeeded);
        }

        public virtual void FailedMission()
        {
            SetState(MissionState.Failed);
        }

        public virtual float GetMinigameRemainingTime() => -1f;
        public virtual float GetMinigameTotalDuration() => 0f;

        private void SetState(MissionState newState)
        {
            State = newState;

            switch (newState)
            {
                case MissionState.Ready:
                {
                    EnterReadyState();
                    break;
                }
                case MissionState.InProgress:
                {
                    EnterInProgressState();
                    break;
                }
                case MissionState.Succeeded:
                {
                    EnterSucceededState();
                    break;
                }
                case MissionState.Failed:
                {
                    EnterFailedState();
                    break;
                }
            }

            OnMissionStateChanged?.Invoke(newState);
        }

        protected virtual void EnterReadyState() { }
        protected virtual void EnterInProgressState() { }
        protected virtual void EnterSucceededState() { }
        protected virtual void EnterFailedState() { }
    }
}
