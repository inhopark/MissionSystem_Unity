using System;
using System.Collections;
using UnityEngine;

namespace MissionSystem
{
    /// Owned by the mission (not the manager) - which missions use the minigame is the mission's decision.
    public class DefenseMinigameController
    {
        public event Action OnSurvived;

        public float SurvivalDuration { get; private set; } = 30f;

        private MonsterSpawner _spawner;
        private ICoroutineRunner _runner;
        private Coroutine _survivalRoutine;
        private float _remainingTime;
        private bool _running;

        /// Only takes effect if called before Start().
        public void SetSurvivalDurationOverride(float seconds)
        {
            SurvivalDuration = seconds;
        }

        public void Start(PlayerCharacter player, ICoroutineRunner runner)
        {
            _runner = runner;
            _remainingTime = SurvivalDuration;
            _running = true;

            _spawner = CreateMonsterSpawner();
            _spawner.StartSpawning(player);

            _survivalRoutine = runner.StartCoroutine(SurvivalTimerRoutine());
        }

        public void Stop()
        {
            _running = false;

            if (_survivalRoutine != null && _runner != null)
            {
                _runner.StopCoroutine(_survivalRoutine);
                _survivalRoutine = null;
            }

            if (_spawner != null)
            {
                _spawner.StopSpawning();
                UnityEngine.Object.Destroy(_spawner.gameObject);
                _spawner = null;
            }
        }

        public float GetRemainingTime() => _running ? _remainingTime : -1f;

        private static MonsterSpawner CreateMonsterSpawner()
        {
            GameObject go = new GameObject("MonsterSpawner");
            return go.AddComponent<MonsterSpawner>();
        }

        private IEnumerator SurvivalTimerRoutine()
        {
            while (_remainingTime > 0f)
            {
                yield return null;
                _remainingTime -= Time.deltaTime;
            }

            _remainingTime = 0f;
            _running = false;
            OnSurvived?.Invoke();
        }
    }
}
