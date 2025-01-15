using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratoreNuvoleScript : MonoBehaviour
{

    public GameObject nuvola;
    public float spawnRate;
    public float velocita;
    private float timer;
    private List<GameObject> nuvoleInstanziate;

    // Start is called before the first frame update
    void Start()
    {
        nuvoleInstanziate = new List<GameObject>();
        nuvoleInstanziate.Add(Instantiate(nuvola, new Vector3(11, Random.Range(0f, 6f)-2f, 1), Quaternion.identity));
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            nuvoleInstanziate.Add(Instantiate(nuvola, new Vector3(11, Random.Range(0f, 6f) - 2f, 1), Quaternion.identity));
            timer = 0;
        }

        for (int i = 0; i < nuvoleInstanziate.Count; i++)
        {
            GameObject obj = nuvoleInstanziate[i];
            obj.transform.position += Vector3.left * velocita * Time.deltaTime;

            float xPos = obj.transform.position.x;
            if (xPos < -11)
            {
                nuvoleInstanziate.RemoveAt(i);
                Destroy(obj);
                i--; // Decrementa indice dopo rimozione
            }
        }
    }
}
