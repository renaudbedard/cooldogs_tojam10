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
		obj.GetComponentInChildren<SpriteRenderer>().sprite = Ingredients.Instance.Sprites.First(x => x.name == ingredientName);
		obj.transform.parent = transform;
		obj.transform.localRotation = Quaternion.AngleAxis(90, Vector3.right);
		obj.GetComponent<Snack>().snackType = ingredientName;
		obj.GetComponent<Snack>().Initialize();

		lastSpawn = obj;
	}
}
