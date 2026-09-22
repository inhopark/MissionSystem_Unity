using UnityEngine;

namespace MissionSystem
{
    public class NPC : MonoBehaviour
    {
        public MissionUnique missionUnique = MissionUnique.Act1Chapter1;

        private Renderer[] _renderers;
        private Collider[] _colliders;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>(true);
            _colliders = GetComponentsInChildren<Collider>(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerCharacter>() == null)
            {
                return;
            }

            MissionManager.Instance.ShowMainMissionWidget(missionUnique, this);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerCharacter>() == null)
            {
                return;
            }

            MissionManager.Instance.HideMainMissionWidget();
        }

        public void Show()
        {
            foreach (Renderer r in _renderers)
            {
                r.enabled = true;
            }

            foreach (Collider c in _colliders)
            {
                c.enabled = true;
            }
        }

        public void Hide()
        {
            foreach (Renderer r in _renderers)
            {
                r.enabled = false;
            }

            foreach (Collider c in _colliders)
            {
                c.enabled = false;
            }
        }
    }
}
