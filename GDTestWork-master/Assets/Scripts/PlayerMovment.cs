using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CharacterController))]
public class PlayerMovment : MonoBehaviour

{
    [Header("Movment Settings")]
    //Handling
    public float rotationSpeed = 450;
    public float walkSpeed = 5;
    public float runSpeed = 8;

    //Components
    private CharacterController controller;
    private Animator animControl;

    //System
    private Quaternion targetRotation;

    private void Start()
    {
        animControl = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Player.instance.blockScript || Player.instance.startAttack || SceneManager.Instance.winGame)
        {
            return;
        }

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (input != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        Vector3 motion = input;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);

        animControl.SetFloat("Speed", motion.sqrMagnitude);
    }
}

