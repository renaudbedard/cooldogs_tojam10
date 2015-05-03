using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Ingredients : MonoBehaviour
{
	public static Ingredients Instance { private set; get; }

	public Sprite[] Sprites;
	public GameObject SnackTemplate;

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

		//var recipe = GetRecipe(4, true);
		//Debug.Log("Snacky recipe : " + recipe.Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (4, 0% snacky) : " + GetIngredients(4, recipe, 0).Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (8, 0% snacky) : " + GetIngredients(8, recipe, 0).Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (4, 50% snacky) : " + GetIngredients(4, recipe, 0.5f).Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (8, 50% snacky) : " + GetIngredients(8, recipe, 0.5f).Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (4, 100% snacky) : " + GetIngredients(4, recipe, 1.0f).Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (8, 100% snacky) : " + GetIngredients(8, recipe, 1.0f).Aggregate("", (a, b) => a + b.ToString() + ", "));

		//recipe = GetRecipe(4, false);
		//Debug.Log("Snackless recipe : " + recipe.Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (4, 0% snacky) : " + GetIngredients(4, recipe, 0).Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (8, 0% snacky) : " + GetIngredients(8, recipe, 0).Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (4, 50% snacky) : " + GetIngredients(4, recipe, 0.5f).Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (8, 50% snacky) : " + GetIngredients(8, recipe, 0.5f).Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (4, 100% snacky) : " + GetIngredients(4, recipe, 1.0f).Aggregate("", (a, b) => a + b.ToString() + ", "));
		//Debug.Log("Ingredients (8, 100% snacky) : " + GetIngredients(8, recipe, 1.0f).Aggregate("", (a, b) => a + b.ToString() + ", "));

		//Debug.Log("Ingredients with missing sprites : " + FriendlyNames.Keys.Where(x => !Sprites.Any(y => y.name == x)).Aggregate("", (a, b) => a + b.ToString() + ", "));
	}

	public Dictionary<string, int> GetIngredientLikeness(string ingredient)
	{
		Dictionary<string, int> ret;
		if (FoodsToSnacks.TryGetValue(ingredient, out ret))
			return ret;
		return SnacksToFoods[ingredient];
	}

	public List<string> GetRecipe(int ingredientCount, bool snacks)
	{
		return (snacks ? SnacksToFoods : FoodsToSnacks).Keys.Shuffle().Take(ingredientCount).ToList();
	}

	public List<string> GetIngredients(int ingredientCount, List<string> recipe, float snackiness)
	{
		var ingredients = new List<string>();

		int foodsCount = Mathf.RoundToInt((1 - snackiness) * ingredientCount);

		// start by choosing one good matching ingredient for the recipe
		foreach (var recipeIngredient in recipe)
		{
			if (foodsCount > 0)
			{
				ingredients.Add(recipeIngredient);
				foodsCount--;
			}
			else
				ingredients.Add(GetIngredientLikeness(recipeIngredient).Where(x => x.Value > 0).Shuffle().First().Key);
		}

		// special case for :cooldog:
		var relativeFoods = SnacksToFoods.ContainsKey(recipe[0]) ? SnacksToFoods : FoodsToSnacks;
		var relativeSnacks = SnacksToFoods.ContainsKey(recipe[0]) ? FoodsToSnacks : SnacksToFoods;

		// fill in the rest with random stuff
		while (ingredients.Count < ingredientCount)
		{
			if (foodsCount > 0)
			{
				ingredients.Add(relativeFoods.Keys.Except(ingredients).Shuffle().First());
				foodsCount--;
			}
			else
				ingredients.Add(relativeSnacks.Keys.Except(ingredients).Shuffle().First());
		}

		return ingredients.Shuffle().ToList(); // DAT EFFICIENCY
	}

	public int GetScore(List<string> recipe, List<string> chosenIngredients)
	{
		recipe = recipe.ToList();
		chosenIngredients = chosenIngredients.ToList();

		int recipeSize = recipe.Count;
		int chosenSize = chosenIngredients.Count;

		int points = 0;

		foreach (var recipeIngredient in recipe.ToArray())
		{
			// check for a good-matching ingredient in provided stuff (and eliminate)
			foreach (var ingredient in chosenIngredients.ToArray())
			{
				var dict = GetIngredientLikeness(ingredient);
				int score;
				if (dict.TryGetValue(recipeIngredient, out score))
				{
					if (score == 1)
					{
						points++;
						chosenIngredients.Remove(ingredient);
						recipe.Remove(recipeIngredient);
						break;
					}
					else if (score == 2)
					{
						points += 2;
						chosenIngredients.Remove(ingredient);
						recipe.Remove(recipeIngredient);
						break;
					}
				}
			}
		}

		foreach (var recipeIngredient in recipe)
		{
			// check for real bad matches
			foreach (var ingredient in chosenIngredients)
			{
				var dict = GetIngredientLikeness(ingredient);
				int score;
				if (dict.TryGetValue(recipeIngredient, out score) && score == -1)
				{
					points -= 2;
					break;
				}
			}
		}

		// unfulfilled = bad
		points -= recipe.Count;

		if (points >= recipeSize)
			return 2;
		if (points > recipeSize / 2)
			return 1;
		return 0;
	}
}

public static class IEnumerableExtensions
{
	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
	{
		// DO NOT TRY THIS AT HOME FOLKS
		return enumerable.OrderBy(x => Guid.NewGuid());
	}
}
