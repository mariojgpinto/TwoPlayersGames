using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	#region BUTTONS_CALLBACKS
	public void OnButtonPressed(int id){
		switch(id){
		case 1:
			Application.LoadLevel("01-MemoryGame");
			break;
		case 4:
			Application.LoadLevel("04-Reversi");
			break;
		case 98:
			Application.LoadLevel("98-TurnSquaresGame");
			break;
		case 99:
			Application.LoadLevel("99-TurnSquares");
			break;
		default: break;
		}
	}
	#endregion

	#region UNITY_CALLBACKS
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnGUI(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}
	#endregion
}
