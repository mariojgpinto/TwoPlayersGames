using UnityEngine;
using System.Collections;

public class FlipCardScript : MonoBehaviour {
	public bool flag = false;
    
    bool isRotationg = false;

    public void FlipCard()
    {
        if (!isRotationg)
        {
            StartCoroutine(FlipCard_routine());
        }
    }

	IEnumerator FlipCard_routine()
	{
		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.1
		float duration = .5f;
		Vector3 initialRotation = this.transform.rotation.eulerAngles;
        Vector3 initialScale = this.transform.localScale;
       
        isRotationg = true;

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

        isRotationg = false;
		
		yield break;
	}
}
