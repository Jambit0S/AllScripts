using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour, IControllable
{
    [SerializeField] CharacterController CharacterController;
    [SerializeField] Camera Camera;
    [SerializeField] float MaxMoveSpeed = 10f;
    private float _currentMoveSpeed;
    [SerializeField] float NormalWeight = 10f;
    [SerializeField] float CameraSensative = 10f;
    [SerializeField] float InteractibleLenght = 3f;
    [SerializeField] float PowerToThrow = 10f;
    private float _xRotation = 0f;
    private GameObject _currentTarget;
    private GameObject _holdPoint;

    /// <summary>
    /// Происходил ли какая интеракция.
    /// </summary>
    private bool _isInteract = false;

    void Start()
    {
        CharacterController ??= GetComponent<CharacterController>();
        Camera ??= GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        _isInteract = false;
        _currentMoveSpeed = MaxMoveSpeed;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, InteractibleLenght))
        {
            if (hit.collider.gameObject is GameObject gameObject 
            && !gameObject.CompareTag("Player")
            && gameObject != _currentTarget)
            {
                InteractStop();
                _currentTarget = gameObject;
            }

        }
        else
        {
            InteractStop();
            _currentTarget = null;
        }
    }

    /// <inheritdoc/>
    public void Move(Vector2 inputDirection)
    {
        var moveDirectionX = inputDirection.x * Camera.transform.right;
        var moveDirectiony = inputDirection.y * Camera.transform.forward;

        moveDirectiony.y = 0;

        var resMove = (moveDirectionX + moveDirectiony).normalized;

        CharacterController.Move(resMove * Time.deltaTime * _currentMoveSpeed);
    }

    /// <inheritdoc/>
    public void Rotate(Vector2 inputDirection)
    {
        inputDirection *= Time.deltaTime * CameraSensative;

        _xRotation -= inputDirection.y;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        Camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        this.transform.Rotate(Vector3.up * inputDirection.x);
    }

    /// <inheritdoc/>
    public void Interact()
    {
        if (_currentTarget is not null 
            && _currentTarget.GetComponent<IInteractible>() is IInteractible interactObj)
        {
            try
            {
                
                interactObj.Interact();
            }
            catch { }
        }

        if (_currentTarget is not null 
            &&_currentTarget.GetComponent<IHoldable>() is IHoldable holdObj)
        {
            try
            {
                _isInteract = true;
                _holdPoint = new GameObject("HolderPoint");
                _holdPoint.transform.SetParent(Camera.transform);
                _holdPoint.transform.position = Camera.transform.position + Camera.transform.forward * 1.5f; // Немного выдвигаем вперед
                _currentMoveSpeed = GetSpeedWithObject(_currentTarget.GetComponent<Rigidbody>().mass);

                holdObj.Grab(_holdPoint);
            }
            catch { }
        }
    }

    /// <inheritdoc/>
    public void InteractStop()
    {
        if (_currentTarget is not null 
            && _isInteract
            &&_currentTarget.GetComponent<IHoldable>() is IHoldable holdObj)
        {
            try
            {
                _isInteract = false;
                _currentMoveSpeed = MaxMoveSpeed;
                Destroy(_holdPoint);

                holdObj.Drop();
            }
            catch { }

        }
    }

    /// <inheritdoc/>
    public void Throw()
    {
        if (_currentTarget is not null 
            && _isInteract
            &&_currentTarget.GetComponent<IHoldable>() is IHoldable throwObj)
        {
            InteractStop();
            _currentTarget.GetComponent<Rigidbody>().AddForce(Camera.transform.forward * PowerToThrow);
        }
    }

    /// <summary>
    /// Получить скорость с объектом.
    /// </summary>
    /// <param name="mass">Масса объекта.</param>
    public float GetSpeedWithObject(float mass) 
    {
        if (mass > NormalWeight)
            return NormalWeight / mass * _currentMoveSpeed;
            
        return _currentMoveSpeed;
    }
}
