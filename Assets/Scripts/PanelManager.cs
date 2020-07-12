using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public int timeBetweenSpawn = 2;

    private RectTransform uiTrans;
    private char lastKey;

    public GameObject[] events;
    public GameObject[] spawnPoints;

    void Start()
    {
        uiTrans = GetComponentInParent<RectTransform>();
        // StartCoroutine("SpawnEvents");

        // TODO: Stretch this object's width to match that of UiTrans
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            lastKey = 'A';
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            lastKey = 'S';
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            lastKey = 'D';
        }
    }

    private IEnumerator SpawnEvents()
    {
        while(true) // eventually replace with if game is active
        {
            yield return new WaitForSeconds(timeBetweenSpawn);
            Instantiate(events[Random.Range(0, events.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].transform);
        }
    }

    private void OnTriggerEnter(Collider other) // Effect hit 
    {
        if(lastKey != 'A') // && other.gameObject.transform.y > NUMBER
        {
            Debug.Log("Collision with A");
            Destroy(other.gameObject);
        }
        else if(lastKey != 'S') // && other.gameObject.transform.y < NUMBER && other.gameObject.transform.y > NUMBER
        {
            Debug.Log("Collision with S");
            Destroy(other.gameObject);
        }
        else if(lastKey != 'D') // && other.gameObject.transform.y < NUMBER
        {
            Debug.Log("Collision with D");
            Destroy(other.gameObject);
        }
    }

    // A BUNCH OF FUNCTIONS FOR OnTriggerEnter TO INVOKE
}
