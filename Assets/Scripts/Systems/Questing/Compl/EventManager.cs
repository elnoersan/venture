using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: https://gist.github.com/bendangelo/093edb33c2e844c5c73a
public class EventManager : MonoBehaviour
{
    public bool LimitQueueProcesing = false; // Whether to limit the time spent processing events in each frame
    public float QueueProcessTime = 0.0f; // The maximum time (in seconds) allowed for processing events in each frame
    private static EventManager s_Instance = null; // Singleton instance of the EventManager
    private Queue m_eventQueue = new Queue(); // Queue to store events that need to be processed

    // Delegate for handling events of type T
    public delegate void EventDelegate<T>(T e) where T : GameEvent;
    private delegate void EventDelegate(GameEvent e);

    private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>(); // Dictionary to map event types to their delegates
    private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>(); // Dictionary to map generic delegates to non-generic ones
    private Dictionary<System.Delegate, System.Delegate> onceLookups = new Dictionary<System.Delegate, System.Delegate>(); // Dictionary to track one-time event listeners

    // Singleton instance property
    public static EventManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = GameObject.FindObjectOfType(typeof(EventManager)) as EventManager;
            }
            return s_Instance;
        }
    }

    // Adds a delegate for a specific event type
    private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent
    {
        // Early-out if we've already registered this delegate
        if (delegateLookup.ContainsKey(del))
            return null;

        // Create a new non-generic delegate which calls our generic one.
        // This is the delegate we actually invoke.
        EventDelegate internalDelegate = (e) => del((T)e);
        delegateLookup[del] = internalDelegate;

        EventDelegate tempDel;
        if (delegates.TryGetValue(typeof(T), out tempDel))
        {
            delegates[typeof(T)] = tempDel += internalDelegate;
        }
        else
        {
            delegates[typeof(T)] = internalDelegate;
        }

        return internalDelegate;
    }

    // Adds a listener for a specific event type
    public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        AddDelegate<T>(del);
    }

    // Adds a one-time listener for a specific event type
    public void AddListenerOnce<T>(EventDelegate<T> del) where T : GameEvent
    {
        EventDelegate result = AddDelegate<T>(del);

        if (result != null)
        {
            // Remember this is only called once
            onceLookups[result] = del;
        }
    }

    // Removes a listener for a specific event type
    public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        EventDelegate internalDelegate;
        if (delegateLookup.TryGetValue(del, out internalDelegate))
        {
            EventDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDelegate;
                if (tempDel == null)
                {
                    delegates.Remove(typeof(T));
                }
                else
                {
                    delegates[typeof(T)] = tempDel;
                }
            }

            delegateLookup.Remove(del);
        }
    }

    // Removes all listeners
    public void RemoveAll()
    {
        delegates.Clear();
        delegateLookup.Clear();
        onceLookups.Clear();
    }

    // Checks if a listener is registered for a specific event type
    public bool HasListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        return delegateLookup.ContainsKey(del);
    }

    // Triggers an event and invokes all registered listeners
    public void TriggerEvent(GameEvent e)
    {
        EventDelegate del;
        if (delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);

            // Remove listeners which should only be called once
            foreach (EventDelegate k in delegates[e.GetType()].GetInvocationList())
            {
                if (onceLookups.ContainsKey(k))
                {
                    delegates[e.GetType()] -= k;

                    if (delegates[e.GetType()] == null)
                    {
                        delegates.Remove(e.GetType());
                    }

                    delegateLookup.Remove(onceLookups[k]);
                    onceLookups.Remove(k);
                }
            }
        }
        else
        {
            Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
        }
    }

    // Queues an event to be processed later
    public bool QueueEvent(GameEvent evt)
    {
        if (!delegates.ContainsKey(evt.GetType()))
        {
            Debug.LogWarning("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetType());
            return false;
        }

        m_eventQueue.Enqueue(evt);
        return true;
    }

    // Processes the event queue
    void Update()
    {
        float timer = 0.0f;
        while (m_eventQueue.Count > 0)
        {
            if (LimitQueueProcesing)
            {
                if (timer > QueueProcessTime)
                    return;
            }

            GameEvent evt = m_eventQueue.Dequeue() as GameEvent;
            TriggerEvent(evt);

            if (LimitQueueProcesing)
                timer += Time.deltaTime;
        }
    }

    // Cleans up the EventManager when the application quits
    public void OnApplicationQuit()
    {
        RemoveAll();
        m_eventQueue.Clear();
        s_Instance = null;
    }
}