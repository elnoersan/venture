/// <summary>
/// Represents an interactable object in the game.
/// Classes that implement this interface must define the behavior for the Interact method.
/// </summary>
public interface Interactable
{
    /// <summary>
    /// Defines the interaction behavior when the player interacts with the object.
    /// </summary>
    void Interact();
}