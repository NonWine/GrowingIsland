using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class ResourceThrowAnimation : MonoBehaviour
{
    [Inject] private ResourcePartObjFactory _resourceFactory; 
    [Inject] private Player _playerContainer;
    public Transform point;
    private Coroutine _coroutine;
    
    void Awake()
    {
        
    }
    

    private IEnumerator SpawnResources()
        {
            float delay = 0.05f;
            
            for (int i = 0; i < 10; i++)
            {
                var res = _resourceFactory.Create(eCollectable.Wood);
                res.transform.parent = point;
                res.transform.position = _playerContainer.transform.position;
                res.transform.rotation = Quaternion.Euler(new Vector3(-30f,0f,0f));
                res.transform.DOLocalJump(Vector3.zero,2f,1,0.25f).SetEase(Ease.OutQuad).OnComplete((() =>
                    {
                        res.gameObject.SetActive(false);
                    }));
                yield return new WaitForSeconds(delay);
            }
        }

    public void StartAnim()
    {
        _coroutine =   StartCoroutine(SpawnResources());
    }

    public void Stop()
    {
        StopCoroutine(_coroutine);
    }
}
