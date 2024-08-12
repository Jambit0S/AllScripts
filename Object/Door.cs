using UnityEngine;

public class Door : MonoBehaviour, IInteractible
{
    public void Interact()
    {   
        var locPost = this.transform.localPosition;
        this.transform.localPosition = (locPost + new Vector3(1,0,0));
    }

    public void InteractStop() {}
}