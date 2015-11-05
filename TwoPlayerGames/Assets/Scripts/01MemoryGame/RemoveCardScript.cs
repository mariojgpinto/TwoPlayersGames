using UnityEngine;
using System.Collections;

public class RemoveCardScript : MonoBehaviour {
	#region VARIABLES
	bool isBusy = false;
	#endregion

	#region REMOVE_CARD
	public void RemoveCard(Vector3 destination){
		if (!isBusy) {
			StartCoroutine(RemoveCard_routine(destination));
		}
	}

	IEnumerator RemoveCard_routine(Vector3 destination){		
		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.1
		float duration = .5f;
		this.transform.position = this.transform.position + Vector3.back;
		Vector3 initialPosition = this.transform.position;
		Vector3 finalPosition = destination;
		finalPosition.z = initialPosition.z;
		Vector3 initialScale = this.transform.localScale;
		
		isBusy = true;
		
		while(progress < 1)
		{
			this.transform.position = Vector3.Lerp(initialPosition,finalPosition, progress);

			this.transform.localScale = initialScale - initialScale * (progress*.8f);
						
			progress += Time.deltaTime/duration;
			yield return true;//new WaitForSeconds(smoothness);
		}
		
		this.transform.position = finalPosition;
		this.transform.localScale = Vector3.zero;

		
		isBusy = false;
		
		yield break;
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
	#endregion
}
