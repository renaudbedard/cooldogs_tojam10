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

	public static Phase CurrentPhase;
	
	CameraMover cameraMover;
	SnackPicker snackPicker;

	[SerializeField]
	GameObject garageDoor;
	[SerializeField]
	SkyColours sky;
	Vector3 garageClosedPosition;
	Vector3 garageOpenPosition;

	public int Failures;
	public int Successes;

	void Start() {
		garageClosedPosition = garageDoor.transform.position;
		garageOpenPosition = garageDoor.transform.position;
		garageOpenPosition.y += 6f;

		cameraMover = GetComponent<CameraMover>();
		snackPicker = GetComponent<SnackPicker>();

		CurrentPhase = StartingPhase;
	}

	void Update() {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        if (Input.GetKeyDown (KeyCode.Escape)) {
			ResetGame();
		}
#endif

        switch (CurrentPhase) {
		case Phase.WaitingForStart:
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0)) {
#else
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended) {
#endif

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
            Rect screenRect = new Rect(0, 0, Screen.width, Screen.height/4);
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonUp(0) && screenRect.Contains(Input.mousePosition))) {
#else
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended && screenRect.Contains(Input.touches[0].position)) {
#endif
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
			BoxCollider cc = Customers.Instance.currentCustomer.GetComponent<BoxCollider>();
			if (cc != null){
				Destroy(cc);
			}
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0) && snackPicker.ClickedPlate())) {
#else
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended && snackPicker.ClickedPlate()) {
#endif
                    Customers.Instance.ServeCustomer(snackPicker.hasSnacksOnPlate);
				sky.ChangeColour();
			}
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
