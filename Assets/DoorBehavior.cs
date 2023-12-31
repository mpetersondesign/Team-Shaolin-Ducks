using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public bool Activatable;
    public bool Locked;
    public GameObject Lock;
    public DoorBehavior DestinationDoor;
    public Animator ScreenFader;
    Animator DA;
    public GameObject InteractIndicator;
    public AudioCue audioCue;

    private void Start()
    {
        InteractIndicator.SetActive(false);
        Activatable = false;
        DA = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Activatable = true;
            InteractIndicator.SetActive(true);
        }

        if(collision.tag == "Key" && Locked == true)
        {
            Locked = false;
            Lock.SetActive(false);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Activatable = false;
            InteractIndicator.SetActive(false);
        }
    }

    public void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        if (Activatable && Input.GetKeyDown(KeyCode.E) && Locked == false)
        {
            FindObjectOfType<PlayerController>().CanMove = false;
            FindObjectOfType<PlayerController>().RB.velocity = Vector3.zero;
            DA.Play("DoorOpen");
            DestinationDoor.GetComponent<Animator>().Play("DoorOpen");
            ScreenFader.Play("FadeOut");
            Invoke("Transport", 1f);
        }
    }

    public void Transport()
    {
        FindObjectOfType<PlayerController>().transform.position = DestinationDoor.transform.position;
        ScreenFader.Play("FadeIn");
        FindObjectOfType<PlayerController>().CanMove = true;
        DestinationDoor.GetComponent<Animator>().Play("DoorClose");
        DA.Play("DoorClose");
    }
}
