/// <summary>
/// Взаимодействие.
/// </summary>
interface IInteractible {

    /// <summary>
    /// Взаимодействовать.
    /// </summary>
    public void Interact();  
}

interface IDurationInteractible : IInteractible
{
    /// <summary>
    /// Перестать взаимодействовать.
    /// </summary>
    public void InteractStop();
}