using UnityEngine;

namespace Duck_Shoot
{
    public class CustomPlayerInputHandler : PlayerInputHandler
    {
        private bool _isFiring;

        [Header("Custom")]
        [Tooltip("Reference to the main camera used for the player")]
        public Camera playerCamera;

        [Tooltip("Reference to the weapon, to make weapon follow camera")]
        public GameObject weaponRotation;

        void Update()
        {
            if (weaponRotation != null)
                weaponRotation.transform.rotation = playerCamera.transform.rotation;
        }

        public void OnFireButton(bool isPressed)
        {
            this._isFiring = isPressed;
        }

        public override bool GetFireInputHeld()
        {
            if (CanProcessInput()) {
                return _isFiring;
            }

            return false;
        }

        #region Disabled Overrides

        public override bool CanProcessInput()
        {
            return true;
        }

        public override bool GetAimInputHeld()
        {
            return false;
        }

        public override bool GetCrouchInputDown()
        {
            return true;
        }

        public override bool GetCrouchInputReleased()
        {
            return true;
        }

        public override float GetMouseOrStickLookAxis(string mouseInputName, string stickInputName)
        {
            return 0f;
        }

        public override int GetSwitchWeaponInput()
        {
            return 0;
        }

        public override bool GetJumpInputHeld()
        {
            return false;
        }

        public override bool GetJumpInputDown()
        {
            return false;
        }

        public override int GetSelectWeaponInput()
        {
            return 0;
        }

        public override bool GetSprintInputHeld()
        {
            return false;
        }

        public override Vector3 GetMoveInput()
        {
            return Vector3.zero;
        }

        #endregion
    }
}