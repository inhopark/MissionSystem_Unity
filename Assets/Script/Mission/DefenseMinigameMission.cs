namespace MissionSystem
{
    /// Template Method: owns and drives a DefenseMinigameController through the mission's own state hooks.
    public abstract class DefenseMinigameMission : BaseMission
    {
        private DefenseMinigameController _defenseMinigame;

        public DefenseMinigameController GetOrCreateDefenseMinigameController()
        {
            if (_defenseMinigame == null)
            {
                _defenseMinigame = new DefenseMinigameController();
                _defenseMinigame.OnSurvived += HandleMinigameSurvived;
            }

            return _defenseMinigame;
        }

        protected override void EnterInProgressState()
        {
            base.EnterInProgressState();
            GetOrCreateDefenseMinigameController().Start(MissionPlayer, MissionManager.Instance);
        }

        protected override void EnterSucceededState()
        {
            base.EnterSucceededState();
            _defenseMinigame?.Stop();
        }

        protected override void EnterFailedState()
        {
            base.EnterFailedState();
            _defenseMinigame?.Stop();
        }

        private void HandleMinigameSurvived()
        {
            SuccessMission();
        }

        public override float GetMinigameRemainingTime() => _defenseMinigame?.GetRemainingTime() ?? -1f;
        public override float GetMinigameTotalDuration() => _defenseMinigame?.SurvivalDuration ?? 0f;
    }
}
