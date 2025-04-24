using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Events/GameEvents")]
public abstract class GameEvents : ScriptableObject
{
    public string eventName;
    public string description;
    public abstract void TriggerEvent();
    public abstract void ResetEvent();
}
