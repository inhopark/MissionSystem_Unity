using System.Collections.Generic;
using UnityEngine;
using static UIDefine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    private UIInfo _uiInfo = null;

    private Dictionary<UIUnique, BaseUI> _uiList = new Dictionary<UIUnique, BaseUI>();

    public override void Initialize()
    {
        base.Initialize();

        _uiInfo = Resources.Load<UIInfo>("ScriptableObject/UIInfo");
        if(_uiInfo == null)
        {
            Debug.LogError("## UI Error ## UIInfo is null");
            return;
        }

        // UI 로드
        LoadUI();
    }

    private void LoadUI()
    {
        foreach(UIInfoTable uITable in _uiInfo._loadList)
        {
            GameObject uiPrefab = Resources.Load<GameObject>(uITable.uiPath);
            if(uiPrefab != null)
            {
                GameObject uiInstance = GameObject.Instantiate(uiPrefab);
                if(uiInstance != null)
                {
                    uiInstance.TryGetComponent<BaseUI>(out BaseUI ui);
                    if(ui != null)
                    {
                        AddUIList(uITable.uiUnique, ui);
                    }
                }
            }
        }
    }

    private void AddUIList(UIUnique unique, BaseUI baseUI)
    {
        if(_uiList.ContainsKey(unique) == false)
        {
            _uiList.Add(unique, baseUI);
        }
        else
        {
            Debug.LogError("## UI Error ## areday ui : " + unique);
        }
    }

    public T GetUI<T>(UIUnique unique) where T : BaseUI
    {
         if(_uiList.ContainsKey(unique) == true)
        {
            return _uiList[unique] as T;
        }

        return null;
    }

}