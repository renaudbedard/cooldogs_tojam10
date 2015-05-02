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
}
