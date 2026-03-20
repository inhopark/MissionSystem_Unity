public class MissionDefine 
{
    public enum MissionUnique
    {
        None,
        Mission_NPC1,
        Mission_NPC2
    }

    public enum MissionState
    {
        None,
        Ready,
        InProgress,
        Completed,
    }

    public enum MissionResult
    {
        None,
        Success,
        Fail,
    }

    [System.Serializable]
    public class MissionInfoTable
    {
        public MissionUnique missionUnique;

        public string dialogText;
    }
}
