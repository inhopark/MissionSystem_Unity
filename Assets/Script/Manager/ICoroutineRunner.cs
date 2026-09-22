using System.Collections;
using UnityEngine;

namespace MissionSystem
{
    /// Lets a plain C# object (not a MonoBehaviour) run a timed coroutine on a host MonoBehaviour,
    /// mirroring UE5's UDefenseMinigameController using the owning subsystem's FTimerManager.
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
        void StopCoroutine(Coroutine routine);
    }
}
