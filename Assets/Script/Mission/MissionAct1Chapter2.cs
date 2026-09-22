using UnityEngine;

namespace MissionSystem
{
    /// No unique content vs Chapter1 by design (matches the UE5 reference exactly) - log tag only.
    public class MissionAct1Chapter2 : DefenseMinigameMission
    {
        protected override void EnterReadyState()
        {
            base.EnterReadyState();
            Debug.Log("## MissionAct1Chapter2 ## Ready state entered");
        }

        protected override void EnterInProgressState()
        {
            base.EnterInProgressState();
            Debug.Log("## MissionAct1Chapter2 ## InProgress state entered");
        }

        protected override void EnterSucceededState()
        {
            base.EnterSucceededState();
            Debug.Log("## MissionAct1Chapter2 ## Succeeded state entered");
        }

        protected override void EnterFailedState()
        {
            base.EnterFailedState();
            Debug.Log("## MissionAct1Chapter2 ## Failed state entered");
        }
    }
}
