using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI Global")]
    public TextMeshProUGUI totalGoldText;

    [System.Serializable]
    public class SkinItem
    {
        public string skinName;
        public int price;
        public Button actionButton;
        public TextMeshProUGUI buttonText;
        public bool isDefault = false;
    }

    [Header("Skins Configuration")]
    public SkinItem[] skins; // Setup 3 items in the inspector

    void Start()
    {
        // Make sure skin 0 is always owned
        PlayerPrefs.SetInt("Skin_0_Owned", 1);
        RefreshUI();
    }

    public void RefreshUI()
    {
        int currentGold = PlayerPrefs.GetInt("SavedGold", 0);
        totalGoldText.text = currentGold + " $";

        int equippedID = PlayerPrefs.GetInt("EquippedSkin", 0);

        for (int i = 0; i < skins.Length; i++)
        {
            bool isOwned = PlayerPrefs.GetInt("Skin_" + i + "_Owned", 0) == 1;

            if (i == equippedID)
            {
                skins[i].buttonText.text = "EQUIPPED";
                skins[i].actionButton.interactable = false;
            }
            else if (isOwned)
            {
                skins[i].buttonText.text = "EQUIP";
                skins[i].actionButton.interactable = true;
            }
            else
            {
                skins[i].buttonText.text = "BUY (" + skins[i].price + ")";
                skins[i].actionButton.interactable = currentGold >= skins[i].price;
            }
        }
    }

    public void OnButtonClick(int id)
    {
        int currentGold = PlayerPrefs.GetInt("SavedGold", 0);
        bool isOwned = PlayerPrefs.GetInt("Skin_" + id + "_Owned", 0) == 1;

        if (isOwned)
        {
            // Just equip it
            PlayerPrefs.SetInt("EquippedSkin", id);
        }
        else
        {
            // Buy it
            if (currentGold >= skins[id].price)
            {
                currentGold -= skins[id].price;
                PlayerPrefs.SetInt("SavedGold", currentGold);
                PlayerPrefs.SetInt("Skin_" + id + "_Owned", 1);
                PlayerPrefs.SetInt("EquippedSkin", id);
            }
        }

        PlayerPrefs.Save();
        RefreshUI();
    }
}