using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils;
using UnityEngine.Serialization;

public class ConstructionUI : MonoBehaviour
{
    [FormerlySerializedAs("_uiLabelObj")]
    [SerializeField] private GameObject uiLabelObj;
    [FormerlySerializedAs("_uiTriggerZone")]
    [SerializeField] private GameObject uiTriggerZone;
    [FormerlySerializedAs("_uiLabel")]
    [SerializeField] private AutoSizeUILabel uiLabel;
    
    [ShowInInspector, ReadOnly]
    private Dictionary<eCollectable, TextIconView> resourceTexts = new Dictionary<eCollectable, TextIconView>();

    public void InitializeUI(List<ResourceRequirement> requirements)
    {
        if (uiLabel == null)
            uiLabel = GetComponentInChildren<AutoSizeUILabel>();
        
        if(resourceTexts.Count == requirements.Count)
            return;
            
        int i = 0;
        foreach (var requirement in requirements)
        {
             uiLabel.Labels[i].gameObject.SetActive(true);
             var display = uiLabel.Labels[i];
             string text ="0/" +requirement.amount.ToString();
            display.SetIconView(requirement.WalletObj.ItemIcon, text);
            resourceTexts[requirement.WalletObj.TypeWallet] = display;
            i++;
        }
        uiLabel.UpdateRectToPadding();
     //   uiLabel
    }

    public void UpdateUI(eCollectable type, int current, int total)
    {
        if (resourceTexts.ContainsKey(type))
        {
            resourceTexts[type].UpdateText( $"{current}/{total}");
        }
    }

    public void Hide()
    {
        uiLabelObj.SetActive(false);
        uiTriggerZone.SetActive(false);
    }
}
