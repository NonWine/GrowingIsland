using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Wallet", fileName = "Wallet", order = 0)]
public class WalletObj : ScriptableObject
{
    public Sprite ItemIcon;
    public eCollectable TypeWallet;
}