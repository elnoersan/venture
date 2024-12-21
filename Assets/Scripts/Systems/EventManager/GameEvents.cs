using System;

/// <summary>
/// Manages global game events and provides methods to invoke these events.
/// This class is a persistent singleton that allows other systems to subscribe to and respond to game events.
/// </summary>
public class GameEvents : PersistentSingleton<GameEvents>
{
    /*
     * Dialogue Events
     */
    /// <summary>
    /// Event triggered when a dialogue is shown.
    /// </summary>
    public event Action onShowDialog;

    /// <summary>
    /// Invokes the onShowDialog event, notifying subscribers that a dialogue has been shown.
    /// </summary>
    public void OnShowDialogInvoke()
    {
        onShowDialog?.Invoke();
    }

    /// <summary>
    /// Event triggered when a dialogue is closed.
    /// </summary>
    public event Action onCloseDialog;

    /// <summary>
    /// Invokes the onCloseDialog event, notifying subscribers that a dialogue has been closed.
    /// </summary>
    public void OnCloseDialogInvoke()
    {
        onCloseDialog?.Invoke();
    }

    /*
     * Questing Events
     */
    /// <summary>
    /// Event triggered when the player wins a combat encounter.
    /// </summary>
    public event Action onCombatVictory;

    /// <summary>
    /// Invokes the onCombatVictory event, notifying subscribers that the player has won a combat encounter.
    /// </summary>
    public void CombatVictoryInvoke()
    {
        onCombatVictory?.Invoke();
    }

    /// <summary>
    /// Event triggered when the player gathers an item or resource.
    /// </summary>
    public event Action<string> onGatheredTrigger;

    /// <summary>
    /// Invokes the onGatheredTrigger event, notifying subscribers that the player has gathered an item or resource.
    /// </summary>
    /// <param name="name">The name of the item or resource that was gathered.</param>
    public void GatheredInvoke(string name)
    {
        if (onGatheredTrigger != null)
        {
            onGatheredTrigger(name);
        }
    }

    /// <summary>
    /// Event triggered when the player interacts with an object or NPC.
    /// </summary>
    public event Action<string> onInteractedWithTrigger;

    /// <summary>
    /// Invokes the onInteractedWithTrigger event, notifying subscribers that the player has interacted with an object or NPC.
    /// </summary>
    /// <param name="name">The name of the object or NPC that was interacted with.</param>
    public void InteractedWithTrigger(string name)
    {
        if (onInteractedWithTrigger != null)
        {
            onInteractedWithTrigger(name);
        }
    }
}