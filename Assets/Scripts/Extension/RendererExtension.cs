using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class RendererExtension 
    {
		public static void ColorAlpha(this Color color)
		{
			color.a = 1f;
		}

		public static void ColorAlpha(this Color color, float alpha)
		{
			color.a = alpha;
		}

		public static void SetAlpha(this Color color)
		{
			Color colors = new();
			colors = color;
			colors.a = 1;
			color = colors;
		}

		public static void SetAlpha(this Color color, float alpha)
		{
			Color colors = new();
			colors = color;
			colors.a = alpha;
			color = colors;
		}
		

		public static void CroosFadeMat(this Material mat, float fadeTime = 1f, float timeStart = 0f, Action after = null)
		{
			DOVirtual.DelayedCall(timeStart, () =>
			{
				DOTween.To(() => mat.color, x => mat.color = x, new Color(1, 1, 1, 0), fadeTime).OnComplete(() => after?.Invoke());
			});
		}

		public static void CroosFadeMat(this Material mat, Color color, float fadeTime = 1f, float timeStart = 0f, Action after = null)
		{
			DOVirtual.DelayedCall(timeStart, () =>
			{
				DOTween.To(() => mat.color, x => mat.color = x, color, fadeTime).OnComplete(() => after?.Invoke());
			});
		}

		public static void SetMaterial(this Renderer rend, Material mat, int ind)
		{
			Material[] mats = rend.materials;
			mats[ind] = mat;
			rend.materials = mats;
		}

		public static void SetMaterial(this Renderer rend, List<Material> materials, List<int> ind)
		{
			Material[] mats = rend.materials;
			for (int i = 0; i < materials.Count; i++)
			{
				mats[i] = materials[ind[i]];
			}
			
			rend.materials = mats;
		}

		public static void SetMaterial(this Renderer rend, List<Material> materials)
		{
			rend.materials = materials.ToArray();
		}


		public static void SetMaterial(this Renderer rend, List<MatIndex> matIndex)
		{
			Material[] mats = rend.materials;

			for (int i = 0; i < matIndex.Count; i++)
			{
				mats[matIndex[i].index] = matIndex[i].material;
			}

			rend.materials = mats;
		}

	}
}

[Serializable]
public class MatIndex
{
	public int index;
	public Material material;
}