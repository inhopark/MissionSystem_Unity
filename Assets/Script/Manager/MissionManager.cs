using UnityEngine;
using static MissionDefine;

public class MissionManager : SingletonMonoBehaviour<MissionManager>
{
    private MissionInfo _missionInfo = null;

    private BaseMission _currentMission = null;

    public override void Initialize()
    {
        base.Initialize();

      _missionInfo = Resources.Load<MissionInfo>("ScriptableObject/MissionInfo");
        if(_missionInfo == null)
        {
            Debug.LogError("## Mission Error ## MissionInfo is null");
            return;
        }
    }

    public bool IsRequestMission()
    {
        // 이미 미션 진행중에는 진행 불가.
        if(_currentMission != null)
        {
            return false;
        }

        return true;
    }

    public void RequestMission(MissionUnique missionUnique)
    {
        if(IsRequestMission() == false)
        {
            return;
        }

        _currentMission = CreateMission(missionUnique);
        if(_currentMission == null)
        {
            Debug.LogError("## Mission Error ## mission is null");
            return;
        }

        _currentMission.SetState(MissionState.Ready);
    }

    private BaseMission CreateMission(MissionUnique missionUnique)
    {
        BaseMission baseMission = null;
        switch(missionUnique)
        {
            case MissionUnique.Mission_NPC1: baseMission = new MissionNPC1(); break;
            case MissionUnique.Mission_NPC2: baseMission = new MissionNPC2(); break;
        }

        if(baseMission != null)
        {
            baseMission.Initialize(GetMissionInfoTable(missionUnique));
        }

        return baseMission;
    }

    private MissionInfoTable GetMissionInfoTable(MissionUnique missionUnique)
    {
        foreach(var missionInfo in _missionInfo._missionInfos)
        {
            if(missionInfo.missionUnique == missionUnique)
            {
                return missionInfo;
            }
        }

        return null;
    }

    public void AgreeMission(MissionUnique missionUnique)
    {
        // 기존 진행 미션 없으면 실패
        if(_currentMission == null)
        {
            return;
        }

        if(_currentMission.GetMissionUnique() != missionUnique)
        {
            return;
        }

        _currentMission.Agree();
    }

    public void CancelMission(MissionUnique missionUnique)
    {
        // 기존 진행 미션 없으면 실패
        if(_currentMission == null)
        {
            return;
        }

        if(_currentMission.GetMissionUnique() != missionUnique)
        {
            return;
        }

        _currentMission.Cancel();

        _currentMission = null;
    }

    public void SuccessMission(MissionUnique missionUnique)
    {
        // 기존 진행 미션 없으면 실패
        if(_currentMission == null)
        {
            return;
        }

        if(_currentMission.GetMissionUnique() != missionUnique)
        {
            return;
        }

        _currentMission.Success();

        _currentMission = null;
    }

    public void FailMission(MissionUnique missionUnique)
    {
        // 기존 진행 미션 없으면 실패
        if(_currentMission == null)
        {
            return;
        }

        // 성공하는 미션과 현재 미션 유니크 같은지 체크.
        if(_currentMission.GetMissionUnique() != missionUnique)
        {
            return;
        }

        _currentMission.Fail();

        _currentMission = null;
    }
}