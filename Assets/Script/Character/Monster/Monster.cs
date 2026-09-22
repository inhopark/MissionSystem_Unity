using System;
using UnityEngine;

namespace MissionSystem
{
    [RequireComponent(typeof(SphereCollider))]
    public class Monster : MonoBehaviour
    {
        private const float MoveSpeed = 9f;
        private const float MaxTravelDistance = 30f;
        private const float DamageAmount = 10f;
        private const float VisualScale = 0.7f;
        private const float ColliderRadius = 0.5f;

        public event Action<Monster> OnDeactivated;

        private float _distanceTraveled;
        private bool _isActive;

        /// No custom mesh assigned - falls back to a scaled sphere primitive, matching the UE5
        /// reference's own no-mesh-assigned fallback for AMonster.
        public static GameObject CreateDefault()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Monster";
            go.transform.localScale = Vector3.one * VisualScale;

            SphereCollider sphere = go.GetComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = ColliderRadius / VisualScale;

            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;

            go.AddComponent<Monster>();
            go.SetActive(false);

            return go;
        }

        public void ActivateAt(Vector3 spawnPosition)
        {
            transform.position = spawnPosition;
            _distanceTraveled = 0f;
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (_isActive == false)
            {
                return;
            }

            _isActive = false;
            gameObject.SetActive(false);
            OnDeactivated?.Invoke(this);
        }

        private void Update()
        {
            if (_isActive == false)
            {
                return;
            }

            float step = MoveSpeed * Time.deltaTime;
            transform.position += Vector3.back * step;
            _distanceTraveled += step;

            if (_distanceTraveled >= MaxTravelDistance)
            {
                Deactivate();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActive == false)
            {
                return;
            }

            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player == null)
            {
                return;
            }

            player.ApplyDamage(DamageAmount);
            Deactivate();
        }
    }
}
