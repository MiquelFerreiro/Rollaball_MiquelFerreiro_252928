using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject perfectTextObject;
    public AudioClip pickupSound;
    public AudioClip winSound;
    public AudioClip perfectSound;

    private int count;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int total;
    private bool extra;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        total = 12;
        extra = false;

        SetCountText();
        winTextObject.SetActive(false);
        perfectTextObject.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString() + " / " + total.ToString();

        if(count == total && extra)
        {
            winTextObject.SetActive(false);
            perfectTextObject.SetActive(true);

            audioSource.PlayOneShot(perfectSound);
        }
        else if(count == total)
        {
            winTextObject.SetActive(true);

            audioSource.PlayOneShot(winSound);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count += 1;

            SetCountText();

            audioSource.PlayOneShot(pickupSound);
        }
        else if (other.gameObject.CompareTag("Extra PickUp"))
        {
            other.gameObject.SetActive(false);
            extra = true;
            count += 1;
            total += 1;

            countText.color = Color.red;
            SetCountText();

            audioSource.PlayOneShot(pickupSound);
        }
    }
}
