using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections))]
public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public float walkSpeed = 5f;
    public float airWalkSpeed = 3f;
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    public float crouchSpeed = 2.5f;

    public PhotonView pv;
    public TMP_Text nameText;
    private Vector3 smoothMove;
    public GameObject sceneCamera;
    
    public GameObject playerCamera;
    private Vector3 respawnPoint;
    private Vector2 moveInput;
    public GameObject fallDetector;
    private bool _isFacingRight = true; // Corrected the property name
    TouchingDirections touchingDirections;

    private bool isCrouching = false;
    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    // On ground and not touching the wall
                    return IsRunning ? runSpeed : walkSpeed;
                }
                else
                {
                    // In air
                    return airWalkSpeed;
                }
            }

            // Idle or on wall, speed is 0
            return 0;
        }
    }


    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
            {
                _isFacingRight = value;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1); // Corrected the Vector constructor
                // Ajout pour flipper le texte du nom.
            if (nameText != null) {
                // Ceci inverse la transformation sur l'axe X uniquement pour le nomText.
                nameText.transform.localScale = new Vector3(-nameText.transform.localScale.x, nameText.transform.localScale.y, nameText.transform.localScale.z);
            }
                pv.RPC("SyncAnimationStates", RpcTarget.All, IsRunning, IsMoving, _isFacingRight);

            }
        }
    }

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        pv = GetComponent<PhotonView>();

         if (fallDetector == null)
    {
        fallDetector = GameObject.FindWithTag("FallDetector"); // Remplacez "FallDetectorTag" par le tag réel que vous avez utilisé pour votre détecteur de chute.
    }
    }
    [PunRPC]
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    [PunRPC]
    public void SyncAnimationStates(bool isRunning, bool isMoving, bool isFacingRight)
    {
        IsRunning = isRunning;
        IsMoving = isMoving;
        IsFacingRight = isFacingRight; // Assurez-vous de g�rer correctement cette propri�t�
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            moveInput = context.ReadValue<Vector2>();
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
            // Utilisez la propri�t� IsMoving directement dans l'appel RPC
            pv.RPC("SyncAnimationStates", RpcTarget.All, IsRunning, IsMoving, _isFacingRight);

        }
        
    }
    [PunRPC]
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !_isFacingRight) // Corrected the condition check
        {
            IsFacingRight = true;
            nameText.transform.localScale = new Vector3(Mathf.Abs(nameText.transform.localScale.x), nameText.transform.localScale.y, nameText.transform.localScale.z);
            pv.RPC("SyncAnimationStates", RpcTarget.All, IsRunning, IsMoving, _isFacingRight);
        }
        else if (moveInput.x < 0 && _isFacingRight) // Corrected the condition check
        {
            IsFacingRight = false;
            nameText.transform.localScale = new Vector3(-Mathf.Abs(nameText.transform.localScale.x), nameText.transform.localScale.y, nameText.transform.localScale.z);
            pv.RPC("SyncAnimationStates", RpcTarget.All, IsRunning, IsMoving, _isFacingRight);
        }
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            IsRunning = context.ReadValueAsButton();
            // Utilisez la propri�t� IsMoving directement dans l'appel RPC
            pv.RPC("SyncAnimationStates", RpcTarget.All, IsRunning, IsMoving, _isFacingRight);

        }
    }


    [PunRPC]
    void PerformJump()
    {
        animator.SetTrigger(AnimationStrings.jump);
        rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
    }

    [PunRPC]

    public void OnJump(InputAction.CallbackContext context)
    {
        //todo check if alive as well
        if(photonView.IsMine){
        if (context.started && touchingDirections.IsGrounded) {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
                pv.RPC("PerformJump", RpcTarget.All);
            }
        }
    }
        void Start(){
        respawnPoint = transform.position;
        if (photonView.IsMine){
         nameText.text = PhotonNetwork.NickName;
        sceneCamera.SetActive(true);
        playerCamera.SetActive(false);

        } else{
            nameText.text = pv.Owner.NickName;
            playerCamera.SetActive(false);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine){
                checkInput();
        }else {
                smootNetMovement();
    }

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
        


        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Move(horizontalInput);

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCrouch();
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            StopCrouch();
        }
    }

     private void Move(float horizontalInput)
    {
        // Move the player left or right
        float moveSpeed = isCrouching ? crouchSpeed : walkSpeed;
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Update the animator
        if (horizontalInput != 0)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isCrouching", isCrouching);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

     private void StartCrouch()
    {
        isCrouching = true;
        // Optionally, adjust the player's collider size
        // Transition to crouch animation
        animator.SetBool("isCrouching", true);
    }

    private void StopCrouch()
    {
        isCrouching = false;
        // Optionally, reset the player's collider size
        // Transition out of crouch animation
        animator.SetBool("isCrouching", false);
    }

    // /////////////////////////////////////////////////


    private void smootNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);
    }
    private void checkInput()
    {
        float verticalMovement =0f;
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), verticalMovement, 0f);
        transform.position += move * runSpeed * Time.deltaTime;
    }

    
     public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting){
            stream.SendNext(transform.position);
        }else if(stream.IsReading){
            smoothMove = (Vector3) stream.ReceiveNext();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FallDetector") {
            transform.position = respawnPoint;
        }else if(collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
    }
}
