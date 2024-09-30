using UnityEngine;
using UnityEngine.AI;

public class EnemyMoving : IMoveable
{
    private NavMeshAgent _navMeshAgent;
    private float _timeBetweenChangeDirection;
    private float _timer = Mathf.Infinity;
    private Vector3 _offset;

    public EnemyMoving(NavMeshAgent navMeshAgent , float timeBetweenChangeDirection)
    {
        _navMeshAgent = navMeshAgent;
        _timeBetweenChangeDirection = timeBetweenChangeDirection;
    }
    
    public void Move()
    {
        _timer += Time.deltaTime;
           

        if (_timer >= _timeBetweenChangeDirection)
        {
            _navMeshAgent.velocity = Vector3.zero;
            _timeBetweenChangeDirection = Random.Range(2f, 3f);
            float randomAngle = Random.Range(0f, 2 * Mathf.PI);
            _offset = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
            _timer = 0f;
        }
        _navMeshAgent.Move(_offset.normalized * (_navMeshAgent.speed * Time.deltaTime));
        Quaternion targetRotation = Quaternion.LookRotation(_offset, Vector3.up);
           _navMeshAgent.transform.rotation = Quaternion.Slerp(_navMeshAgent.transform.rotation, targetRotation, Time.deltaTime * 2);    
    }
}

