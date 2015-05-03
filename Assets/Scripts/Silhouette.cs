using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class Silhouette : MonoBehaviour
{
	public Sprite[] SilhouetteTemplates;
	public string Type;

	public float FailThreshold;

	SpriteRenderer sr;

	float match;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		sr.sprite = SilhouetteTemplates.SingleOrDefault(x => x.name == Type);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			Debug.Log("Matches to " + TestMatch() + "%");
		}
	}

	float TestMatch()
	{
		var rt = RenderTexture.GetTemporary((int) sr.sprite.textureRect.width, (int) sr.sprite.textureRect.height, 24,
		                                    RenderTextureFormat.ARGB32,
		                                    RenderTextureReadWrite.Default, 1);

		var cam = gameObject.AddComponent<Camera>();
		cam.orthographic = true;
		cam.orthographicSize = sr.sprite.textureRect.height / sr.sprite.pixelsPerUnit / 2.0f;
		cam.nearClipPlane = -5;
		cam.cullingMask = LayerMask.GetMask("Silhouette");
		cam.targetTexture = rt;
		cam.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		cam.clearFlags = CameraClearFlags.SolidColor;

		// snapshot just the silhouette
		cam.Render();

		RenderTexture.active = rt;
		var silhouetteTex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
		silhouetteTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
		RenderTexture.active = null;

		// snapshot the snax (only)
		gameObject.layer = 0;
		foreach (var snack in FindObjectsOfType<Snack>())
			if (snack.placed)
				snack.gameObject.layer = LayerMask.NameToLayer("Silhouette");

		cam.Render();

		RenderTexture.active = rt;
		var snacksTex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
		snacksTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
		RenderTexture.active = null;

		// revert
		gameObject.layer = LayerMask.NameToLayer("Silhouette");
		foreach (var snack in FindObjectsOfType<Snack>())
			snack.gameObject.layer = LayerMask.NameToLayer("Snacks");

		// cleanup
		cam.targetTexture = null;
		RenderTexture.ReleaseTemporary(rt);
		Destroy(cam);

		// debug
		byte[] bytes = silhouetteTex.EncodeToPNG();
		File.WriteAllBytes("C:\\silhouette.png", bytes);
		bytes = snacksTex.EncodeToPNG();
		File.WriteAllBytes("C:\\snacks.png", bytes);

		// compare
		float matchingPixellos = 0, expectedPixellos = 0;
		for (int i = 0; i < silhouetteTex.width; i++)
		for (int j = 0; j < silhouetteTex.height; j++)
		{
			var silhouettePixel = silhouetteTex.GetPixel(i, j).a != 0;
			var snackPixel = snacksTex.GetPixel(i, j).a != 0;

			if (silhouettePixel)
			{
				expectedPixellos++;
				if (snackPixel)
					matchingPixellos++;
			}
			else if (snackPixel)
				matchingPixellos--;
		}

		// clean up textures
		Destroy(silhouetteTex);
		Destroy(snacksTex);

		return (float) matchingPixellos / expectedPixellos;
	}
}
