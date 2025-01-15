using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicManagerScript : MonoBehaviour
{

    public GameObject menu;
    public GameObject impostazioni;

    public int playerScore;

    public GameObject personaggio;
    public GameObject gameStartScreen;
    public GameObject punteggio;

    public GameObject gameOverScreen;
    public GeneratoreMappaScript generatoreMappaScript;

    public GameObject pauseButton;

    public Slider soundSlider;
    public Slider musicSlider;
    public AudioSource jumpSound;
    public AudioSource eatSound;
    public AudioSource musicSound;


    void Start()
    {

        //imposta record
        punteggio.SetActive(true);
        punteggio.GetComponent<Text>().fontSize = 64;
        punteggio.GetComponent<Text>().text = "Record: "+GetRecord();


        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);

        // Imposta il valore iniziale degli slider
        musicSlider.value = 1f;
        soundSlider.value = 1f;

        // Applica il volume salvato
        eatSound.volume = soundSlider.value;
        musicSound.volume = musicSlider.value;

        // Aggiungi listener per aggiornare il volume quando lo slider viene modificato
        musicSlider.onValueChanged.AddListener(delegate { UpdateMusicVolume(); });
        soundSlider.onValueChanged.AddListener(delegate { UpdateSoundVolume(); });
    }

    public void UpdateMusicVolume()
    {
        musicSound.volume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void UpdateSoundVolume()
    {
        jumpSound.volume = soundSlider.value;
        eatSound.volume = soundSlider.value;
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
    }

    [ContextMenu("Increase Score")]
    public void addScore()
    {
        eatSound.Play();
        playerScore++;
        punteggio.GetComponent<Text>().fontSize = 196;
        punteggio.GetComponent<Text>().text = playerScore.ToString();
    }

    public void startGame()
    {
        generatoreMappaScript.velocita = 2;
        gameStartScreen.SetActive(false);
        personaggio.SetActive(true);
        punteggio.GetComponent<Text>().fontSize = 196;
        punteggio.GetComponent<Text>().text = playerScore.ToString();
        pauseButton.GetComponentInChildren<Text>().fontSize = 64;
        pauseButton.GetComponentInChildren<Text>().text = "Pausa";
    }

    public void pauseGame()
    {
        menu.SetActive(false);
        impostazioni.SetActive(true);
        generatoreMappaScript.pause();
        personaggio.GetComponent<Animator>().enabled = false;
        personaggio.GetComponent<Rigidbody2D>().simulated = false;
    }

    public void playGame()
    {
        impostazioni.SetActive(false);
        menu.SetActive(true);
        if(personaggio.active && personaggio.GetComponent<PersonaggioScript>().personaggioIsAlive){
            generatoreMappaScript.velocita = 2;
        }
        personaggio.GetComponent<Animator>().enabled = true;
        personaggio.GetComponent<Rigidbody2D>().simulated = true;
    }

    public void homePage()
    {
        gameOverScreen.SetActive(false);
        gameStartScreen.SetActive(true);
        personaggio.SetActive(false);
        personaggio.GetComponent<PersonaggioScript>().Reborn();
        punteggio.GetComponent<Text>().fontSize = 64;
        punteggio.GetComponent<Text>().text = "Record: " + GetRecord();
        playerScore = 0;
        generatoreMappaScript.restart();
    }

    public void restartGame()
    {
        gameOverScreen.SetActive(false);
        generatoreMappaScript.restart();
        generatoreMappaScript.velocita = 2;
        personaggio.GetComponent<PersonaggioScript>().Reborn();
        playerScore = 0;
        punteggio.GetComponent<Text>().fontSize = 196;
        punteggio.GetComponent<Text>().text = playerScore.ToString();
        pauseButton.GetComponentInChildren<Text>().fontSize = 64;
        pauseButton.GetComponentInChildren<Text>().text = "Pausa";
    }

    public void gameOver()
    {
        personaggio.GetComponent<Animator>().enabled = false;
        pauseButton.GetComponentInChildren<Text>().fontSize = 40;
        pauseButton.GetComponentInChildren<Text>().text = "Impostazioni";
        gameOverScreen.SetActive(true);
        generatoreMappaScript.velocita = 0;
        SaveRecord(playerScore);
    }

    public void SaveRecord(int newRecord)
    {
        int currentRecord = PlayerPrefs.GetInt("HighScore", 0); // Ottieni il record attuale, di default 0
        if (newRecord > currentRecord)
        {
            PlayerPrefs.SetInt("HighScore", newRecord); // Salva il nuovo record
            PlayerPrefs.Save(); // Assicurati che i dati siano salvati
            Debug.Log("Nuovo record salvato: " + newRecord);
        }
    }

    public int GetRecord()
    {
        return PlayerPrefs.GetInt("HighScore", 0); // Ottieni il record salvato
    }
}
