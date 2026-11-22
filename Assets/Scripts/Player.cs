using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    CameraController cmController;
    float position = 0;

    //[SerializeField] LayerMask layers;
    Camera mainCamera;

    public GameObject bulletPrefab;
    public GameObject gun;
    Gun gunScript;
    bool isAiming = false;
    bool isFire = false;
    public float bulletRandomRotation = 0f;
    float timerFire = 0f;
    public float fireRate = 2f;

    public Texture2D[] cursors;

    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cmController =  FindFirstObjectByType<CameraController>();

        if(mainCamera == null) mainCamera = Camera.main;

        gunScript = gun.GetComponent<Gun>();

        if (cursors.Length > 0) ApplyCursor(0);
        singleCrsAnimRate = cursorAnimRate / cursors.Length;
        currentCrsAnimRate = singleCrsAnimRate;
        
    }

    // Update is called once per frame
    void Update()
    {
        aimMouse();

        pausePanel.SetActive(value: GameState.Instance.currentState == GameState.State.Paused);
        winPanel.SetActive(value: GameState.Instance.currentState == GameState.State.Win);
        gameOverPanel.SetActive(value: GameState.Instance.currentState == GameState.State.GameOver);
    }

    private void FixedUpdate()
    {
        #region FIRE
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
        #endregion

        #region anim
        //cursor
        if (isAiming && currentCursor < cursors.Length - 1)
        {
            if (timerCursor <= cursorAnimRate)
            {
                timerCursor += Time.fixedDeltaTime;
                if(timerCursor >= currentCrsAnimRate)
                {
                    currentCursor++;
                    if(currentCrsAnimRate <= cursorAnimRate) currentCrsAnimRate += singleCrsAnimRate;
                    ApplyCursor(currentCursor);
                }
            }
        }
        else if (!isAiming && currentCursor > 0)
        {
            if(timerCursor >= 0)
            {
                timerCursor -= Time.fixedDeltaTime;
                if (timerCursor <= currentCrsAnimRate)
                {
                    currentCursor--;
                    if (currentCrsAnimRate > 0) currentCrsAnimRate -= singleCrsAnimRate;
                    ApplyCursor(currentCursor);
                }
            }
        }
        #endregion
    }

    #region aim
    void aimMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Plane aimPlane = new Plane(Vector3.up, gun.transform.position);

        /*
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layers))
        {
            Debug.DrawLine(new Vector3(ray.origin.x, ray.origin.y + gun.transform.position.y, ray.origin.z), hit.point, Color.red);

            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f; // Keep the player looking horizontally

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }*/

        if (aimPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            Vector3 aimDirection = (targetPoint - gun.transform.position).normalized;

            Debug.DrawLine(gun.transform.position, targetPoint, Color.red);

            Vector3 lookDirection = targetPoint - transform.position;
            lookDirection.y = 0f;


            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    void OnAim(InputValue value)
    {
        isAiming = value.isPressed;
        StarterAssetsInputs sa = GetComponent<StarterAssetsInputs>();
        sa.isAimSA = isAiming;
    }

    int currentCursor = 0;
    public float cursorAnimRate = 1f;
    float currentCrsAnimRate = 0f;
    float singleCrsAnimRate = 0f;
    float timerCursor = 0f;

    void ApplyCursor(int index)
    {
        Texture2D tex = cursors[index];
        if (tex == null) return;

        Vector2 hotspot = new Vector2(tex.width / 2f, tex.height / 2f);

        Cursor.SetCursor(tex, hotspot, CursorMode.Auto);
    }
    #endregion

    #region fire
    void OnFire(InputValue value)
    {
        isFire = value.isPressed;
    }

    void Shoot(GameObject bulletPrefab)
    {
        Instantiate(bulletPrefab, gun.transform.position, transform.rotation * Quaternion.Euler(0f, Random.Range(-bulletRandomRotation, bulletRandomRotation), 0f));
    }
    #endregion

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
