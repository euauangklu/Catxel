using UnityEngine;

[CreateAssetMenu(menuName = "Events/Cat Butt Event")]
public class CatButtEvent : GameEvents
{
    [SerializeField] private GameObject _gameObject;

    public override void TriggerEvent()
    {
        Instantiate(_gameObject, new Vector3(0, -1.5f, 0), Quaternion.identity);
    }

    public override void ResetEvent()
    {
    }
}
