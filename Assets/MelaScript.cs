using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelaScript : MonoBehaviour
{

    private LogicManagerScript LogicManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        LogicManagerScript = GameObject.FindGameObjectWithTag("LogicManager").GetComponent<LogicManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -11)
        {
            Destroy(this.gameObject);
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
        LogicManagerScript.addScore();
    }
}
