namespace MissionSystem
{
    public enum MissionUnique
    {
        Act1Chapter1,
        Act1Chapter2
    }

    public enum MissionState
    {
        Ready,
        InProgress,
        Succeeded,
        Failed
    }

    public enum MissionButtonAction
    {
        Left,
        Right
    }

    public static class MissionText
    {
        public const string WaitForMission = "Waiting for Mission";
        public const string MissionInProgress = "Mission in Progress";
        public const string MissionSuccess = "Mission Success";
        public const string MissionFailed = "Mission Failed";
        public const string SuccessButton = "Success";
        public const string FailButton = "Fail";
        public const string AgreeButton = "Agree";
        public const string DisagreeButton = "Disagree";
        public const string ConfirmButton = "Confirm";
    }
}
