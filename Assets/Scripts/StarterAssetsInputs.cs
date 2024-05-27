using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
        public bool reload;
		[SerializeField] private GameObject GameLogic;
		public int weaponIndex = 0;
		public bool pause;
		public bool select;
		public bool inventory;
		public bool switchNextWeapon;
        public bool switchPreviousWeapon;
        public bool shooting;
		public bool rifleShooting;
		public bool shootingHold;
		public bool interact;

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;


#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
            JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
        }
        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }
		public void OnReload(InputValue value)
		{
            reload = value.isPressed;
        }
		public void OnSwitchNextWeapon(InputValue value)
		{
            SwitchNextWeaponInput(value.isPressed);
			switchNextWeapon = value.isPressed;
        }
        public void OnSwitchPreviousWeapon(InputValue value)
        {
            SwitchPreviousWeaponInput(value.isPressed);
			switchPreviousWeapon = value.isPressed;
        }
        public void OnShooting(InputValue value)
        {
            ShootingInput(value.isPressed);
        }

		public void OnPause(InputValue value)
		{
			pause = value.isPressed;
        }

		public void OnInventory(InputValue value)
		{
            inventory = value.isPressed;
        }
		public void OnInteract(InputValue value)
		{
            InteractInput(value.isPressed);
        }

		public void OnSelect(InputValue value)
		{
            select = value.isPressed;
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AimInput(bool newAimState)
		{
            aim = newAimState;
        }

		public void SwitchNextWeaponInput(bool newWeapon)
		{
			if(newWeapon && weaponIndex < GameLogic.GetComponent<GameLogic>().currentWeapons.Count - 1)
                weaponIndex++;
			else if(newWeapon)
				weaponIndex = 0;
        }

        public void SwitchPreviousWeaponInput(bool newWeapon)
        {
            if (newWeapon && weaponIndex > 0)
                weaponIndex--;
            else if (newWeapon)
                weaponIndex = GameLogic.GetComponent<GameLogic>().currentWeapons.Count - 1;
        }

		public void ShootingInput(bool newShootingState)
		{
			shooting = newShootingState;
            rifleShooting = newShootingState;
        }

		public void InteractInput(bool newInteractState)
		{
			   interact = newInteractState;
		}

		public void SelectInput(bool newSelectState)
		{
            select = newSelectState;
        }

        private void OnApplicationFocus(bool hasFocus)
		{
            SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}