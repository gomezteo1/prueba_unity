using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float horizontalMove;
    public float verticalMove;
    
    private Vector3 playerInput;

    public CharacterController player;
    public float playerSpeed;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 movePlayer;

    //slide
    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;


    void Start(){

        player = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = 10;
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer = movePlayer * playerSpeed;
        
        player.transform.LookAt(player.transform.position + movePlayer);
            
        SetGravity();
        playerSkill();
        


        player.Move(movePlayer * Time.deltaTime);

    
    }
    
    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;
        
        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

    }
    //Funcion Gravedad
    void SetGravity()
    {
       

        if (player.isGrounded)
        {
            fallVelocity =- gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        
        else 
        {
            fallVelocity  -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;

        }
    }
    public void playerSkill()
    {

    //Doble salto
       // for (jumpForce = 0; jumpForce < 16f; jumpForce++) { 
            if (player.isGrounded && Input.GetButtonDown("Jump"))
            {
                jumpForce = 8f;
                fallVelocity = jumpForce;
                movePlayer.y = fallVelocity;

            //jumpForce += jumpForce;

            //if (Input.GetButtonDown("Jump"))
            //{


            //  fallVelocity = jumpForce;
            // movePlayer.y = fallVelocity;
            //}   
            SlideDown();


            }
        //}
    }
    
    public void SlideDown() 
    {
        slideVelocity = 7;
        slopeForceDown = -10;
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;
        if (isOnSlope)
        {
            movePlayer.x += ((1f-hitNormal.y) * hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f-hitNormal.y) * hitNormal.z) * slideVelocity;
            movePlayer.y += slopeForceDown;

        }

    }
        
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;

    }
}
