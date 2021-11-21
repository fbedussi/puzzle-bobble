 using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public class Group
    {
        public Color color { set; get; }
        public List<int> bubblesId { set; get; }

        public Group(Color color, int bubbleId)
        {
            this.color = color;
            this.bubblesId = new List<int>() { bubbleId };
        }
    }

    class Cell
    {
        Color color { set; get; }
        int bubbleId { set; get; }

        public Cell(Color color, int bubbleId)
        {
            this.color = color;
            this.bubbleId = bubbleId;
        }
    }

    public static List<Group> groups = new List<Group>();

    public static int maxNumberOfBubblesInGroup = 3;

    public Bubble Bubble;

    public static GameManager Instance { get; private set; }


    static List<List<Cell>> bubbles { set; get; }

    static float bubbleRadius = 1f;
    static float sqrt3 = Mathf.Sqrt(3);
    static float exagonSide = (2 * bubbleRadius * sqrt3) / 3;

    void Start()
    {
        Instance = this;
        bubbles = new List<List<Cell>>();
        for (int rowN = 0; rowN < 4; rowN++)
        {
            List<Cell> row = new List<Cell>();
            bubbles.Add(row);
            
            for (int colN = 0; colN < 18; colN++)
            {
                bool isEvenRow = (rowN + 1) % 2 == 0;
                float x = -9 + ((colN * 2) + 1 + (isEvenRow ? 1 : 0)) * 0.5f;
                float y = 1 + (((rowN * 1.5f) + 1) * exagonSide) * 0.5f;
                Vector3 position = new Vector3(x, y, 0);
                Bubble newBubble = Instantiate(Bubble, position, new Quaternion());
                Rigidbody2D newBubbleRigidBody = newBubble.GetComponent<Rigidbody2D>();
                newBubbleRigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
                SpriteRenderer newBubbleRenderer = newBubble.GetComponent<SpriteRenderer>();
                Color color = newBubbleRenderer.material.GetColor("_Color");
                row.Add(new Cell(color, newBubble.gameObject.GetInstanceID()));
            }
        }
        
    }

    //public static void AddToGrid(Bubble bubble)
    //{
    //    for (int rowN = 0; rowN < 8; rowN++)
    //    {
    //        for (int colN = 0; colN < 18; colN++)
    //        {
    //            bool isEvenRow = (rowN + 1) % 2 == 0;
    //            float x = -9 + ((colN * 2) + 1 + (isEvenRow ? 1 : 0)) * 0.5f;
    //            float y = 1 + (((rowN * 1.5f) + 1) * exagonSide) * 0.5f;
    //            if (bubble.transform.position.x - x < bubbleRadius && bubble.transform.position.y - y < bubbleRadius)
    //            {
    //                bubble.transform.position = new Vector2(x, y);
    //                bubbles[rowN][colN] = new Cell(bubble.GetComponent<SpriteRenderer>().material.GetColor("_Color"), bubble.gameObject.GetInstanceID());
    //            }
    //        }
    //    }
    //}

    public static void AddBubbleToGroup(int bubbleId, int collidingBubbleId, Color bubbleColor)
    {
        Group existingGroup = null;
        foreach (Group group in groups)
        {
            if (group.color == bubbleColor)
            {
                foreach (int id in group.bubblesId)
                {
                    if (id == collidingBubbleId)
                    {
                        existingGroup = group;
                        break;
                    }
                    if (existingGroup != null)
                    {
                        break;
                    }
                }
            }
        }

        if (existingGroup != null)
        {
                existingGroup.bubblesId.Add(bubbleId);
            if (existingGroup.bubblesId.Count >= maxNumberOfBubblesInGroup)
            {
                foreach (int id in existingGroup.bubblesId)
                {
                    Object bubbleToDestroy = EditorUtility.InstanceIDToObject(id);
                    Destroy(bubbleToDestroy);
                }
            }
        } else
        {
            groups.Add(new Group(bubbleColor, bubbleId));
        }
    }

    void Update()
    {
    }
}
