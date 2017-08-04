using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour 
{
	// Singleton instance
	public static InputManager instance;

	// Dictionary for holding keycodes with our own string names
	Dictionary<string, KeyCode> controls;

	// Add two default input axis
	public Axis horizontalAxis;
	public Axis verticalAxis;

	// Initialization
	void Awake() 
	{
		// Check for if we are violating the singleton
		if(instance != null) 
		{
			// There can only be one
			Destroy(this);
		} 
		else
		{
			// Set instance
			instance = this;
		}
	}

	// Use this for initialization
	void Start () 
	{
		// Initialzie keys
		this.initializeControls();
	}

	private void initializeControls() 
	{
		// Attempt to load from saved preferences file
		this.controls = this.loadControlLayout();

		// If we return with an empty control layout, then we add instead the default values
		if(this.controls.Count == 0) 
		{
			// Default movement controls
			this.controls["Up"] = KeyCode.W;
			this.controls["Down"] = KeyCode.S;
			this.controls["Left"] = KeyCode.A;
			this.controls["Right"] = KeyCode.D;

			// Alternative movement controls
			this.controls["alt_Up"] = KeyCode.UpArrow;
			this.controls["alt_Down"] = KeyCode.DownArrow;
			this.controls["alt_Left"] = KeyCode.LeftArrow;
			this.controls["alt_Right"] = KeyCode.RightArrow;

			// Mouse interaction controls
			this.controls["Shoot"] = KeyCode.Mouse0;
		}

		// Initialize horizontal input axis
		this.horizontalAxis = new Axis(this.controls["Right"], this.controls["Left"], 
			this.controls["alt_Right"], this.controls["alt_Left"]);

		// Initialize vertical input axis
		this.verticalAxis = new Axis(this.controls["Up"], this.controls["Down"],
			this.controls["alt_Up"], this.controls["alt_Down"]);
	}

	public bool GetKey(string keyName) 
	{
		// Check if the key exists
		if(this.controls.ContainsKey(keyName)) 
		{
			// Then we do have the key in the control layout
			return Input.GetKey(this.controls[keyName]);
		}

		// We do not have the key registered in our layout
		return false;
	}


	public bool GetKeyDown(string keyName) 
	{
		// Check if the key exists
		if(this.controls.ContainsKey(keyName)) 
		{
			// Then we do have the key in the control layout
			return Input.GetKeyDown(this.controls[keyName]);
		}

		// We do not have the key registered in our layout
		return false;
	}

	private Dictionary<string, KeyCode> loadControlLayout() 
	{
		// Check if player has key layout already saved
		// TODO: Load key layout from file

		// We did not return before this point, so we should just initialise an empty Dictionary as there was no previous saved layout
		return new Dictionary<string, KeyCode>();
	} 

	private void saveKeyLayout() 
	{
		// TODO: Implement
		return;
	}
}
