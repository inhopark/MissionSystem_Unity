using UnityEngine;

public class StartScene : MonoBehaviour
{
    private void Start()
    {
        UserManager.Instance.Initialize();
        CameraManager.Instance.Initialize();
        UIManager.Instance.Initialize();
        MissionManager.Instance.Initialize();
    }
}
