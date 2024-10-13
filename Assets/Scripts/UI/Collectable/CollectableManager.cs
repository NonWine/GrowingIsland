using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [SerializeField, ReadOnly] protected CollectableWallet[] _collectableWallets;
    
    #region Editor
    private void OnValidate() 
        => SetRefs();

    [Button]
    protected virtual void SetRefs()
    {
        _collectableWallets = GetComponentsInChildren<CollectableWallet>(true);
    }
    #endregion

    private void Start()
    {
        foreach (var wallet in _collectableWallets)
        {
            if (wallet.Amount > 0)
                wallet.gameObject.SetActive(true);
        }
    }

    public void SaveAmount()
    {
        foreach (var wallet in _collectableWallets)
        {
            wallet.SaveAmount();
        }
    }
    
    public void PreSaveAmount()
    {
        foreach (var wallet in _collectableWallets)
        {
            wallet.PreSaveAmount();
        }
    }

    public CollectableWallet GetWallet(eCollectable collectable)
    {
        foreach (var wallet in _collectableWallets)
        {
            if (wallet.CollectableType == collectable)
                return wallet;
        }
    
        Debug.LogError("Collectable wallet wasn't found!");
        return null;
    }
}
