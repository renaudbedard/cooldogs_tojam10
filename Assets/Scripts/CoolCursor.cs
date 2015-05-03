﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CoolCursor : MonoBehaviour
{
	public LayerMask Mask;

	void Start()
	{
		
	}

	void Update()
	{
		var clickRay = GetComponentInParent<Camera>().ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay(clickRay.origin, clickRay.direction * 500f);
		RaycastHit clickHit;
		if (Physics.Raycast(clickRay, out clickHit, 500f, Mask))
		{
			Debug.Log(clickHit.collider.gameObject.name);
			transform.position = clickHit.point;
		}
	}
}