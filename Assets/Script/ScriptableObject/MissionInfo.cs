using System.Collections.Generic;
using UnityEngine;
using static MissionDefine;

[CreateAssetMenu(fileName = "MissionInfo", menuName = "Scriptable Objects/MissionInfo")]
public class MissionInfo : ScriptableObject
{
    public List<MissionInfoTable> _missionInfos = new List<MissionInfoTable>();
}
