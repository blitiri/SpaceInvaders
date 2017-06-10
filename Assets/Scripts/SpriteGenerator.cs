using UnityEngine;
using System.Collections;

public class SpriteGenerator : MonoBehaviour
{
	public GameObject pixelPrefab;

	public GameObject Generate (string spriteMapFileName, Color spriteColor, GameObject parent)
	{
		bool[,] spriteMap;
		string[] lines;

		lines = System.IO.File.ReadAllLines (@spriteMapFileName);
		spriteMap = Normalize (lines);
		return Generate (spriteMap, spriteColor, parent);
	}

	public GameObject Generate (bool[,] spriteMap, Color spriteColor, GameObject parent)
	{
		GameObject pixel;
		Renderer pixelRenderer;
		int row;
		int col;

		for (row = 0; row < spriteMap.GetLength (0); row++) {
			for (col = 0; col < spriteMap.GetLength (1); col++) {
				if (spriteMap [row, col]) {
					pixel = Instantiate (pixelPrefab) as GameObject;
					pixel.transform.parent = parent.transform;
					pixel.transform.localPosition = new Vector3 (col, row, 0);
					pixelRenderer = pixel.GetComponent<MeshRenderer> ();
					pixelRenderer.material.color = spriteColor;
				}
			}
		}
		return parent;
	}

	private static bool[,] Normalize (string[] lines)
	{
		string[] dimension;
		bool[,] spriteMap;
		int width;
		int height;
		int row;
		int col;

		spriteMap = new bool[0, 0];
		if (lines.Length > 0) {
			dimension = lines [0].Split (',');
			if (dimension.Length == 2) {
				width = int.Parse (dimension [0]);
				height = int.Parse (dimension [1]);
				spriteMap = new bool[height, width];
				for (row = 0; row < height; row++) {
					for (col = 0; col < width; col++) {
						if ((row < lines.Length) && (col < lines [row].Length)) {
							spriteMap [row, col] = true;
						} else {
							spriteMap [row, col] = false;
						}
					}
				}
			}
		}
		return spriteMap;
	}
}
