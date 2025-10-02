using System;
using UnityEngine;

public class BaseMiniGame : MonoBehaviour
{
    public event Action<float> OnAddToProgression;
    public Action<float> GetOnAddToProgression() => OnAddToProgression;

    public virtual void Initialize()
    {
        OnAddToProgression?.Invoke(0.2f);
    }
}
