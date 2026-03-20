using UnityEngine;

public class UserManager : SingletonMonoBehaviour<UserManager>
{
    private Player _player = null;

    public override void Initialize()
    {
        base.Initialize();

        // 케릭터 프리팹 생성.
        GameObject playerPrefab= Resources.Load<GameObject>("Character/Player");
        if(playerPrefab != null)
        {
            GameObject playerInstance = GameObject.Instantiate(playerPrefab, new Vector3(0f, 0f, -4f), Quaternion.identity);
            if(playerInstance != null)
            {
                playerInstance.TryGetComponent<Player>(out Player player);
                if(player != null)
                {
                    _player = player;
                }
            }
        }
    }

    public Player GetPlayer()
    {
        return _player;
    }


}