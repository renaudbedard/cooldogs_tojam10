using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class IngredientSpawn : MonoBehaviour
{
	GameObject lastSpawn;

	public void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position, 0.1f);
	}

	public void Spawn(string ingredientName)
	{
		if (lastSpawn)
			Destroy(lastSpawn);

		var obj = Instantiate(Ingredients.Instance.SnackTemplate, transform.position, Quaternion.identity) as GameObject;
        var matches = Ingredients.Instance.Sprites.Where(x => x.name == ingredientName);
        if (matches.Count() == 0)   Debug.LogError("No matching sprite for " + ingredientName);
        if (matches.Count() > 1)    Debug.LogError("Too many matches for " + ingredientName + "(" + matches.Count() + ")");
        obj.GetComponentInChildren<SpriteRenderer>().sprite = matches.First();
		obj.transform.parent = transform;
		obj.transform.localRotation = Quaternion.AngleAxis(90, Vector3.right);
		obj.GetComponent<Snack>().snackType = ingredientName;
		obj.GetComponent<Snack>().Initialize();

		lastSpawn = obj;
	}
}
