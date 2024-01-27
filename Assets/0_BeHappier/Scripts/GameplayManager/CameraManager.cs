using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Animator cameraAnimator;
    
    public CinemachineVirtualCamera vCameraMain;
    public CinemachineVirtualCamera vCameraLoad;

    public void SwitchToCameraMain()
    {
        cameraAnimator.Play("Camera Main");
    }
}
