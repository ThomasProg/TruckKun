using UnityEngine;

public class TruckScript : MonoBehaviour
{
    private const float MovementBounds = .55f;
    private const float RotationBounds = 25f;

    public float speed;
    public DrivingGameManager manager;

    private const float DriftInertia = .2f;
    private const float DefaultInertia = .5f;
    private float rotationInertia = .5f;
    private float rotationMultiplier = 1.15f;
    private Vector3 initialPosition;

    [Header("Audio")] 
    public AudioSource TruckAudioSource;
    public AudioClip DriftAudio;
    public AudioClip CrashAudio;
    public AudioClip PickupAudio;
    
    void Start()
    {
        rotationInertia = .5f;
        initialPosition = transform.position;
    }

    public void Reset()
    {
        rotationInertia = .5f;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = initialPosition;
    }

    void Update()
    {
        ReadInput();
        UpdateTruckRotation();
    }

    private void ReadInput()
    {
        var isControlling = false;
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            TruckAudioSource.pitch = .9f;
            isControlling = true;
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            TruckAudioSource.pitch = .9f;
            isControlling = true;
            MoveRight();
        }

        if (!isControlling)
        {
            rotationInertia = Mathf.Lerp(rotationInertia, DefaultInertia, 5 * Time.deltaTime);
            TruckAudioSource.pitch = 1f;
        }
    }

    private void MoveLeft()
    {
        if (transform.position.x <= -MovementBounds) return;
        transform.Translate(Vector3.left * (speed * Time.deltaTime), Space.World);

        if (rotationInertia > DefaultInertia)
        {
            rotationInertia -= 20 * rotationMultiplier * Time.deltaTime;
            if (rotationInertia > 1f - DriftInertia) TruckAudioSource.PlayOneShot(DriftAudio);
        }
        else
        {
            rotationInertia -= rotationMultiplier * Time.deltaTime;
        }
    }

    private void MoveRight()
    {
        if (transform.position.x >= MovementBounds) return;
        transform.Translate(Vector3.right * (speed * Time.deltaTime), Space.World);

        if (rotationInertia < DefaultInertia)
        {
            rotationInertia += 20 * rotationMultiplier * Time.deltaTime;
            if (rotationInertia < DriftInertia) TruckAudioSource.PlayOneShot(DriftAudio);
        }
        else
        {
            rotationInertia += rotationMultiplier * Time.deltaTime;
        }
    }

    private void UpdateTruckRotation()
    {
        var yRotation = Mathf.Lerp(-RotationBounds, RotationBounds, rotationInertia);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "DrivingObstacle" :
                manager.GetHitObstacle();
                TruckAudioSource.PlayOneShot(CrashAudio);
                var rb = other.GetComponent<Rigidbody>();
                var xForce = (other.transform.position.x - transform.position.x) * 1000f;
                rb.AddForce(xForce, 500f, 500f);
                Destroy(other.gameObject, 5f);
                break;
            case "DrivingPickup" :
                manager.GetPickup();
                TruckAudioSource.PlayOneShot(PickupAudio);
                Destroy(other.gameObject);
                break;
        }
    }
}
