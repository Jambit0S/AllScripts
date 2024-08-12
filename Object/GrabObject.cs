using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 0.1f; // Количество болтанки
    public float swaySpeed = 5f; // Скорость движения при болтанке
    [SerializeField] float lengthToObj = 3f;

    private Camera playerCamera;
    private Rigidbody rb;
    private bool isHolding = false;
    private Transform holdPosition;

    private Quaternion originalRotation; // Запоминаем начальную ориентацию объекта

    void Start()
    {
        playerCamera = Camera.main;
        holdPosition = new GameObject("HoldPosition").transform;
        holdPosition.SetParent(playerCamera.transform); // Удерживаем объект относительно камеры
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHolding)
            {
                ReleaseObject();
            }
            else
            {
                TryGrabObject();
            }
        }
    }

    void TryGrabObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, lengthToObj))
        {
            if (hit.rigidbody != null)
            {
                isHolding = true;
                rb = hit.rigidbody;
                rb.useGravity = false;
                rb.drag = 10; // Увеличиваем сопротивление для более плавного движения

                // Запоминаем начальную ориентацию объекта
                originalRotation = rb.rotation;
            }
        }
    }

    void ReleaseObject()
    {
        if (rb != null)
        {
            isHolding = false;
            rb.useGravity = true;
            rb.drag = 1; // Сбрасываем сопротивление
            rb = null;
        }
    }

    void FixedUpdate()
    {
        if (isHolding && rb != null)
        {
            // Позиция, куда мы будем перемещать объект
            Vector3 targetPosition = holdPosition.position;

            // Добавляем болтанку
            Vector3 swayOffset = new Vector3(Mathf.Sin(Time.time * swaySpeed) * swayAmount, 
                                            Mathf.Cos(Time.time * swaySpeed) * swayAmount, 0);
            rb.MovePosition(targetPosition + swayOffset);

            // Сохраняем ориентацию объекта, не позволяя ему поворачиваться
            rb.rotation = originalRotation;
        }
    }

    private void LateUpdate()
    {
        if (isHolding)
        {
            // Устанавливаем позицию holdPosition для объекта
            holdPosition.position = playerCamera.transform.position + playerCamera.transform.forward * 1.5f; // Немного выдвигаем вперед
        }
    }
}
