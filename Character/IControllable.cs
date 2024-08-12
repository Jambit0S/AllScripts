using UnityEngine;

/// <summary>
/// Методы воздействия игрока на мир.
/// </summary>
interface IControllable {

    /// <summary>
    /// Движиение.
    /// </summary>
    /// <param name="inputDirection">Вектор куда идем.</param>
    public void Move(Vector2 inputDirection);

    /// <summary>
    /// Вращение.
    /// </summary>
    /// <param name="inputDirection">Вектор куда тянем мышь.</param>
    public void Rotate(Vector2 inputDirection);

    /// <summary>
    /// Взаимодействовать с объектом.
    /// </summary>
    public void Interact();

    /// <summary>
    /// Перестать взаимодействовать с объектом.
    /// </summary>
    public void InteractStop();

    /// <summary>
    /// Кинуть\ударить.
    /// </summary>
    public void Throw();
}