using UnityEngine;

public class Bubble : MonoBehaviour
{
    bool isStill = false;
    Rigidbody2D _rigidbody;
    SpriteRenderer _renderer;
    static float bubbleRadius = 1f;
    static float sqrt3 = Mathf.Sqrt(3);
    static float exagonSide = (2 * bubbleRadius * sqrt3) / 3;

    Vector3 _velocity;

    Color GetRandomColor()
    {
        Color[] colors = { Color.red, Color.magenta, Color.green, Color.grey, Color.blue, Color.cyan, Color.yellow };
        return colors[Random.Range(0, colors.Length - 1)];
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.material.SetColor("_Color", GetRandomColor());
    }

    // Update is called once per frame
    void Update()
    {
        if (isStill)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        //else
        //{
        //    this.GetComponent<Rigidbody2D>().velocity = this._velocity;
        //}
    }

    public void Launch(Vector3 velocity)
    {
        this._velocity = velocity;
        this.GetComponent<Rigidbody2D>().velocity = velocity;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStill == false)
        {
            isStill = true;
            if (_rigidbody && _rigidbody.velocity!= Vector2.zero)
            {
                GameManager.AddToGrid(this);
            }
            _renderer = GetComponent<SpriteRenderer>();

            if (_renderer && GameManager.isPlaying)
            {
                GameManager.CreateGroups();
                GameManager.DeleteBubbles();
            }
        }
    }
}