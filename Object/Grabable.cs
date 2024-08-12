using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grabable : MonoBehaviour, IHoldable
{
    [SerializeField] float ObjectSpeed = 1f;
    private Rigidbody _body;

    /// <summary>
    /// Позиция, к которой объект стремится.
    /// </summary>
    private Transform _holdPosition;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    /// <inheritdoc/>
    public void Grab(GameObject holdPoint)
    {
        Debug.Log("Grab");
        _body.useGravity = false;
        _body.freezeRotation = true;
        _holdPosition = holdPoint.transform;
        
        var player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>());
    }

    /// <inheritdoc/>
    public void Drop()
    {
        Debug.Log("Drop");
        _body.useGravity = true;
        _body.freezeRotation = false;
        _holdPosition = null;

        var player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_holdPosition is not null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _holdPosition.transform.position, ObjectSpeed * Time.deltaTime);
        }
    }

}
