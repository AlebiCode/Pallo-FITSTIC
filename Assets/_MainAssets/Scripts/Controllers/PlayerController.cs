using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Enrico
{
	public class PlayerController : MonoBehaviour
	{
		InputManager input;
		bool isInitialized = false;
		bool isMoving = false;
		Vector2 moveVector;
		private Rigidbody myRigidbody;

		private void Awake()
		{
			Init();
		}

		void Init()
		{
			if (!isInitialized)
			{
				input = new InputManager();
				isInitialized = true;
				myRigidbody = GetComponent<Rigidbody>();
			}
		}

		private void OnEnable()
		{
			input.Enable();
			input.Gameplay.Move.started += OnMoveStarted;
			input.Gameplay.Move.performed += OnMovePerformed;
			input.Gameplay.Move.canceled += OnMoveCanceled;
			//oppure input.Gameplay.Move.canceled += context => OnMove(context.ReadValue<Vector2>());
		}

		private void OnDisable()
		{
			input.Disable();
			input.Gameplay.Move.started += OnMoveStarted;
			input.Gameplay.Move.performed += OnMovePerformed;
			input.Gameplay.Move.canceled += OnMoveCanceled;
		}

		private void Update()
		{
			if (isMoving)
			{
				myRigidbody.AddForce(new Vector3(moveVector.x, 0, moveVector.y) * 5f);
				//transform.position += (Vector3)moveVector * Time.deltaTime * 5;
			}
		}

		private void OnMoveStarted(InputAction.CallbackContext callbackContext)
		{
			isMoving = true;

			Debug.Log("Move started: " + moveVector);
		}

		private void OnMovePerformed(InputAction.CallbackContext callbackContext)
		{
			moveVector = callbackContext.ReadValue<Vector2>();
			Debug.Log("Move performed: " + moveVector);

		}

		private void OnMoveCanceled(InputAction.CallbackContext callbackContext)
		{
			isMoving = false;
			moveVector = Vector2.zero;
			Vector2 direction = callbackContext.ReadValue<Vector2>();

			Debug.Log("Move cancelled: " + moveVector);

		}
	}
}