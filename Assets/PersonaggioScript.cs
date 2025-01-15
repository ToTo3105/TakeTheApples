using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PersonaggioScript : MonoBehaviour
{

    public float forzaSalto;
    public float tempoLimiteDoppioSalto;

    private LogicManagerScript LogicManagerScript;
    private Rigidbody2D rigidBody;
    private float tempoDalSalto;
    private bool isGrounded;
    public bool personaggioIsAlive;
    private bool doppioSalto;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        personaggioIsAlive = true;
        rigidBody = GetComponent<Rigidbody2D>();
        LogicManagerScript = GameObject.FindGameObjectWithTag("LogicManager").GetComponent<LogicManagerScript>();
    }

    public void Reborn()
    {
        personaggioIsAlive = true;
        transform.position = new Vector3(0,0,0);
        transform.rotation = Quaternion.identity;
        GetComponent<Animator>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(transform.position.x < -7.5)
        {
            personaggioIsAlive = false;
            LogicManagerScript.gameOver();
        }

        bool isUITouched = EventSystem.current.IsPointerOverGameObject();
        bool jumpPressed = false;

        // Gestione dell'input del salto
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                jumpPressed = true;
            }
        }
        else if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            jumpPressed = true;
        }

        // Evita input su interfacce grafiche
        jumpPressed = jumpPressed && !isUITouched;

        // Salto singolo
        if (isGrounded && jumpPressed && personaggioIsAlive)
        {
            audioSource.Play();
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, forzaSalto);
            isGrounded = false; // Imposta come non a terra
            doppioSalto = true; // Abilita il doppio salto
            tempoDalSalto = 0; // Resetta il timer del salto
        }
        // Salto doppio
        else if (!isGrounded && jumpPressed && tempoDalSalto < tempoLimiteDoppioSalto && doppioSalto && personaggioIsAlive)
        {
            audioSource.Play();
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y + forzaSalto); // Velocità del salto
            doppioSalto = false; // Disabilita il doppio salto
        }

        // Incrementa il timer del salto
        tempoDalSalto += Time.deltaTime;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
        float xPosition = transform.position.x;
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180)
            zRotation -= 360;

        if (zRotation > 75 || zRotation < -75){
            personaggioIsAlive = false;
            LogicManagerScript.gameOver();
        }
    }

}
