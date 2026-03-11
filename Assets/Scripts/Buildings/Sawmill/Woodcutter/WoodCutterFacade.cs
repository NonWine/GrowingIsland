using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class WoodCutterFacade
{
    private readonly Sawmill sawmill;

    public Sawmill Sawmill => sawmill;
    
    [ShowInInspector, ReadOnly] public EnvironmentResource CurrentTree { get; private set; }
    [ShowInInspector, ReadOnly] public int CarriedWood { get; private set; }
   


    private WoodCutterFacade( Sawmill sawmill)
    {
        this.sawmill = sawmill;
    }
    
    public class Factory : PlaceholderFactory<Sawmill, WoodcutterView> { }
    
    public bool HasTree => CurrentTree != null && CurrentTree.isAlive;
    public bool HasWood => CarriedWood > 0;
    public bool StorageFull => sawmill != null && sawmill.IsStorageFull;
    public int CarryCapacity => sawmill?.CarryCapacity ?? 1;
    public float ChopInterval => sawmill?.ChopInterval ?? 1f;
    
    #region Actions
    public void SetTree(EnvironmentResource tree) => CurrentTree = tree;
    public void ClearTree() => CurrentTree = null;

    public void AddWood(int amount) => CarriedWood = Mathf.Max(0, CarriedWood + amount);
    public void RemoveWood(int amount) => CarriedWood = Mathf.Max(0, CarriedWood - amount);
    public void ResetWood() => CarriedWood = 0;
    
    public void TryDepositWood()
    {
        if (HasWood && sawmill != null)
        {
            var stored = sawmill.DepositWood(CarriedWood);
            RemoveWood(stored);
        }
    }
    #endregion
    
}