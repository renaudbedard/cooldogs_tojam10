using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Ingredients : MonoBehaviour
{
	public static Ingredients Instance { private set; get; }

	Dictionary<string, Dictionary<string, int>> SnacksToFoods = new Dictionary<string, Dictionary<string, int>>();
	Dictionary<string, Dictionary<string, int>> FoodsToSnacks = new Dictionary<string, Dictionary<string, int>>();
	Dictionary<string, string> FriendlyNames = new Dictionary<string, string>();

	void Awake()
	{
		Instance = this;
		DontDestroyOnLoad(gameObject);

		var csv = Resources.Load("ingredients") as TextAsset;
		var lines = csv.text.Split('\n').Select(x => x.Split(',')).ToArray();

		// fill in friendly names
		for (int i = 3; i < lines.Length; i++)
			FriendlyNames.Add(lines[i][0], lines[i][1]);
		for (int i = 2; i < lines[0].Length; i++)
			FriendlyNames.Add(lines[0][i], lines[1][i]);

		// snackz 2 foodz
		for (int i = 3; i < lines.Length; i++)
		{
			var dict = new Dictionary<string, int>();
			for (int j = 2; j < lines[i].Length; j++)
				dict.Add(lines[0][j], int.Parse(lines[i][j]));
			SnacksToFoods.Add(lines[i][0], dict);
		}

		// foodz 2 snackz
		for (int j = 2; j < lines[0].Length; j++)
		{
			var dict = new Dictionary<string, int>();
			for (int i = 3; i < lines.Length; i++)
				dict.Add(lines[i][0], int.Parse(lines[i][j]));
			FoodsToSnacks.Add(lines[0][j], dict);
		}

		Resources.UnloadUnusedAssets();

		// debug

		//foreach (var fn in FriendlyNames)
		//	Debug.Log(fn.Key + " = " + fn.Value);

		//foreach (var s2f in SnacksToFoods)
		//	Debug.Log(s2f.Key + " : " + s2f.Value.Aggregate("", (a, b) => a + b.ToString() + ", "));

		//foreach (var f2s in FoodsToSnacks)
		//	Debug.Log(f2s.Key + " : " + f2s.Value.Aggregate("", (a, b) => a + b.ToString() + ", "));
	}
}
