using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Events/GameEvents")]
public abstract class GameEvents : ScriptableObject
{
    public abstract void TriggerEvent(RandomEventManager manager);
}
