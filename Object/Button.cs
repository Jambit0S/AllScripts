using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractible
{
    [SerializeField] GameObject ObjectToInvokeLogic;

    public void Interact()
    {
        ObjectToInvokeLogic.GetComponent<IInteractible>().Interact();
    }
}
