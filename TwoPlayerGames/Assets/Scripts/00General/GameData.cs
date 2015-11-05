using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GameData : MonoBehaviour {
	#region VARIABLES
	static GameData instance = null;
	public static PlayerProfile[] players;
	static PlayerProfile player;

	public int data = 0;

	#endregion

	#region DATA_PERSISTENCE
	public static void SaveData(){
		MySerializer.AddToDictionary("Player",player);

		string path = Application.persistentDataPath + "/TwoPlayerGames.dat";
		MySerializer.SaveDictionary(path);
	}
	
	public static bool LoadData(){
		string path = Application.persistentDataPath + "/TwoPlayerGames.dat";
		Debug.Log("Path: " + path);

		if(!MySerializer.LoadDictionary(path))
			return false;

		PlayerProfile cenas = (PlayerProfile)MySerializer.GetFromDictionary("Player");

		if(cenas != null)
			Debug.Log(cenas.ToString());

		return true;
	}
	#endregion

	#region UNITY_CALLBACKS
	// Use this for initialization
	void Start () {
		if(instance == null){
			DontDestroyOnLoad(this);
			instance = this;
		} else if(instance != this){
			DestroyImmediate(this.gameObject);
		}



//		string path = Application.persistentDataPath + "/TwoPlayerGames.dat";
//		Debug.Log("Path: " + path);
//
//		players = new PlayerProfile[2];
//
//		Debug.Log("Start");
//
//		player = new PlayerProfile();
//		player.name = "Mario Pinto";
//		player.age = 26;
//		player.preferedColor = Color.cyan;
//		player.sex = 1;
//		player.hasPhoto = false;
//
//		Debug.Log(player.ToString());
//
//		SaveData();

		LoadData();

	}
	#endregion
}
