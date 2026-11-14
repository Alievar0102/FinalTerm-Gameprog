using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    CameraController cmController;
    float position = 0;

    [SerializeField] LayerMask groundLayer;
    Camera mainCamera;

    public GameObject bulletPrefab;
    public GameObject gun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cmController =  FindFirstObjectByType<CameraController>();

        if(mainCamera == null) mainCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        aimMouse();
    }

    void aimMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);

            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f; // Keep the player looking horizontally

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    void OnFire(InputValue value)
    {
        if (value.isPressed)
        {
            Shoot(bulletPrefab);
        }
    }

    void Shoot(GameObject bulletPrefab)
    {
        Instantiate(bulletPrefab, gun.transform.position, transform.rotation);
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
