using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConstructionUI : MonoBehaviour
{
    [SerializeField] private Transform resourceDisplayParent;
    [SerializeField] private TextIconView resourceDisplayPrefab;

    private Dictionary<eCollectable, TextIconView> resourceTexts = new Dictionary<eCollectable, TextIconView>();

    public void InitializeUI(List<ResourceRequirement> requirements)
    {
        foreach (var requirement in requirements)
        {
            var display = Instantiate(resourceDisplayPrefab, resourceDisplayParent);
            string text ="0/" +requirement.amount.ToString();
            display.SetIconView(requirement.WalletObj.ItemIcon, text);
            resourceTexts[requirement.WalletObj.TypeWallet] = display;
        }
    }

    public void UpdateUI(eCollectable type, int current, int total)
    {
        if (resourceTexts.ContainsKey(type))
        {
            resourceTexts[type].UpdateText( $"{current}/{total}");
        }
    }
}