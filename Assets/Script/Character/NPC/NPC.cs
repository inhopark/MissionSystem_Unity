using UnityEngine;
using static NPCDefine;
using static MissionDefine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCUnique _npcUnique = NPCUnique.None;

    [SerializeField]
    private MissionUnique _missionUnique = MissionUnique.None;

    public NPCUnique GetNPCUnique()
    {
        return _npcUnique;
    }

    public MissionUnique GetMissionUnique()
    {
        return _missionUnique;
    }

}