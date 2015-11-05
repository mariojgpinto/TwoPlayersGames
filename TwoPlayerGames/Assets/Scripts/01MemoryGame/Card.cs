using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
	public Material[] materials;
	#region VARIABLES
	[SerializeField] GameObject go_front;
	[SerializeField] GameObject go_back;

	[SerializeField] FlipCardScript s_flip;
	[SerializeField] RemoveCardScript s_remove;

	Vector3 initialPosition;
	Vector3 initialScale;
	Quaternion initialRotation;

	int id;

	public int posX;
	public int posY;
	#endregion

	#region SETUP
	public void SetCard(int _id){
		id = _id;
		go_front.GetComponent<Renderer>().material = materials[_id];
	}

	public int GetID(){
		return id;
	}

	public void ResetStatus(){
		this.transform.position = initialPosition;
		this.transform.localScale = initialScale;
		this.transform.rotation = initialRotation;
	}
	#endregion

	public void SaveInit(){
		initialPosition = this.transform.position;
		initialScale = this.transform.localScale;
		initialRotation = this.transform.rotation;
	}

	#region ACTIONS
	public void TurnCard(){
		s_flip.FlipCard();
	}

	public void RemoveCard(Vector3 position){
		s_remove.RemoveCard(position);
	}
	#endregion

	#region UNITY_CALLBACKS
	// Use this for initialization
//	void Start () {
//		initialPosition = this.transform.position;
//		initialScale = this.transform.localScale;
//		initialRotation = this.transform.rotation;
//	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}
	#endregion
}
