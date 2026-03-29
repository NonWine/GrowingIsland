using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class CollectableManager : MonoBehaviour
{
    [FormerlySerializedAs("_walletObjs")]
    [SerializeField] private WalletObj[] walletObjs;
    [FormerlySerializedAs("_walletPrefab")]
    [SerializeField] private CollectableWallet walletPrefab;
    [Inject] private DiContainer diContainer;
    
    private List<CollectableWallet> collectableWallets;

    private void Start()
    {
        collectableWallets = new List<CollectableWallet>();
        foreach (var wallet in walletObjs)
        {
           var wall =  diContainer.InstantiatePrefabForComponent<CollectableWallet>(walletPrefab, transform);
           wall.Init(wallet);
            collectableWallets.Add(wall);
        }
    }
    

    public CollectableWallet GetWallet(eCollectable collectable)
    {
        foreach (var wallet in collectableWallets)
        {
            if (wallet.WalletType == collectable)
                return wallet;
        }
    
        Debug.LogError("Collectable wallet wasn't found!");
        return null;
    }

    [Button]
    public void Add100Wood()
    {
        GetWallet(eCollectable.Wood).Add(1000,false);
    }
    
    
    [Button]
    public void Add100Stone()
    {
        GetWallet(eCollectable.Stone).Add(1000,false);
    }
}
