using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MissionSystem
{
    public enum PlayMode
    {
        Normal,
        Defense
    }

    [RequireComponent(typeof(CharacterController))]
    public class PlayerCharacter : MonoBehaviour
    {
        [Header("Wired at edit-time via Unity Editor / MCP")]
        public InputActionAsset inputActions;
        public Camera followCamera;
        public Transform visual;
        public Animator visualAnimator;

        [Header("Movement")]
        [SerializeField] private float normalModeMoveSpeed = 5f;
        [SerializeField] private float defenseModeMoveSpeed = 8.5f;
        [SerializeField] private float bodyTurnSpeed = 720f;

        [Header("Camera Boom (fixed quarter-view - yaw never rotates; WASD is plain forward/left/back/right)")]
        [SerializeField] private float pivotHeight = 1.6f;
        [SerializeField] private float normalArmLength = 6f;
        [SerializeField] private float normalPitch = 30f;
        [SerializeField] private float defenseArmLength = 13f;
        [SerializeField] private float defenseForwardOffset = 4.5f;
        [SerializeField] private float defensePitch = 55f;
        [SerializeField] private float normalCameraInterpRate = 4f;
        [SerializeField] private float defenseCameraInterpRate = 3f;

        [Header("HP")]
        [SerializeField] private float maxHP = 100f;

        public event Action<float, float> OnHPChanged;

        public PlayMode CurrentPlayMode { get; private set; } = PlayMode.Normal;
        public float CurrentHP { get; private set; }
        public float MaxHP => maxHP;

        private CharacterController _controller;
        private InputAction _moveAction;
        private bool _movementLocked;
        private bool _autoControlActive;

        // Camera yaw is always 0 (fixed world-space direction) - only pitch/arm-length/offset
        // interpolate between Normal and Defense mode. WASD therefore always maps to the same
        // screen-relative forward/left/back/right regardless of which way the character faces.
        private float _currentPitch;
        private float _currentArmLength;
        private float _currentForwardOffset;
        private string _currentAnimationState;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            CurrentHP = maxHP;
            _currentArmLength = normalArmLength;
            _currentPitch = normalPitch;

            if (inputActions != null)
            {
                InputActionMap map = inputActions.FindActionMap("Player");
                _moveAction = map?.FindAction("Move");
            }
        }

        private void Start()
        {
            MissionManager.Instance.RegisterPlayer(this);
        }

        private void OnEnable()
        {
            _moveAction?.Enable();
        }

        private void OnDisable()
        {
            _moveAction?.Disable();
        }

        private void Update()
        {
            if (_autoControlActive == false)
            {
                HandleMove();
            }

            UpdateCameraBoom();
        }

        /// While a mission dialog/result widget is open, the player cannot move or look
        /// (mirrors UE5's SetUICursorMode toggling bIgnoreMoveInput/bIgnoreLookInput).
        public void SetMovementLocked(bool locked)
        {
            _movementLocked = locked;
        }

        /// While the F8 auto-cycle debug tool drives the player via AutoMove, HandleMove's keyboard
        /// path must stay off - both write the same animator state each frame, and having them fight
        /// over Idle/Walk every frame restarts the animation constantly, reading as a jittery stutter.
        public void SetAutoControlActive(bool active)
        {
            _autoControlActive = active;
        }

        private void HandleMove()
        {
            Vector2 moveInput = (_movementLocked == false && _moveAction != null) ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
            float speed = CurrentPlayMode == PlayMode.Defense ? defenseModeMoveSpeed : normalModeMoveSpeed;

            // Defense mode: world X-axis-only dodge movement, character always faces world forward.
            // Normal mode: fixed camera yaw (0) means world axes double as screen axes (W/S = forward/back,
            // A/D = left/right) with no dependency on which way the character is currently facing.
            Vector3 motion = CurrentPlayMode == PlayMode.Defense
                ? Vector3.right * moveInput.x
                : Vector3.forward * moveInput.y + Vector3.right * moveInput.x;

            if (CurrentPlayMode != PlayMode.Defense && motion.sqrMagnitude > 1f)
            {
                motion.Normalize();
            }

            _controller.SimpleMove(motion * speed);
            ApplyMovementVisuals(motion);
        }

        /// Turns the visual to face travel direction (or snaps forward in Defense mode) and drives
        /// the Idle/Walk/Run animator state. Shared by manual input (HandleMove) and the F8 auto-cycle
        /// debug tool (AutoMove), which otherwise bypasses this and never animated.
        private void ApplyMovementVisuals(Vector3 motion)
        {
            bool isMoving = motion.sqrMagnitude > 0.0001f;

            if (CurrentPlayMode == PlayMode.Defense)
            {
                if (visual != null)
                {
                    visual.rotation = Quaternion.identity;
                }
            }
            else if (isMoving == true && visual != null)
            {
                Quaternion targetRot = Quaternion.LookRotation(motion, Vector3.up);
                visual.rotation = Quaternion.RotateTowards(visual.rotation, targetRot, bodyTurnSpeed * Time.deltaTime);
            }

            UpdateAnimation(isMoving);
        }

        private void UpdateAnimation(bool isMoving)
        {
            if (visualAnimator == null)
            {
                return;
            }

            // Normal mode moves at walking pace; Defense mode's faster dodge reads better as a run.
            string targetState = isMoving == true
                ? (CurrentPlayMode == PlayMode.Defense ? "Run" : "Walk")
                : "Idle";

            if (targetState == _currentAnimationState)
            {
                return;
            }

            _currentAnimationState = targetState;
            visualAnimator.Play(targetState);
        }

        private void UpdateCameraBoom()
        {
            bool defense = CurrentPlayMode == PlayMode.Defense;
            float targetArmLength = defense == true ? defenseArmLength : normalArmLength;
            float targetForwardOffset = defense == true ? defenseForwardOffset : 0f;
            float targetPitch = defense == true ? defensePitch : normalPitch;
            float interpRate = defense == true ? defenseCameraInterpRate : normalCameraInterpRate;
            float t = 1f - Mathf.Exp(-interpRate * Time.deltaTime);

            _currentArmLength = Mathf.Lerp(_currentArmLength, targetArmLength, t);
            _currentForwardOffset = Mathf.Lerp(_currentForwardOffset, targetForwardOffset, t);
            _currentPitch = Mathf.LerpAngle(_currentPitch, targetPitch, t);

            if (followCamera == null)
            {
                return;
            }

            Vector3 pivotPosition = transform.position + Vector3.up * pivotHeight;
            Quaternion pivotRotation = Quaternion.Euler(_currentPitch, 0f, 0f);
            Vector3 pivotForward = pivotRotation * Vector3.forward;

            // Positive pitch tilts pivotForward downward, so subtracting it places the camera
            // above-and-behind looking diagonally down - the quarter-view framing.
            Vector3 camPosition = pivotPosition - pivotForward * (_currentArmLength - _currentForwardOffset);

            followCamera.transform.position = camPosition;
            followCamera.transform.rotation = pivotRotation;
        }

        public void SetPlayMode(PlayMode mode)
        {
            CurrentPlayMode = mode;
        }

        /// Bypasses normal input, used by the auto-cycle debug tool (mirrors AddMovementInput bForce=true).
        public void AutoMove(Vector3 worldDelta)
        {
            _controller.Move(worldDelta);
            ApplyMovementVisuals(worldDelta);
        }

        public void ApplyDamage(float amount)
        {
            CurrentHP = Mathf.Clamp(CurrentHP - amount, 0f, maxHP);
            OnHPChanged?.Invoke(CurrentHP, maxHP);

            if (CurrentHP <= 0f)
            {
                MissionManager.Instance.HandlePlayerDefeated();
            }
        }

        public void ResetHP()
        {
            CurrentHP = maxHP;
            OnHPChanged?.Invoke(CurrentHP, maxHP);
        }

        public void WarpTo(Vector3 position, Quaternion rotation)
        {
            _controller.enabled = false;
            transform.position = position;
            transform.rotation = Quaternion.identity;
            if (visual != null)
            {
                visual.rotation = rotation;
            }
            _controller.enabled = true;
        }
    }
}
