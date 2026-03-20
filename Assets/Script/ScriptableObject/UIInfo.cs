using System.Collections.Generic;
using UnityEngine;
using static UIDefine;

[CreateAssetMenu(fileName = "UIInfo", menuName = "Scriptable Objects/UIInfo")]
public class UIInfo : ScriptableObject
{
    public List<UIInfoTable> _loadList = new List<UIInfoTable>();
}
