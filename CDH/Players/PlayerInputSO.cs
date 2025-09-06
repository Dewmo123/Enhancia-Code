using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _00.Work.CDH.Code.Players
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public Vector2 InputDirection {  get; private set; }

        private Controls _controls;
        public void SetEnabled(bool enabled)
        {
            if (enabled)
                _controls.Player.Enable();
            else
                _controls.Player.Disable();

        }
        private void OnEnable()
        {
            if(_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        #region PlayerInput

        public event Action OnExitKeyPressed;
        public event Action OnJumpKeyPressed;
        public event Action OnAttackKeyPressed;
        public event Action OnInteractKeyPressed;
        public event Action OnDashKeyPressed;
        public event Action OnSlideKeyPressed;
        public event Action<int> OnSkillKeyPressed;

        public void OnMove(InputAction.CallbackContext context)
        {
            InputDirection = context.ReadValue<Vector2>();
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnAttackKeyPressed?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnInteractKeyPressed?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnJumpKeyPressed?.Invoke();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnDashKeyPressed?.Invoke();
        }
        
        public void OnSlide(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSlideKeyPressed?.Invoke();
        }

        public void OnSkill1(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSkillKeyPressed?.Invoke(0); // 인덱스로 사용하려고
        }

        public void OnSkill2(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSkillKeyPressed?.Invoke(1);
        }

        public void OnSkill3(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSkillKeyPressed?.Invoke(2);
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnExitKeyPressed?.Invoke();
        }

        #endregion PlayerInput
    }
}
