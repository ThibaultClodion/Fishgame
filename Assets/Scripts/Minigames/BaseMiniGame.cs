using System;
using UnityEngine;

public abstract class BaseMiniGame : MonoBehaviour
{
    public delegate void AddToProgressionDelegate(float value);
    // Not an event because there is always only one
    public AddToProgressionDelegate AddToProgressionCallback;

    // Wrap the callback to avoid NPEs (shouldn't happen but just in case)
    public void AddToProgression(float value)
    {
        if (AddToProgressionCallback == null)
        {
            Debug.LogError("Tried to add to progression when CB was null...");
            return;
        }
        AddToProgressionCallback(value);
    }

    public virtual void Initialize(FishData data)
    {
        AddToProgression(0.35f);
    }

    public virtual void Disable() {}
}
