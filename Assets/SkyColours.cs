using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SkyColours : MonoBehaviour {

	public Color[] Colours;
	SpriteRenderer SkyRenderer;

	void Start() {
		SkyRenderer = GetComponent<SpriteRenderer> ();
	}

	public void ChangeColour() {
		SkyRenderer.color = Colours[Random.Range(0, Colours.Length)];
	}
}
