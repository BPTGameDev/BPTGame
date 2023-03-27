using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CovidSpawner : MonoBehaviour
{
   public float spawnDistance = 11.0f;
    public float trajectoryVariance = 15.0f;
    //public GameObject covidPrefab;
    public Transform player;
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public float spawnRate = 2.0f;
    public int maxSpawnAmount = 10;
    public int spawnAmount = 1;
    public int currSpawnAmount;
    private float startTime = 0f;
    public void Start() {
        startTime = Time.time;
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }
    void Update() {
        float duration = Time.time - startTime;
        float coeff = Mathf.Min(1f, duration/60f);
        currSpawnAmount = Mathf.RoundToInt(coeff*(maxSpawnAmount) + (1-coeff) * spawnAmount);
    }

    public void Spawn() {
        for (int i = 0; i < this.currSpawnAmount; i++) {
            Vector2 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnpoint = this.spawnDistance * spawnDirection + (Vector2)Camera.main.transform.position;


            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            int enemyIndex = Random.Range(0, 3);
            GameObject covidobj = PhotonNetwork.Instantiate(enemyPrefabs[enemyIndex].name, spawnpoint, rotation);
            //GameObject covidobj = PhotonNetwork.Instantiate(this.covidPrefab.name, spawnpoint, rotation);

            int variant = Random.Range(0, 3);
            Covid covid = covidobj.GetComponent<Covid>();
            
            covid.getPlayer(player);
            covid.SetVariant(variant);
            covid.size = Random.Range(3, 6);
            //covid.size = Random.Range(covid.minSize, covid.maxSize);
            covid.setTrajectory(rotation * -spawnDirection);

            /*Covid covid = covidobj.GetComponent<Covid>();
            covid.size = Random.Range(covid.minSize, covid.maxSize);
            covid.setTrajectory(rotation * -spawnDirection);*/
        }
    }
}
