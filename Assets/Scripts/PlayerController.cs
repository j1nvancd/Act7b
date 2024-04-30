using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Lives")]
    public int currentLives;
    public int maxLives;


    [Header("Movement")]
    public float speed;
    public float jumpForce; 
    public float run;
    public float superJump;

    [Header("Camera")]
    public float mouseSensibility;
    public float maxViewX;
    public float minViewX;
    private float rotationX;

    private Camera cameraPlayer;
    private Rigidbody rb;
    private WeaponController weaponController;

    [Header("Audio")]
    public AudioSource shootSound; // AudioSource para reproducir el sonido 
    public AudioClip dmgClip; // AudioClip que se reproducirá cuando se haga el daño

    private void Awake()
    {
        cameraPlayer = Camera.main;
        rb = GetComponent<Rigidbody>();
        weaponController = GetComponent<WeaponController>();

        //Hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MovePlayer();
        CameraView();

        if (Input.GetButtonDown("Jump"))
            Jump();

        //Fire
        if(Input.GetButton("Fire1"))
            if(weaponController.CanShoot())
                weaponController.Shoot();

    }

    /// <summary>
    /// Jump action
    /// </summary>
    private void Jump()
    {
        //Through a ray down direction from player position
        Ray ray = new Ray(transform.position, Vector3.down);
        
        //If the ray collide with something at 1.1m then add force up
        if (Physics.Raycast(ray,1.1f))
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Control Camera View with Sensibility
    /// </summary>
    private void CameraView()
    {
        //if (!GameManager.instance.gamePaused) { 
            //Take from Mouse Input X and Y axis
            float y = Input.GetAxis("Mouse X") * mouseSensibility;
            rotationX += Input.GetAxis("Mouse Y") * mouseSensibility;

            //Cut x rotation with min and max limits
            rotationX = Mathf.Clamp(rotationX, minViewX, maxViewX);

            //Rotate the camera
            cameraPlayer.transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);
            //rotate the player
            transform.eulerAngles += Vector3.up * y;
        //}

    }

    /// <summary>
    /// Player Movement Input controller
    /// </summary>
    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
           float x = Input.GetAxis("Horizontal") * run;
            float z = Input.GetAxis("Vertical") * run;

            Vector3 direction = transform.right * x + transform.forward * z;
            direction.y = rb.velocity.y;

            rb.velocity = direction;  
        }
        else
        {
            float x = Input.GetAxis("Horizontal") * speed;
            float z = Input.GetAxis("Vertical") * speed;

            Vector3 direction = transform.right * x + transform.forward * z;
            direction.y = rb.velocity.y;

            rb.velocity = direction; 
        }
        
        //posible solución diferente

        /*float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        bool isrunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        float currentSpeed = isrunning ? speed * 2f : speed;

        Vector3 direction = transform.right * x + transform.forward * z;
        direction.y = rb.velocity.y;

        rb.velocity = direction;*/
    }

    /// <summary>
    /// Player reduce lives
    /// </summary>
    /// <param name="quantity">number of lives</param>
    public void DamagePlayer(int quantity)
    {
        // Reproducir el sonido de daño
        shootSound.PlayOneShot(dmgClip);
        
        currentLives -= quantity;

        //Call the static instance of HUDCOntroller to Update healt bar
        HUDController.instance.UpdateHealthBar(currentLives, maxLives);

        // call DamageIndicator
        HUDController.instance.ShowDamageIndicator();
        

        if (currentLives <= 0)
        {
            Debug.Log("GAME OVER!!!");
        }
    }

   
}