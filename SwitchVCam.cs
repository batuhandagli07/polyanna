using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int priorityBoostAmount=10;  // Priority for cam. Which cam priority's higher, it will run.

    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;  // Closer aim

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];  // 2nd mouse button

        aimCanvas.enabled = false;  // Preventive for starts
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();  // When we push the 2nd mouse button, we are calling StartAim() here
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount; // We are increasing priority of game
        aimCanvas.enabled = true;  // Canvas get enable
        thirdPersonCanvas.enabled = false;  // Far camera getting disable
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }

}
