﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class LevelManager : MonoBehaviour {
	// Singleton instance
	public static LevelManager instance;

	// Levels in game
	public Level[] levels;

	// Current level being worked on
	public Level currentLevel;

	// Empty game object container for level
	private Transform levelContainer;

	// The level file read into a list of character arrays
	List<char[]> currentCharacterLevelMap;

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

	void Update() 
	{
		Debug.Log("Do not have level container");
		// We need to find the levelContainer if we have not already
		if(!this.levelContainer) 
		{
			Debug.Log("Do not have level container");
			// We do not have levelContainer, attempt to find it
			this.levelContainer = GameObject.Find("Level").transform;
			if(this.levelContainer) {
				Debug.Log("We have level container now");
			}
		}
	}

	public bool CanLoadMap() 
	{
		if(this.levelContainer != null) {
			return true;
		}

		return false;
	}

	public void LoadMap(string levelName)
	{
		// First empty the current map
		this.EmptyMap();

		// Set current level
		this.SetLevelByName(levelName);

		// Get file path to character map file
		string filePath = Application.dataPath + "/StreamingAssets/" + this.currentLevel.fileName;

		// Then get the characters from the level text file
		this.currentCharacterLevelMap = this.getCharactersFromTextFile(filePath);

		// Spawn tiles for the correct characters
		for (int y = 0; y < this.currentCharacterLevelMap.Count(); y++) 
		{
			// Go through each character array in the list
			char[] currentCharacterLine = this.currentCharacterLevelMap.ElementAt(y);

			for (int x = 0; x < currentCharacterLine.Length; x++) 
			{
				// Spawn a tile for every character in every character array
				// Due to the file having been read top down, the spawning of tiles will be upside down, that is why we need to flip it by subtracting y by the row count of the character map
				this.SpawnTileAt (currentCharacterLine [x], x, this.currentCharacterLevelMap.Count() - 1 - y);
			}
		}
	}

	private void SetLevelByName(string levelname) 
	{
		for(int i = 0; i < this.levels.Count(); i++) 
		{
			if(this.levels[i].name == levelname) 
			{
				this.currentLevel = this.levels[i];
				return;
			}
		}

		Debug.Log("Did not find level with name: " + levelname);
		return;
	}

	List<char[]> getCharactersFromTextFile(string filePath) 
	{
		// Our results from the file will be a list of character arrays, going from the top line of the text file to the bottom
		List<char[]> result = new List<char[]>();

		foreach (string line in File.ReadAllLines(filePath)) 
		{
			result.Add (line.ToCharArray());
		}

		// return the list
		return result;
	}

	void SpawnTileAt(char c, int x, int y) 
	{
		// If c is 'A' then it designates "Air" and should be empty
		if (c == 'A' || c == ' ') 
		{
			return;
		}

		// Check if we have the player
		if (c == 'P') 
		{
			// Move the preset player object to the position determined in the map
			GameObject player = GameObject.Find ("Player");
			player.transform.position = new Vector3 (x, y, 0);
			return;
		}

		// Find the right character in the character to prefab mapping
		foreach (CharacterToLevelTilePrefab ctp in this.currentLevel.availableLevelTiles) 
		{
			// If we found a matching character
			if (ctp.character == c)
			{
				GameObject go = (GameObject)Instantiate (ctp.levelTile, new Vector3 (x, y, 0), Quaternion.identity);
				// If we are did not instantiate the player object then we want to set the instantiated object as a child of the level object
				go.transform.parent = this.levelContainer;
				// Also rename the object to keep it seperate from other cloned instances of the prefab

				// Could possibly do more to the object
				return;
			}
		}

		// No matching character in the character to prefab mapping
		Debug.LogError("No character to prefab found for: " + c);
	}

	void EmptyMap() 
	{
		// Find the children of the level empty and destroy their game objects
		while (this.levelContainer.childCount > 0) {
			Transform c = this.levelContainer.GetChild (0);
			c.SetParent (null);
			Destroy (c.gameObject);
		}
	}
}