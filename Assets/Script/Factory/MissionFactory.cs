namespace MissionSystem
{
    public class MissionFactory
    {
        public BaseMission CreateMission(MissionUnique missionUnique)
        {
            switch (missionUnique)
            {
                case MissionUnique.Act1Chapter1:
                    return new MissionAct1Chapter1();
                case MissionUnique.Act1Chapter2:
                    return new MissionAct1Chapter2();
                default:
                    return null;
            }
        }
    }
}
