using UnityEngine;
using static UIDefine;

public class Player : MonoBehaviour
{
    // const
    private const float _moveSpeed = 5f;  // 이동 속도 설정
    private Vector3 _offset = new Vector3(0f, 9f,-11f); // 카메라 위치 오프셋
    private Animator _animator = null;

    private void Start()
    {
        _animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }

    private void Update()
    {
        // 입력값 받기 (WASD 또는 방향키)
        float moveX = Input.GetAxis("Horizontal"); // A, D or ←, →
        float moveZ = Input.GetAxis("Vertical");   // W, S or ↑, ↓

        // 이동 방향 벡터 계산
        Vector3 move = new Vector3(moveX, 0f, moveZ);

        // 애니메이션 상태 변경
        if (move.magnitude > 0f)  // 키를 누르고 있을 때
        {
            HandleMovement(move);

            PlayAnimation("Run");
        }
        else  // 아무 키도 안 눌렀을 때
        {
            PlayAnimation("Idle");
        }
    }

    // Player가 Update()에서 움직인 이후에 카메라가 따라오게 해서 부드럽게 동기화됩니다.
    private void LateUpdate()
    {
        // 카메라 위치 세팅
        Camera.main.transform.position = this.transform.position + _offset;

        // 항상 플레이어 바라보기
        Camera.main.transform.LookAt(this.transform);
    }

    private void HandleMovement(Vector3 targetPosition)
    {
        // 이동 방향을 바라보게 회전
        transform.rotation = Quaternion.LookRotation(targetPosition);

        // 실제 이동 처리
        transform.Translate(targetPosition * _moveSpeed * Time.deltaTime, Space.World);
    } 

    private void PlayAnimation(string animationName)
    {
        _animator.Play(animationName);
    }

    // NPC 가 컬리전에 충돌 했을 경우 처리.
    public void OnTriggerEnter(Collider other)
    {
        // 미션 진행중이면 불가.
        if(MissionManager.Instance.IsRequestMission() == false)
        {
            return;
        }

        other.transform.TryGetComponent<NPC>(out NPC npcComponent);
        if(npcComponent != null)
        {
            NPCDefine.NPCUnique npcUnique = npcComponent.GetNPCUnique();

            // Main UI 텍스트 세팅.
            MainUI mainUI = UIManager.Instance.GetUI<MainUI>(UIUnique.MainUI);
            if(mainUI != null)
            {
                mainUI.SetMissionUnique(npcComponent.GetMissionUnique());
                mainUI.SetDialogText(string.Format("{0}와 대화가 가능 합니다.", npcUnique.ToString()));
                mainUI.ShowTalkButton(true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // 미션 진행중이면 불가.
        if(MissionManager.Instance.IsRequestMission() == false)
        {
            return;
        }

        // Main UI 초기화.
        MainUI mainUI = UIManager.Instance.GetUI<MainUI>(UIUnique.MainUI);
        if(mainUI != null)
        {
            mainUI.Initialize();
        }
    }

}
