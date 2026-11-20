using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    CameraController cmController;
    float position = 0;

    [SerializeField] LayerMask layers;
    Camera mainCamera;

    public GameObject bulletPrefab;
    public GameObject gun;
    Gun gunScript;
    bool isAiming = false;
    bool isFire = false;
    public float bulletRandomRotation = 0f;
    float timerFire = 0f;
    public float fireRate = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cmController =  FindFirstObjectByType<CameraController>();

        if(mainCamera == null) mainCamera = Camera.main;

        gunScript = gun.GetComponent<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        aimMouse();
    }

    private void FixedUpdate()
    {
        if (timerFire >= fireRate)
        {
            if (isFire) 
            {
                bulletRandomRotation = gunScript.bulletSpread * (isAiming ? 1f : gunScript.bulletSpreadMultiplier);
                /*if (!isAiming) bulletRandomRotation = gunScript.bulletSpread * gunScript.bulletSpreadMultiplier;
                else bulletRandomRotation = gunScript.bulletSpread;*/
                Shoot(bulletPrefab);
                timerFire = 0f;
            }
            else
            {
                timerFire = fireRate;
            }
        }
        else
        {
            timerFire += Time.deltaTime;
        }

    }

    void aimMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layers))
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
        isFire = value.isPressed;
    }

    void OnAim(InputValue value)
    {
        isAiming = value.isPressed;
    }

    void Shoot(GameObject bulletPrefab)
    {
        Instantiate(bulletPrefab, gun.transform.position, transform.rotation * Quaternion.Euler(0f, Random.Range(-bulletRandomRotation, bulletRandomRotation), 0f));
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
