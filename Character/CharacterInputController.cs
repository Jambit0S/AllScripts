using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputController : MonoBehaviour
{
    private MyInput _gameInput;
    private IControllable _controllable;

    private void Awake()
    {
        _gameInput = new MyInput();

        _controllable = GetComponent<IControllable>();
        _gameInput.GameplayInput.Interact.started += func =>
            OnInteract(func);
        _gameInput.GameplayInput.Interact.canceled += func =>
            OnInteractStop(func);

        _gameInput.Enable();
    }

    private void Update()
    {
        ReadMovement();
        ReadRotation();
    }

    /// <inheritdoc cref="IControllable.Interact"/>
    void OnInteract(InputAction.CallbackContext context) =>
        _controllable.Interact();

    /// <inheritdoc cref="IControllable.InteractStop"/>
    void OnInteractStop(InputAction.CallbackContext context) =>
        _controllable.InteractStop();

    /// <summary>
    /// Ввести вектор движения персонажа.
    /// </summary>
    private void ReadMovement()
    {
        var inputDirection = _gameInput.GameplayInput.Movement.ReadValue<Vector2>();
        _controllable.Move(inputDirection);
    }

    /// <summary>
    /// Ввести вектор вращения персонажа.
    /// </summary>
    private void ReadRotation()
    {
        var inputDirection = new Vector2(
         _gameInput.Mouse.MouseX.ReadValue<float>(),
         _gameInput.Mouse.MouseY.ReadValue<float>()
        );
        
        _controllable.Rotate(inputDirection);
    }

}
