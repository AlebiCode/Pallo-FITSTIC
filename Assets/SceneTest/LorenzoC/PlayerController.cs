using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public ControlMap Cm;
    bool isInitialized=false;
    public float speed = 5f;
    Vector3 dir;
    bool isMoving=false;

    private void Awake() {
            Init();
        
    }

    private void Init() {
        if (!isInitialized) {
            Cm = new ControlMap();
            isInitialized = true;
        }
    }

    private void OnEnable() {
        Cm.Enable();
        Cm.Gameplay.Movement.started += OnMoveStarted;
        Cm.Gameplay.Movement.performed +=OnMovePerformed;
        Cm.Gameplay.Movement.canceled += OnMoveCanceled;
    }

    void OnMoveCanceled(InputAction.CallbackContext callbackContext) {
        dir = Vector3.zero;
       isMoving = false;
    }

    private void OnMovePerformed(InputAction.CallbackContext callbackContext) {
        dir = (Vector3)callbackContext.ReadValue<Vector2>();
    }

    private void OnMoveStarted(InputAction.CallbackContext callbackContext) {
        dir = (Vector3)callbackContext.ReadValue<Vector2>();
        isMoving = true;
    }

    private void OnDisable() {
        Cm.Disable();
        Cm.Gameplay.Movement.started -= OnMoveStarted;
        Cm.Gameplay.Movement.performed -= OnMovePerformed;
        Cm.Gameplay.Movement.canceled -= OnMoveCanceled;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Cm == null) {
            Debug.LogError("Player " + this.gameObject.name + " doesn't have a Character Controller attached");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) {
            transform.position += dir * Time.deltaTime * speed;
        }
        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //cc.Move(move * Time.deltaTime * speed);
    }
}
