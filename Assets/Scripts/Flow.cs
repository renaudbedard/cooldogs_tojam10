using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Flow : MonoBehaviour
{
	public enum Phase
	{
		CustomerSpeech,
		Prepare,
		GiveOut,
		Verdict,
		CustomerLeave
	}

	public Order CurrentOrder;
	public Phase CurrentPhase;

	void Update()
	{
		switch (CurrentPhase)
		{
			case Phase.CustomerSpeech:
				// prepare stuff for customer arrival and speech bubble and stuff
				// when over...

				CurrentPhase = Phase.Prepare;
				break;

			case Phase.Prepare:

				break;
		}
	}
}
