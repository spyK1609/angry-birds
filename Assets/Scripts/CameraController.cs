using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public GameObject bird;
    public bool isFree;
    public float sensitivity = 0.15f;
    public AudioSource audioSource;
    public Canvas canvas;
    
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        audioSource = gameObject.GetComponent<AudioSource>();
        canvas.worldCamera = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isFree = true;
            print(isFree);
        }
        if (Input.GetMouseButton(1) && isFree)
            FreeCamMoving();
        if (Input.GetMouseButtonDown(0) && isFree)
            {
                isFree = false;
            }
        if (!isFree)
            FollowBird();
    }
    void FreeCamMoving()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        transform.position -= new Vector3(mouseX, 0f, 0f) * sensitivity; 
    }
        
    void FollowBird()
    {
        if (bird == null) return;
        Vector3 target = new Vector3(bird.transform.position.x, transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, target, 5f * Time.deltaTime);
    }
}
