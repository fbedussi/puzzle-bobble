using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float horizontalSpeed = 30.0F;
    public Bubble Bubble;
    
    public int bulletSpeed = 6;

    Bubble  currentBubble;

    // Start is called before the first frame update
    void Start()
    {
        transform.RotateAround(new Vector3(0, -4, 0), Vector3.forward, 0);
        this.InitBubble();
    }

    void Rotate()
    {
        float x = horizontalSpeed * Input.GetAxis("Mouse X");
        transform.RotateAround(new Vector3(0, -4, 0), Vector3.forward, -x);
        if (currentBubble)
        {
            currentBubble.transform.RotateAround(new Vector3(0, -4, 0), Vector3.forward, -x);
        }
    }

    void InitBubble()
    {
        currentBubble = Instantiate(Bubble, transform.localPosition, transform.rotation);
        //currentBubble.transform.localPosition = this.transform.localPosition;
    }

    void Shoot()
    {       
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 velocity = new Vector3(transform.up.x * bulletSpeed, transform.up.y * bulletSpeed, 0);
            currentBubble.Launch(velocity);
            currentBubble = null;
            Invoke("InitBubble", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.Rotate();
        this.Shoot();
    }
}
