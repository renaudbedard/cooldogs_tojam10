using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FailureMarks : MonoBehaviour
{
	public int Fails;
	public GameObject[] Xes;

	public IEnumerator Show()
	{
		yield return new WaitForSeconds(2.25f);

		GetComponent<AudioSource>().Play();

		Fails++;
		if (Fails > 3) Fails = 3;

		for (int i = 0; i < Xes.Length; i++)
			Xes[i].GetComponent<SpriteRenderer>().enabled = i < Fails;

		yield return new WaitForSeconds(1.75f);

		for (int i = 0; i < Xes.Length; i++)
			Xes[i].GetComponent<SpriteRenderer>().enabled =false;
	} 
}
