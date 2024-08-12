using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    /// <summary>
    /// Ускорение свободного падения.
    /// </summary>
    [SerializeField] float Power = 0.015f;
    private float acceleration = 1f;
    private CharacterController _characterController;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (!_characterController.isGrounded)
        {
            _characterController.Move(Physics.gravity * Time.deltaTime * acceleration);
            acceleration+= Power;
        }
        else 
        {
            acceleration = 1f;
        }
    }
}
