using UnityEngine;
using static UIDefine;

public class BaseUI : MonoBehaviour
{
    protected UIUnique _uiUnique = UIUnique.None;

    protected T BindComponent<T>(Transform root, string name) where T : MonoBehaviour
    {
        if(root == null)
        {
            return null;
        }

        Transform findTransform = root.Find(name);
        if(findTransform == null)
        {
            return null;
        }

        return findTransform.GetComponent<T>();
    }
}
