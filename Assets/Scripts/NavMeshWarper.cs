using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshWarper : MonoBehaviour
{
   [SerializeField, ReadOnly] private bool _inWarpZone;
   [SerializeField] private NavMeshLink _warpZone;
   
   private void OnTriggerEnter(Collider other)
   {
      if (other.transform.CompareTag("Player"))
      {

          var agent =
            other.transform.GetComponent<NavMeshAgent>();
         if (agent.isOnOffMeshLink)
         {
            float nearDist = 0f;
            float distanceFromStartPoint, distanceFromEndPoint;
            distanceFromEndPoint = (agent.transform.position - _warpZone.endPoint).magnitude;
            distanceFromStartPoint = (agent.transform.position - _warpZone.startPoint).magnitude;

            if (distanceFromEndPoint < distanceFromStartPoint)
            {
               agent.Warp(_warpZone.startPoint);
            }
            else if (distanceFromStartPoint < distanceFromEndPoint)
            {
               agent.Warp(_warpZone.endPoint);

            }
            
         }

      }
   }
}
