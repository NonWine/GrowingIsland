using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils;

public class ConstructionUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiLabelObj;
    [SerializeField] private GameObject _uiTriggerZone;
    [SerializeField] private AutoSizeUILabel _uiLabel;
    
    [ShowInInspector, ReadOnly]
    private Dictionary<eCollectable, TextIconView> resourceTexts = new Dictionary<eCollectable, TextIconView>();

    public void InitializeUI(List<ResourceRequirement> requirements)
    {
        if (_uiLabel == null)
            _uiLabel = GetComponentInChildren<AutoSizeUILabel>();
        
        if(resourceTexts.Count == requirements.Count)
            return;
            
        int i = 0;
        foreach (var requirement in requirements)
        {
             _uiLabel.Labels[i].gameObject.SetActive(true);
             var display = _uiLabel.Labels[i];
             string text ="0/" +requirement.amount.ToString();
            display.SetIconView(requirement.WalletObj.ItemIcon, text);
            resourceTexts[requirement.WalletObj.TypeWallet] = display;
            i++;
        }
        _uiLabel.UpdateRectToPadding();
     //   _uiLabel
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
        _uiLabelObj.SetActive(false);
        _uiTriggerZone.SetActive(false);
    }
}