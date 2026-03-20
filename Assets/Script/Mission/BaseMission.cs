using UnityEngine;
using static MissionDefine;
using static UIDefine;

public class BaseMission
{
    protected MissionInfoTable _missionInfoTable = null;
    protected MissionState _currentMissionState = MissionState.None;

    protected MissionResult _missionResult = MissionResult.None;

    protected MainUI _mainUI = null;

    public virtual void Initialize(MissionInfoTable missionInfoTable)
    {
        _missionInfoTable = missionInfoTable;

        // Main UI 많이 사용하므로 맴버 변수로 가지고있자
        _mainUI = UIManager.Instance.GetUI<MainUI>(UIUnique.MainUI);
    }

    public void SetState(MissionState missionState)
    {
        _currentMissionState = missionState;

        switch(_currentMissionState)
        {
            case MissionState.Ready :      Ready();      break;
            case MissionState.InProgress : InProgress(); break;
            case MissionState.Completed :  Completed();  break;
        }
    }

    public MissionUnique GetMissionUnique()
    {
        if(_missionInfoTable == null)
        {
            return MissionUnique.None;
        }

        return _missionInfoTable.missionUnique;
    }

    protected virtual void Ready()
    {
        Debug.LogError("## Mission ## Raady ##");
        ShowNPCDialog();
    }

    public virtual void Agree()
    {
        Debug.LogError("## Mission ## Agree ##");
        SetState(MissionState.InProgress);
    }

    public virtual void Cancel()
    {
        Debug.LogError("## Mission ## Cancel ##");
    }

    protected virtual void InProgress()
    {
        Debug.LogError("## Mission ## InProgress ##");
        if(_mainUI != null)
        {
            _mainUI.SetDialogText(string.Format("미션 진행 중 : {0}", GetMissionUnique()));
            _mainUI.ShowMissionButton(false);
            _mainUI.ShowMissionResultButton(true);
        }
    }

    public virtual void Success()
    {
        Debug.LogError("## Mission ## Success ##");
        _missionResult = MissionResult.Success;
        SetState(MissionState.Completed);
    }

    public virtual void Fail()
    {
        Debug.LogError("## Mission ## Fail ##");
        _missionResult = MissionResult.Fail;
        SetState(MissionState.Completed);
    }

    protected virtual void Completed()
    {
        Debug.LogError("## Mission ## Completed ##");
    }

    protected void ShowNPCDialog()
    {
        if(_mainUI != null && _missionInfoTable != null)
        {
            _mainUI.SetDialogText(_missionInfoTable.dialogText);
            _mainUI.ShowTalkButton(false);
            _mainUI.ShowMissionButton(true);
        }
    }

}
