using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MissionDefine;
using static UIDefine;

public class MainUI : BaseUI
{
    private TextMeshProUGUI _mainText = null;

    private Button _btnTalk = null;
    private Button _btnMissionA = null;
    private Button _btnMissionC = null;

    private Transform _btnMissionRoot = null;

    private Button _btnMissionS = null;
    private Button _btnMissionF = null;

    private Transform _btnMissionResultRoot = null;

    private MissionUnique _currentMissionUnique = MissionUnique.None;

    private void Awake()
    {
        _uiUnique = UIUnique.MainUI;

        // 메인 텍스트 바인딩.
        _mainText = BindComponent<TextMeshProUGUI>(transform, "MainText");

        // 대화하기 버튼 바인딩.
        _btnTalk = BindComponent<Button>(transform, "BtnTalk");
        if(_btnTalk != null)
        {
            _btnTalk.onClick.AddListener(OnClickTalk);                
        }


        // 미션 수락/거절 UI 바인딩.
        _btnMissionRoot = transform.Find("BtnMissionRoot");
        if(_btnMissionRoot != null)
        {
            _btnMissionA = BindComponent<Button>(_btnMissionRoot, "BtnMissionA");
            if(_btnMissionA != null)
            {
                _btnMissionA.onClick.AddListener(OnClickMissionAgree);                
            }
  
            _btnMissionC = BindComponent<Button>(_btnMissionRoot, "BtnMissionC");
            if(_btnMissionC != null)
            {
                _btnMissionC.onClick.AddListener(OnClickMissionCancel);                
            }
        }

        // 미션 성공/실패 바인딩
        _btnMissionResultRoot = transform.Find("BtnMissionResultRoot");
        if(_btnMissionResultRoot != null)
        {
            _btnMissionS = BindComponent<Button>(_btnMissionResultRoot, "BtnMissionS");
            if(_btnMissionS != null)
            {
                _btnMissionS.onClick.AddListener(OnClickMissionSuccess);                
            }
            _btnMissionF = BindComponent<Button>(_btnMissionResultRoot, "BtnMissionF");
            if(_btnMissionF != null)
            {
                _btnMissionF.onClick.AddListener(OnClickMissionFail);                
            }
        }

        Initialize();
    }

    public void Initialize()
    {
        SetDialogText("NPC 근처로 가세요.");
        ShowTalkButton(false);
        ShowMissionButton(false);
        ShowMissionResultButton(false);
    }

    public void SetMissionUnique(MissionUnique missionUnique)
    {
        _currentMissionUnique = missionUnique;
    }

    public void SetDialogText(string text)
    {
        if(_mainText != null)
        {
            _mainText.text = text;
        }
    }

    public void ShowTalkButton(bool isShow)
    {
        if(_btnTalk != null)
        {
            _btnTalk.gameObject.SetActive(isShow);
        }
    }

    public void ShowMissionButton(bool isShow)
    {
        if(_btnMissionRoot != null)
        {
            _btnMissionRoot.gameObject.SetActive(isShow);
        }
    }

    public void ShowMissionResultButton(bool isShow)
    {
        if(_btnMissionResultRoot != null)
        {
            _btnMissionResultRoot.gameObject.SetActive(isShow);
        }
    }

    private void OnClickTalk()
    {
        MissionManager.Instance.RequestMission(_currentMissionUnique);
    }

    private void OnClickMissionAgree()
    {
        MissionManager.Instance.AgreeMission(_currentMissionUnique);
    }

    private void OnClickMissionCancel()
    {
        MissionManager.Instance.CancelMission(_currentMissionUnique);
        Initialize();
    }

    private void OnClickMissionSuccess()
    {
        MissionManager.Instance.SuccessMission(_currentMissionUnique);
        Initialize();
    }

    private void OnClickMissionFail()
    {
        MissionManager.Instance.FailMission(_currentMissionUnique);
        Initialize();
    }

}
