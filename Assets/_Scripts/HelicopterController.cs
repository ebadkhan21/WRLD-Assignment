using UnityEngine;
using System.Threading.Tasks;

public class HelicopterController : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 20f;
    [SerializeField] float RotationSpeed = 30f;

    [SerializeField] Transform Body, MainRotor, RearRotor = null;

    private bool CanControl = false;
    private float Horizontal, Vertical, LookRotation, Elevate = 0f;

    private async void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        await Task.Delay(4000);
        CanControl = true;
        await Task.Delay(4000);
        transform.position += Vector3.down * 150f;
    }

    private void Update()
    {
        if (!CanControl)
            return;
        HandleMovement();
        RotorRotation();
    }

    private void HandleMovement()
    {
        Horizontal = Mathf.Lerp(Horizontal, Input.GetAxis("Horizontal"), 5f * Time.deltaTime);
        Vertical = Mathf.Lerp(Vertical, Input.GetAxis("Vertical"), 5f * Time.deltaTime);
        LookRotation = Mathf.Lerp(LookRotation, Input.GetAxis("Mouse X"), 5f * Time.deltaTime);
        Elevate = Mathf.Lerp(Elevate, Input.GetAxis("Mouse ScrollWheel"), 5f * Time.deltaTime);

        Vector3 movementDirection = ((transform.forward * Vertical * 10f) +
                                    (transform.right * Horizontal * 5f) +
                                    (transform.up * Elevate * 100f)) * MoveSpeed;
        transform.position += movementDirection * Time.deltaTime;
        Vector3 ClampedPos = transform.position;
        ClampedPos.y = Mathf.Clamp(ClampedPos.y, 150f, 300f);
        transform.position = ClampedPos;
        transform.Rotate(0f, LookRotation * Time.deltaTime * RotationSpeed * 10f, 0f);
        Body.localRotation = Quaternion.RotateTowards(Body.localRotation,
                                                      Quaternion.Euler(Vertical * 15f, 0f, -Horizontal * 15f), 10f * Time.deltaTime);
    }

    private void RotorRotation()
    {
        MainRotor.Rotate(0f, 0f, 750f * Time.deltaTime);
        RearRotor.Rotate(750f * Time.deltaTime, 0f, 0f);
    }

}