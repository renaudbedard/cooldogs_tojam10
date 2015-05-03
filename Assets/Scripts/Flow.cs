using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Flow : MonoBehaviour
{
	public enum Phase {
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

	[SerializeField]
	Phase StartingPhase = Phase.WaitingForStart;

	public Order CurrentOrder;
	public static Phase CurrentPhase;
	
	CameraMover cameraMover;
	SnackPicker snackPicker;

	[SerializeField]
	GameObject garageDoor;
	[SerializeField]
	SkyColours sky;
	Vector3 garageClosedPosition;
	Vector3 garageOpenPosition;

	void Start() {
		garageClosedPosition = garageDoor.transform.position;
		garageOpenPosition = garageDoor.transform.position;
		garageOpenPosition.y += 6f;

		cameraMover = GetComponent<CameraMover>();
		snackPicker = GetComponent<SnackPicker>();

		CurrentPhase = StartingPhase;
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
				                                      "easeType", "easeInQuad",
				                                      "time", 2f,
				                                      "oncomplete", "GarageDoorOpenFinish",
				                                      "onCompleteTarget", gameObject));
			}
			break;
		case Phase.Waiting:
			break;
		case Phase.CustomerSpeech:
			if (Input.GetKeyDown(KeyCode.Space)) {
				CurrentPhase = Phase.Prepare;
				snackPicker.Reset();
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
				//if ()
				CurrentPhase = Phase.GiveOut;
			}
			break;
		case Phase.GiveOut:
			sky.ChangeColour();
			Customers.Instance.ServeCustomer();
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
		snackPicker.Reset();
		CurrentPhase = Phase.WaitingForStart;
	}
}
