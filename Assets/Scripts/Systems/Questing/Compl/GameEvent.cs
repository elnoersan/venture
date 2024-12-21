// An abstract base class for all game events
public abstract class GameEvent
{
    public string EventDescription; // A description of the event (optional)
}

// A specific event class that represents when a unit is defeated
public class UnitDefeatedEvent : GameEvent
{
    public string unitName; // The name of the unit that was defeated

    // Constructor to initialize the event with the unit's name
    public UnitDefeatedEvent(string name)
    {
        unitName = name;
    }
}