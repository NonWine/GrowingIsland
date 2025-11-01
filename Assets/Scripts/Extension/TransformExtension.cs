using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
	public static class TransformExtension
	{
        public static void SetX(this Transform transform, float x) 
			=> transform.position = new Vector3(x, transform.position.y, transform.position.z);

        public static void SetY(this Transform transform, float y) 
            => transform.position = new Vector3(transform.position.x, y, transform.position.z);

        public static void SetZ(this Transform transform, float z) 
            => transform.position = new Vector3(transform.position.x, transform.position.y, z);

        public static void SetLocalX(this Transform transform, float x) 
            => transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);

        public static void SetLocalY(this Transform transform, float y) 
            => transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);

        public static void SetLocalZ(this Transform transform, float z) 
            => transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);

        public static void SetLocalScaleX(this Transform transform, float x) 
            => transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);

        public static void SetLocalScaleY(this Transform transform, float y) 
            => transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);

        public static void SetLocalScaleZ(this Transform transform, float z) 
            => transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
        
        public static void FadeIn(this Transform transform, float time) 
            => transform.DOScale(Vector3.zero, time);

        public static void FadeIn(this Transform transform, float time, Action action = null) 
            => transform.DOScale(Vector3.zero, time).OnComplete(() => action?.Invoke());
        
        public static Transform GetNearestTarget(this Transform origin, IEnumerable<Transform> targets)
        {
            Transform nearest = null;
            float minDistanceSqr = float.MaxValue;

            Vector3 originPos = origin.position;

            foreach (var target in targets)
            {
                if (target == null) continue;

                float distanceSqr = (target.position - originPos).sqrMagnitude;
                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    nearest = target;
                }
            }

            return nearest;
        }
    }
}


