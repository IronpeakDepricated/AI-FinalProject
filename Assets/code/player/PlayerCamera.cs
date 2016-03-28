﻿using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;

    public Camera topcam;
    public Camera fpscam;

    public static Camera TopCamera;
    public static Camera FPSCamera;

    void Awake()
    {
        m_CharacterTargetRot = transform.parent.localRotation;
        m_CameraTargetRot = transform.localRotation;
        PlayerCamera.TopCamera = topcam;
        PlayerCamera.FPSCamera = fpscam;
        TopCamera.enabled = false;
        FPSCamera.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            TopCamera.enabled = FPSCamera.enabled;
            FPSCamera.enabled = !FPSCamera.enabled;
        }

        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if(clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        transform.parent.localRotation = m_CharacterTargetRot;
        transform.localRotation = m_CameraTargetRot;
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}
