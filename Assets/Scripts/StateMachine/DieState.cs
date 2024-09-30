using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class DieState : IEnemyState
{
    private NavMeshAgent _meshAgent;
    private BaseEnemy _enemy;
    private Rigidbody _rigidbody;
    private PlayerContainer _player;
    private float _duration;
    private Tweener _tweener;
    public DieState(
        NavMeshAgent navMeshAgent,
        Rigidbody rigidbody,
        PlayerContainer player
         )
    {
        _meshAgent = navMeshAgent;
        _rigidbody = rigidbody;
        _player = player;
        _duration = 1.5f;
    }
    
    public void EnterState(BaseEnemy enemy)
    {
        enemy.InvokeOnDie();
        enemy.Stats.IsDeath = true;
        _meshAgent.enabled = false;
        _rigidbody.isKinematic = false;
    _tweener =    _rigidbody.transform.DOScale(0.5f, _duration * 1.2f);
       _rigidbody.DOJump(_player.transform.position, 3, 1, _duration).SetEase(Ease.OutBack).OnUpdate(() =>
       {
           Vector3 dir = _player.transform.position -  _rigidbody.transform.position ;
           _rigidbody.velocity = dir.normalized * 30;
       }).SetUpdate(UpdateType.Fixed);

    }

    public void UpdateState()
    {
        
        Debug.Log("UPDAYTE");
    }
    
    

    public void ExitState()
    {
        _tweener.Kill();
    }
}