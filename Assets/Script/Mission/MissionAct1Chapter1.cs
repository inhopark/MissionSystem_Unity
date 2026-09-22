using UnityEngine;

namespace MissionSystem
{
    /// No unique content vs Chapter2 by design (matches the UE5 reference exactly) - log tag only.
    public class MissionAct1Chapter1 : DefenseMinigameMission
    {
        protected override void EnterReadyState()
        {
            base.EnterReadyState();
            Debug.Log("## MissionAct1Chapter1 ## Ready state entered");
        }

        protected override void EnterInProgressState()
        {
            base.EnterInProgressState();
            Debug.Log("## MissionAct1Chapter1 ## InProgress state entered");
        }

        protected override void EnterSucceededState()
        {
            base.EnterSucceededState();
            Debug.Log("## MissionAct1Chapter1 ## Succeeded state entered");
        }

        protected override void EnterFailedState()
        {
            base.EnterFailedState();
            Debug.Log("## MissionAct1Chapter1 ## Failed state entered");
        }
    }
}
