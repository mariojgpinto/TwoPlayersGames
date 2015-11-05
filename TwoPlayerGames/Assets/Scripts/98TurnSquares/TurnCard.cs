using UnityEngine;
using System.Collections;

public class TurnCard : MonoBehaviour {
	#region VARIABLES
	public bool isBack = false;
	public bool isActive = true;

	public bool isBusy = false;

	public int posX;
	public int posY;
	Vector3 initialScale = Vector3.one * .2f;
	#endregion

	#region SETUP
	public void Init(int x, int y){
		posX = x;
		posY = y;
		isActive = true;
	}

	public void ResetCard(){
		this.transform.localScale = initialScale;
		isActive = true; 
		isBusy = false;
	}
	#endregion

	#region ACTIONS
	public void FlipCard(float waitTime = 0){
		if (!isBusy){
			isBusy = true;
			isBack = !isBack;
			StartCoroutine(FlipCard_routine(waitTime));
		}
	}

	public void HideCard(){
		if (!isBusy){
			StartCoroutine(HideCard_routine());
		}
	}

	public void SetWhite(){
		isBack = false;
		this.transform.rotation = Quaternion.Euler(0,180,0);
	}

	public void SetBlack(){
		isBack = true;
		this.transform.rotation = Quaternion.Euler(0,0,0);
	}
	#endregion

	#region ACTIONS_AUXILIAR
	IEnumerator FlipCard_routine(float waitTime){
		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.1
		float duration = .5f;
		Vector3 initialRotation = this.transform.rotation.eulerAngles;

		yield return new WaitForSeconds(waitTime);

		while(progress < 1)
		{
			this.transform.rotation = Quaternion.Euler(initialRotation + (Vector3.up * 180 * progress));
			
			if(progress < .5f)
				this.transform.localScale = (initialScale) + (initialScale * .5f) * progress;
			else
				this.transform.localScale = (initialScale) + (initialScale * .5f) * (1 - progress);
			
			progress += Time.deltaTime/duration;
			yield return true;//new WaitForSeconds(smoothness);
		}
		
		this.transform.rotation = Quaternion.Euler(initialRotation + (Vector3.up * 180));
		this.transform.localScale = initialScale;
		
		isBusy = false;
		
		yield break;
	}

	IEnumerator HideCard_routine(){
		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.1
		float duration = .5f;

		isBusy = true;
		isActive = false;
		
		while(progress < 1)
		{
			this.transform.localScale = initialScale * (1-progress);
			
			progress += Time.deltaTime/duration;
			yield return true;//new WaitForSeconds(smoothness);
		}

		this.transform.localScale = Vector3.zero;
		
		isBusy = false;

		SetBlack();
		
		yield break;
	}
	#endregion
	
	#region UNITY_CALLBACKS
	// Use this for initialization
//	void Start () {
//		SetBlack();
//	}
	#endregion
}
