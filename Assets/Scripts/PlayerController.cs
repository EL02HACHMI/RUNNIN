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
    public PhotonView pv;
    public TMP_Text nameText;
    private Vector3 smoothMove;
    public GameObject sceneCamera;
    public GameObject playerCamera;

    private Vector2 moveInput;

    private bool _isFacingRight = true; // Corrected the property name
    TouchingDirections touchingDirections;
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
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }
    [PunRPC]
    public void OnMove(InputAction.CallbackContext context)
    {
        if(photonView.IsMine){
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput); // Moved SetFacingDirection call to here with parameter
        
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !_isFacingRight) // Corrected the condition check
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && _isFacingRight) // Corrected the condition check
        {
            IsFacingRight = false;
        }
    }
    [PunRPC]
    public void OnRun(InputAction.CallbackContext context)
    {
        if(photonView.IsMine){
        IsRunning = context.ReadValueAsButton();
            
        }
    }


    [PunRPC]
    void PerformJump()
    {
        animator.SetTrigger(AnimationStrings.jump);
        rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //todo check if alive as well
        if(photonView.IsMine){
        if (context.started && touchingDirections.IsGrounded) {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            photonView.RPC(AnimationStrings.jump, RpcTarget.All);

            }
        }
    }
        void Start(){
        if(photonView.IsMine){
         nameText.text = PhotonNetwork.NickName;
        sceneCamera.SetActive(false);
        playerCamera.SetActive(true);

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
    }

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
        /*else if (stream.IsWriting)
        {
            stream.SendNext(animator.GetBool("isRunning"));
        }
        else
        {
            animator.SetBool("isRunning", (bool)stream.ReceiveNext());
        */
    }
    

    /*[PunRPC]
    void UpdateRunningState(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    void HandleMovement()
    {
        if (photonView.IsMine)
        {
            bool run = Input.GetKey(KeyCode.LeftShift);
            photonView.RPC("UpdateRunningState", RpcTarget.All, run);
        }
    }

    */

}
