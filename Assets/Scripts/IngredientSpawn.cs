using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class IngredientSpawn : MonoBehaviour
{
	public void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position, 0.1f);
	}

	void Start()
	{
		var obj = Instantiate(Ingredients.Instance.SnackTemplate, transform.position, Quaternion.AngleAxis(60, Vector3.right)) as GameObject;
		obj.GetComponent<SpriteRenderer>().sprite = Ingredients.Instance.Sprites.Shuffle().First();
	}
}
