using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Flow : MonoBehaviour
{
	public enum Phase
	{
		WaitingForStart,
		CustomerEnter,
		CustomerSpeech,
		Prepare,
		GiveOut,
		Verdict,
		CustomerLeave,
		Lose
	}

	public Order CurrentOrder;
	public Phase CurrentPhase;

	[SerializeField]
	CustomerSpawner customerSpawner;

	void Start() {
	}

	void Update() {
		switch (CurrentPhase) {
		case Phase.CustomerEnter:
					
			break;
		default:
			break;
		}
	}
}
