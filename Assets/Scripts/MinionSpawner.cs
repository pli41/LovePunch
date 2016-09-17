using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviour {

	public void Spawn(GameObject minion)
    {
        
		Debug.Log ("Minion created. Current Number: " + GameManager.existingMinions.Count);
		GameObject createdMinion = Instantiate(minion, transform.position, transform.rotation) as GameObject;
		GameManager.existingMinions.Add(createdMinion);
    }
}
