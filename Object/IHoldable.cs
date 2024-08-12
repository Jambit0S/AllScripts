using UnityEngine;

/// <summary>
/// Взаимодействовать с объектом.
/// </summary>
interface IHoldable {

    /// <summary>
    /// Взять.
    /// </summary>
    public void Grab(GameObject holdPoint);

    /// <summary>
    /// Бросить.
    /// </summary>
    public void Drop();
}