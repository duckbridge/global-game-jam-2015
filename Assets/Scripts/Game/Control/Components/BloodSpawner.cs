using UnityEngine;
using System.Collections;

public class BloodSpawner : MonoBehaviour {

	public float minimumBloodSpawnForce, maximumBloodSpawnForce;
	public int minimumAmountOfBloodToSpawn, maximumAmountOfBloodToSpawn;
	public float minimumHorizontalDirection, maximumHorizontalDirection;
	public float minimumVerticalDirection, maximumVerticalDirection;

	public Blood bloodSpawnPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnBlood() {
		for(int i = 0; i < Random.Range (minimumAmountOfBloodToSpawn, maximumAmountOfBloodToSpawn); i++) {
			Blood spawnedBlood = (Blood) GameObject.Instantiate(bloodSpawnPrefab, this.transform.position, Quaternion.identity);
			spawnedBlood.OnSpawn(new Vector3(Random.Range (minimumHorizontalDirection, maximumHorizontalDirection)
			                                 ,Random.Range (minimumVerticalDirection, maximumVerticalDirection)),
			                     Random.Range (minimumBloodSpawnForce, maximumBloodSpawnForce));
		}
	}
}
