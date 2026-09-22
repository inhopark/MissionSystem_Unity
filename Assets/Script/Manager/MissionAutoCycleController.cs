using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MissionSystem
{
    /// Debug/demo tool: cycles NPC-contact -> Agree -> minigame -> result-confirm with no human input.
    /// Only calls pre-existing public MissionManager methods, same ones a human clicking would trigger.
    public class MissionAutoCycleController : MonoBehaviour
    {
        public PlayerCharacter player;

        [SerializeField] private float autoApproachMoveSpeed = 6.5f;
        [SerializeField] private float autoDodgeMoveSpeed = 5f;
        [SerializeField] private float dodgeSwitchDistance = 2.5f;
        [SerializeField] private float autoAgreeDelay = 1.5f;
        [SerializeField] private float autoConfirmDelay = 1.5f;
        [SerializeField] private float autoCycleSurvivalDuration = 5f;

        private bool _active;
        private Coroutine _approachRoutine;

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.f8Key.wasPressedThisFrame)
            {
                ToggleAutoCycleMode();
            }
        }

        public void ToggleAutoCycleMode()
        {
            if (_active)
            {
                if (_approachRoutine != null)
                {
                    StopCoroutine(_approachRoutine);
                    _approachRoutine = null;
                }

                Deactivate();
                return;
            }

            _active = true;
            player.SetAutoControlActive(true);
            MissionManager.Instance.OnAnyMissionStateChanged += HandleAnyMissionStateChanged;
            _approachRoutine = StartCoroutine(ApproachNearestNpcRoutine());
        }

        /// Releases manual control back to the player and stops listening for mission state changes.
        /// Called both when F8 is pressed again mid-cycle and when the cycle finishes on its own.
        private void Deactivate()
        {
            _active = false;
            player.SetAutoControlActive(false);
            MissionManager.Instance.OnAnyMissionStateChanged -= HandleAnyMissionStateChanged;
        }

        private IEnumerator ApproachNearestNpcRoutine()
        {
            NPC nearest = FindNearestNpc();
            if (nearest == null)
            {
                yield break;
            }

            while (nearest != null && Vector3.Distance(player.transform.position, nearest.transform.position) > 1.5f)
            {
                Vector3 dir = nearest.transform.position - player.transform.position;
                dir.y = 0f;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    player.AutoMove(dir.normalized * autoApproachMoveSpeed * Time.deltaTime);
                }
                yield return null;
            }

            player.AutoMove(Vector3.zero);
        }

        private NPC FindNearestNpc()
        {
            NPC[] npcs = Object.FindObjectsByType<NPC>(FindObjectsSortMode.None);
            NPC nearest = null;
            float bestDistance = float.MaxValue;

            foreach (NPC npc in npcs)
            {
                float distance = Vector3.Distance(player.transform.position, npc.transform.position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    nearest = npc;
                }
            }

            return nearest;
        }

        private void HandleAnyMissionStateChanged(MissionState state)
        {
            switch (state)
            {
                case MissionState.Ready:
                    StartCoroutine(AutoAgreeRoutine());
                    break;
                case MissionState.InProgress:
                    StartCoroutine(AutoDodgeRoutine());
                    break;
                case MissionState.Succeeded:
                case MissionState.Failed:
                    StartCoroutine(AutoConfirmRoutine());
                    break;
            }
        }

        private IEnumerator AutoAgreeRoutine()
        {
            yield return new WaitForSeconds(autoAgreeDelay);

            if (MissionManager.Instance.CurrentMission is DefenseMinigameMission dm)
            {
                dm.GetOrCreateDefenseMinigameController().SetSurvivalDurationOverride(autoCycleSurvivalDuration);
            }

            MissionManager.Instance.HandleMissionButtonAction(MissionButtonAction.Left);
        }

        private IEnumerator AutoDodgeRoutine()
        {
            float direction = 1f;
            float traveled = 0f;

            while (MissionManager.Instance.CurrentMission != null &&
                   MissionManager.Instance.CurrentMission.State == MissionState.InProgress)
            {
                float step = autoDodgeMoveSpeed * Time.deltaTime;
                player.AutoMove(Vector3.right * direction * step);
                traveled += step;

                if (traveled >= dodgeSwitchDistance)
                {
                    traveled = 0f;
                    direction *= -1f;
                }

                yield return null;
            }

            player.AutoMove(Vector3.zero);
        }

        private IEnumerator AutoConfirmRoutine()
        {
            yield return new WaitForSeconds(autoConfirmDelay);

            MissionManager.Instance.HandleMissionResultConfirmed();

            Deactivate();
        }
    }
}
