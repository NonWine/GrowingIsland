using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NavMeshWarper : MonoBehaviour
{
   [FormerlySerializedAs("_inWarpZone")]
   [SerializeField, ReadOnly] private bool inWarpZone;
   [FormerlySerializedAs("_warpZone")]
   [SerializeField] private NavMeshLink warpZone;
   
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
            distanceFromEndPoint = (agent.transform.position - warpZone.endPoint).magnitude;
            distanceFromStartPoint = (agent.transform.position - warpZone.startPoint).magnitude;

            if (distanceFromEndPoint < distanceFromStartPoint)
            {
               agent.Warp(warpZone.startPoint);
            }
            else if (distanceFromStartPoint < distanceFromEndPoint)
            {
               agent.Warp(warpZone.endPoint);

            }
            
         }

      }
   }
}
