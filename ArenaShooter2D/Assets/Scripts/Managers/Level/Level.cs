using UnityEngine;

[System.Serializable]
public class Level {
	public string name;
	public string fileName;
	public string bgm;
	public CharacterToLevelTilePrefab[] availableLevelTiles;
	
	public int length = 6;

	public int height = 3;
}
