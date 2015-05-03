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
		Waiting,
		CustomerEnter,
		CustomerSpeech,
		Prepare,
		GiveOut,
		Verdict,
		CustomerLeave,
		Lose
	}

	public Order CurrentOrder;
	public static Phase CurrentPhase;
	
	CameraMover cameraMover;

	[SerializeField]
	GameObject garageDoor;
	Vector3 garageClosedPosition;
	Vector3 garageOpenPosition;

	void Start() {
		garageClosedPosition = garageDoor.transform.position;
		garageOpenPosition = garageDoor.transform.position;
		garageOpenPosition.y += 6f;

		cameraMover = GetComponent<CameraMover>();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			ResetGame();
		}
		switch (CurrentPhase) {
		case Phase.WaitingForStart:
			if (Input.GetKeyDown(KeyCode.Space)) {
				CurrentPhase = Phase.Waiting;
				iTween.MoveTo(garageDoor, iTween.Hash("position", garageOpenPosition,
				                                      "easeType", "easeInOutQuad",
				                                      "time", 3.5f,
				                                      "oncomplete", "GarageDoorOpenFinish",
				                                      "onCompleteTarget", gameObject));
			}
			break;
		case Phase.Waiting:
			break;
		case Phase.CustomerSpeech:
			if (Input.GetKeyDown(KeyCode.Space)) {
				CurrentPhase = Phase.Prepare;
			}
			break;
		case Phase.CustomerEnter:
			if(!Customers.Instance.doingThings) {
				Customers.Instance.SpawnNextCustomer();
			}
			break;
		case Phase.Prepare: 
			cameraMover.LookingAtWindow = false;
			if (Input.GetKeyDown(KeyCode.Space)) {
				CurrentPhase = Phase.GiveOut;
			}
			break;
		case Phase.GiveOut:
			cameraMover.LookingAtWindow = true;
			CurrentPhase = Phase.Verdict;
			break;
		case Phase.Verdict:
			cameraMover.LookingAtWindow = true;
			CurrentPhase = Phase.CustomerEnter;
			break;
		default:
			break;
		}
	}

	void GarageDoorOpenFinish() {
		CurrentPhase = Phase.CustomerEnter;
	}

	void ResetGame(){
		//Any and all resetting code
		iTween.MoveTo(garageDoor, iTween.Hash("position", garageClosedPosition, "time", 0f));
		cameraMover.LookingAtWindow = true;
		Customers.Instance.doingThings = false;

		CurrentPhase = Phase.WaitingForStart;
	}
}
