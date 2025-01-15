using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GeneratoreMappaScript : MonoBehaviour
{
    public GameObject[] livello1;
    public GameObject[] livello2;
    public GameObject[] livello3;

    public GameObject mela;

    public int velocita;
    private int livello;
    private List<GameObject> oggettiInstanziati;
    private float size;
    void Start()
    {
        livello = 1;
        oggettiInstanziati = new List<GameObject>();
        oggettiInstanziati.Add(Instantiate(livello1[0], new Vector2(0, -4.28f), Quaternion.identity));
        size = 2.88f;
        oggettiInstanziati.Add(Instantiate(livello1[UnityEngine.Random.Range(0, 2) + 1], new Vector3(size*8/2+size/2, -4.28f, 0), Quaternion.identity));
        AggiornaLivello(oggettiInstanziati[1]);
    }

    public void restart()
    {
        livello = 1;
        for (int i = oggettiInstanziati.Count - 1; i >= 0; i--)
        {
            GameObject gameObject = oggettiInstanziati[i];
            oggettiInstanziati.RemoveAt(i);
            Destroy(gameObject);
        }
        oggettiInstanziati.Add(Instantiate(livello1[0], new Vector2(0, -4.28f), Quaternion.identity));
        oggettiInstanziati.Add(Instantiate(livello1[UnityEngine.Random.Range(0, 2) + 1], new Vector3(size * 8 / 2 + size / 2, -4.28f, 0), Quaternion.identity));
        AggiornaLivello(oggettiInstanziati[1]);
    }


    void Update()
    {
        for (int i = 0; i < oggettiInstanziati.Count; i++)
        {
            GameObject obj = oggettiInstanziati[i];
            obj.transform.position += Vector3.left * velocita * Time.deltaTime;

            float xPos = obj.transform.position.x;
            if ((obj.name == "Inizio(Clone)" && xPos < -31) || (obj.name != "Inizio(Clone)" && xPos < -21))
            {
                oggettiInstanziati.RemoveAt(i);
                Destroy(obj);
                i--; // Decrementa indice dopo rimozione
            }
        }

        GameObject lastInput = oggettiInstanziati[oggettiInstanziati.Count - 1];
        float lastX = lastInput.transform.position.x;
        GameObject toAdd = null;

        if (lastX<=size*8/2)
        {
            if (livello == 1)
                toAdd = Instantiate(livello1[UnityEngine.Random.Range(0, 2) + 1], new Vector3(lastX + size, -4.28f, 0), Quaternion.identity);
            else if (livello == 2)
                toAdd = Instantiate(livello2[UnityEngine.Random.Range(0, 3)], new Vector3(lastX + size, -4.28f, 0), Quaternion.identity);
            else if (livello == 3)
                toAdd = Instantiate(livello3[UnityEngine.Random.Range(0, 2)], new Vector3(lastX + size, -4.28f, 0), Quaternion.identity);

            AggiornaLivello(toAdd);

            if (toAdd.name == "TerrenoPiatto(Clone)")
            {
                GameObject nuovaMela = Instantiate(mela, toAdd.transform.position + new Vector3(0, this.livello * 1.4f, 0), Quaternion.identity);
                nuovaMela.transform.SetParent(toAdd.transform);
            }
            oggettiInstanziati.Add(toAdd);
        }
    }

    private void AggiornaLivello(GameObject toAdd)
    {
        if (toAdd.name == "Salita(Clone)" || toAdd.name == "Salita")
        {
            livello++;
            print(livello);
        }
        else if (toAdd.name == "Discesa(Clone)" || toAdd.name == "Discesa")
        {
            livello--;
            print(livello);
        }
    }

    public void pause()
    {
        velocita = 0;
        for (int i = 0; i < oggettiInstanziati.Count; i++)
        {
            if (transform.childCount > 0)
            {
                GameObject mela = transform.GetChild(0).gameObject;
                mela.GetComponent<Animator>().enabled = false;
            }
        }
    }

    public void play()
    {
        velocita = 2;
        for (int i = 0; i < oggettiInstanziati.Count; i++)
        {
            if (transform.childCount > 0)
            {
                GameObject mela = transform.GetChild(0).gameObject;
                mela.GetComponent<Animator>().enabled = true;
            }
        }
    }
}
