using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRootMotionController : MonoBehaviour
{
    [SerializeField]
    private bool characterEnabled;

    [SerializeField]
    private float dropHeight = 1.5f;

    [SerializeField]
    private Transform characterCamera;

    private PlayerActions playerActions;
    private Animator animator;
    private Vector2 movement;
    private float turnSmoothTime = 15f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerActions = new PlayerActions();

        playerActions.PlayerMovement.Move.canceled += CanceledMove;
        playerActions.PlayerMovement.Sprint.canceled += CanceledSprint;

        playerActions.PlayerMovement.Move.performed += PerformedMove;
        playerActions.PlayerMovement.Sprint.performed += PerformedSprint;
    }

    private void Update()
    {
        if (characterEnabled)
        {
            GetAxis();
            GetRotation();
            GetMovement();
        }
        else
        {
            animator.SetBool("moving", false);
            animator.SetBool("isSprinting", false);
        }
        GetGrounded();
    }
    void GetGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
        RaycastHit hitInfo = new RaycastHit();

        if(Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawRay(transform.position + Vector3.up, -Vector3.up);
            if (hitInfo.distance > dropHeight)
            {
                animator.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0,1,0), 0), 0.35f, 0.5f);
                
            }
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, hitInfo.point);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, hitInfo.point);
        }
    }

    private void GetMovement()
    {
        animator.SetBool("moving", movement.magnitude > 0);

        //animator.SetFloat("movement", new Vector2(horz, vert).sqrMagnitude);
    }

    private void GetAxis()
    {
        animator.SetFloat("xInput", movement.x);
        animator.SetFloat("yInput", movement.y);
    }

    private void GetRotation()
    {
        Vector3 target = characterCamera.forward * 1;
        target.Normalize();
        target.y = 0;

        if (target == Vector3.zero)
        {
            target = transform.forward;
        }

        Quaternion targetRot = Quaternion.LookRotation(target);
        Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, turnSmoothTime * Time.deltaTime);

        transform.rotation = playerRot;
    }

    public void SetEnabled(bool value)
    {
        characterEnabled = value;
    }

    public void ControlsEnabled(bool enable)
    {
        if (enable)
        {
            playerActions.PlayerMovement.Enable();
        }
        else
        {
            playerActions.PlayerMovement.Disable();
        }
    }

    private void CanceledMove(InputAction.CallbackContext context)
    {
        movement = Vector2.zero;
    }

    private void CanceledSprint(InputAction.CallbackContext context)
    {
        animator.SetBool("isSprinting", false);
    }

    private void PerformedMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void PerformedSprint(InputAction.CallbackContext context)
    {
        animator.SetBool("isSprinting", true);
    }
}
