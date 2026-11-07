using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    CameraController cmController;

    float position = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cmController =  FindFirstObjectByType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCameraMove(InputValue value)
    {
        if(cmController)
        {
            position += value.Get<float>();
            if (position < 0) position = 3;
            else if (position > 3) position = 0;

            if(position ==0) cmController.CameraChangeRotation(-45);
            else if(position ==1) cmController.CameraChangeRotation(-135);
            else if(position ==2) cmController.CameraChangeRotation(135);
            else if(position ==3) cmController.CameraChangeRotation(45);
        }
    }
}
