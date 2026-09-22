using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissionSystem
{
    /// Object pool: monsters are never destroyed mid-wave, only deactivated and reused.
    public class MonsterSpawner : MonoBehaviour
    {
        private const float SpawnInterval = 0.5f;
        private const float FirstSpawnDelay = 0.5f;
        private const int MinSpawnCount = 1;
        private const int MaxSpawnCount = 3;
        private const float SpawnDistanceZ = 25f;

        // Intentional 1:1 port of the UE5 reference's near-zero lateral spread quirk (was ±1cm there).
        private const float SpawnRangeX = 0.01f;

        private readonly List<Monster> _pooledMonsters = new List<Monster>();
        private readonly List<Monster> _activeMonsters = new List<Monster>();

        private Transform _target;
        private Coroutine _spawnRoutine;

        public void StartSpawning(PlayerCharacter targetPlayer)
        {
            _target = targetPlayer.transform;

            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
            }

            _spawnRoutine = StartCoroutine(SpawnRoutine());
        }

        public void StopSpawning()
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
                _spawnRoutine = null;
            }

            foreach (Monster monster in new List<Monster>(_activeMonsters))
            {
                monster.Deactivate();
            }
        }

        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(FirstSpawnDelay);

            while (true)
            {
                SpawnMonster();
                yield return new WaitForSeconds(SpawnInterval);
            }
        }

        private void SpawnMonster()
        {
            int count = UnityEngine.Random.Range(MinSpawnCount, MaxSpawnCount + 1);
            for (int i = 0; i < count; i++)
            {
                SpawnOneMonster();
            }
        }

        private void SpawnOneMonster()
        {
            if (_target == null)
            {
                return;
            }

            float offsetX = UnityEngine.Random.Range(-SpawnRangeX, SpawnRangeX);
            Vector3 spawnPosition = _target.position + new Vector3(offsetX, 0f, SpawnDistanceZ);

            Monster monster = AcquireMonster();
            monster.ActivateAt(spawnPosition);
        }

        private Monster AcquireMonster()
        {
            Monster monster;

            if (_pooledMonsters.Count > 0)
            {
                int lastIndex = _pooledMonsters.Count - 1;
                monster = _pooledMonsters[lastIndex];
                _pooledMonsters.RemoveAt(lastIndex);
            }
            else
            {
                GameObject go = Monster.CreateDefault();
                go.transform.SetParent(transform, false);
                monster = go.GetComponent<Monster>();
                monster.OnDeactivated += HandleMonsterDeactivated;
            }

            _activeMonsters.Add(monster);
            return monster;
        }

        private void HandleMonsterDeactivated(Monster monster)
        {
            _activeMonsters.Remove(monster);
            _pooledMonsters.Add(monster);
        }
    }
}
