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
        // Instanzia il primo pezzo della mappa (es. l’inizio)
        oggettiInstanziati.Add(Instantiate(livello1[0], new Vector2(0, -4.28f), Quaternion.identity));
        size = 2.88f;
        // Instanzia il secondo pezzo
        oggettiInstanziati.Add(Instantiate(livello1[UnityEngine.Random.Range(0, 2) + 1],
                                          new Vector3(size * 8 / 2 + size / 2, -4.28f, 0),
                                          Quaternion.identity));
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
        oggettiInstanziati.Add(Instantiate(livello1[UnityEngine.Random.Range(0, 2) + 1],
                                          new Vector3(size * 8 / 2 + size / 2, -4.28f, 0),
                                          Quaternion.identity));
        AggiornaLivello(oggettiInstanziati[1]);
    }

    void Update()
    {
        // Muove tutti gli oggetti instanziati
        for (int i = 0; i < oggettiInstanziati.Count; i++)
        {
            GameObject obj = oggettiInstanziati[i];
            obj.transform.position += Vector3.left * velocita * Time.deltaTime;

            float xPos = obj.transform.position.x;
            if ((obj.name == "Inizio(Clone)" && xPos < -31) || (obj.name != "Inizio(Clone)" && xPos < -21))
            {
                oggettiInstanziati.RemoveAt(i);
                Destroy(obj);
                i--; // Decrementa l'indice dopo la rimozione
            }
        }

        GameObject lastInput = oggettiInstanziati[oggettiInstanziati.Count - 1];
        float lastX = lastInput.transform.position.x;

        // Se siamo vicini al bordo destro, aggiungiamo un nuovo pezzo
        if (lastX <= size * 8 / 2)
        {
            GameObject prefabCandidate = null;
            int tries = 0;
            // Seleziona un prefab in base al livello corrente
            // (il ciclo evita di scegliere lo stesso prefab se gli ultimi due oggetti sono già di quel tipo)
            do
            {
                if (livello == 1)
                {
                    int index = UnityEngine.Random.Range(0, 2) + 1;
                    prefabCandidate = livello1[index];
                }
                else if (livello == 2)
                {
                    int index = UnityEngine.Random.Range(0, 3);
                    prefabCandidate = livello2[index];
                }
                else if (livello == 3)
                {
                    int index = UnityEngine.Random.Range(0, 2);
                    prefabCandidate = livello3[index];
                }
                tries++;
                if (tries > 10) break; // Prevenzione di eventuali loop infiniti
            }
            // Se esistono almeno due oggetti istanziati, controlla se gli ultimi due sono dello stesso prefab candidate
            while (oggettiInstanziati.Count >= 2 &&
                   IsSamePrefab(oggettiInstanziati[oggettiInstanziati.Count - 1], prefabCandidate) &&
                   IsSamePrefab(oggettiInstanziati[oggettiInstanziati.Count - 2], prefabCandidate));

            // Instanzia il prefab selezionato
            GameObject toAdd = Instantiate(prefabCandidate, new Vector3(lastX + size, -4.28f, 0), Quaternion.identity);
            AggiornaLivello(toAdd);

            // Se il pezzo è "TerrenoPiatto" aggiunge una mela
            if (toAdd.name.Replace("(Clone)", "").Equals("TerrenoPiatto"))
            {
                GameObject nuovaMela = Instantiate(mela,
                    toAdd.transform.position + new Vector3(0, this.livello * 1.4f, 0),
                    Quaternion.identity);
                nuovaMela.transform.SetParent(toAdd.transform);
            }
            oggettiInstanziati.Add(toAdd);
        }
    }

    /// <summary>
    /// Confronta il prefab "originario" del GameObject istanziato con il prefab candidato.
    /// Viene rimosso la stringa "(Clone)" per confrontare solo il nome originale.
    /// </summary>
    private bool IsSamePrefab(GameObject instantiated, GameObject prefab)
    {
        return instantiated.name.Replace("(Clone)", "").Equals(prefab.name);
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
