using System.Collections.Generic;
using UnityEngine;
using KModkit;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;
using Rnd = UnityEngine.Random;

public class SimpleModuleScript : MonoBehaviour {

	public KMAudio audio;
	public KMBombInfo info;
	public KMBombModule module;
	public KMSelectable[] competitions;
	public KMSelectable[] AudioButtons;
	static int ModuleIdCounter = 1;
	int ModuleId;

	public TextMesh[] screenTexts;

	public AudioSource bfb;
	public AudioSource tpot;

	public int textMessage1;
	public string textFinder1;

	bool _isSolved = false;
	bool incorrect = false;


	void Awake() 
	{
		ModuleId = ModuleIdCounter++;

		foreach (KMSelectable button in competitions)
		{
			KMSelectable pressedButton = button;
			button.OnInteract += delegate () { competitionDecider(pressedButton); return false; };
		}

		foreach (KMSelectable button in AudioButtons)
		{
			KMSelectable pressedButton = button;
			button.OnInteract += delegate () { audioButtonPresses(pressedButton); return false; };
		}
	}

	void Start ()
	{
		textMessage1 = Rnd.Range(1, 55);
		for (int i = 0; i < textMessage1; i++)
			textFinder1 = textMessage1.ToString();
		screenTexts[0].text = textFinder1;

		Debug.LogFormat("You are contestant no {0}", textMessage1);
	}

	void audioButtonPresses(KMSelectable pressedButton)
	{
		GetComponent<KMAudio>().PlayGameSoundAtTransformWithRef(KMSoundOverride.SoundEffect.ButtonPress, transform);
		int buttonPosition = new int();
		for(int i = 0; i < AudioButtons.Length; i++)
		{
			if (pressedButton == AudioButtons[i])
			{
				buttonPosition = i;
				break;
			}
		}

		switch (buttonPosition) 
		{
		case 0:
			bfb.Play ();
			break;
		case 1:
			tpot.Play ();
			break;
		}
	}

	void competitionDecider(KMSelectable pressedButton)
	{
		GetComponent<KMAudio>().PlayGameSoundAtTransformWithRef(KMSoundOverride.SoundEffect.ButtonPress, transform);
		int buttonPosition = new int();
		for(int i = 0; i < competitions.Length; i++)
		{
			if (pressedButton == competitions[i])
			{
				buttonPosition = i;
				break;
			}
		}
		if (_isSolved == false) 
		{
			switch (buttonPosition) 
			{
			case 0:
				if (textMessage1 > 14) 
				{
					incorrect = true;
					Log ("BFB is not for you!");
				}
				break;
			case 1:
				if (textMessage1 < 15) 
				{
					incorrect = true;
					Log ("TPOT is not for you!");
				}
				break;
			}
			if (incorrect) 
			{
				module.HandleStrike ();
				Log ("You entered the wrong side!");
				incorrect = false;
			}
			else 
			{
				module.HandlePass ();
				Log ("You chose the wise competition!");
				_isSolved = true;
			}
		}
	}

	void Log(string message)
	{
		Debug.LogFormat("[JacknJellify #{0}] {1}", ModuleId, message);
	}
}

