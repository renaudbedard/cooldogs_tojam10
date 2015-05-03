using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour {
	public void OnDrawGizmos() {
		Gizmos.DrawSphere(transform.position, 0.1f);
	}
}
