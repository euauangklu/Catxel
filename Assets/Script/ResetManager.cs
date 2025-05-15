using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ResetManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void CheckForReset()
    {
        if (inputField.text.ToLower() == "reset")
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            ReloadGameData();
        }
    }

    void ReloadGameData()
    {
        BuyItemManager.Instance.ReloadItems();
    }
}
