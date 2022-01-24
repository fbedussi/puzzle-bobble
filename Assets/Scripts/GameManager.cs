using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Group
{
    public Color color { set; get; }
    public List<Bubble> bubbles { set; get; }

    public Group(Color color, Bubble bubble)
    {
        this.color = color;
        this.bubbles = new List<Bubble>() { bubble };
    }

    public void Add(Bubble bubble)
    {
        this.bubbles.Add(bubble);
    }

    public bool Contains(Bubble bubble)
    {
        return this.bubbles.Contains(bubble);
    }
}

class Cell
{
    public Color color { set; get; }
    public Bubble bubble { set; get; }
    public float x { set; get; }
    public float y { set; get; }

    public Cell(Color color, Bubble bubble, float x, float y)
    {
        this.color = color;
        this.bubble = bubble;
        this.x = x;
        this.y = y;
    }

    public void SetBubble(Bubble bubble)
    {
        this.bubble = bubble;
    }
}

public class GameManager : MonoBehaviour
{
    public static List<Group> groups = new List<Group>();

    public static int maxNumberOfBubblesInGroup = 3;

    public Bubble Bubble;

    public static GameManager Instance { get; private set; }


    static List<List<Cell>> bubbles { set; get; }

    public static bool isPlaying = false;

    static float bubbleRadius = 1f;
    static float sqrt3 = Mathf.Sqrt(3);
    static float exagonSide = (2 * bubbleRadius * sqrt3) / 3;

    void InitGrid()
    {
        bubbles = new List<List<Cell>>();
        for (int rowN = 0; rowN < 8; rowN++)
        {
            List<Cell> row = new List<Cell>();
            bubbles.Add(row);

            for (int colN = 0; colN < 18; colN++)
            {
                bool isEvenRow = (rowN + 1) % 2 == 0;
                float x = -9 + ((colN * 2) + 1 + (isEvenRow ? 1 : 0)) * 0.5f;
                float y = -2.5f + (((rowN * 1.5f) + 1) * exagonSide) * 0.5f;
                if (rowN >= 4)
                {
                    Vector3 position = new Vector3(x, y, 0);
                    Bubble newBubble = Instantiate(Bubble, position, new Quaternion());
                    Rigidbody2D newBubbleRigidBody = newBubble.GetComponent<Rigidbody2D>();
                    newBubbleRigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
                    SpriteRenderer newBubbleRenderer = newBubble.GetComponent<SpriteRenderer>();
                    Color color = newBubbleRenderer.material.GetColor("_Color");
                    row.Add(new Cell(color, newBubble, x, y));
                } else
                {
                    row.Add(new Cell(Color.black, null, x, y));
                }
            }
        }
    }

    void Start()
    {
        Instance = this;
        InitGrid();
    }

    public static void SetIsPlaying()
    {
        isPlaying = true;   
    }

    public static void AddToGrid(Bubble bubble)
    {
        foreach (List<Cell> row in bubbles)
        {
            foreach (Cell cell in row)
            {
                if (Mathf.Abs(bubble.transform.position.x - cell.x) < (bubbleRadius/2) && Mathf.Abs(bubble.transform.position.y - cell.y) < (bubbleRadius /2))
                {
                    bubble.transform.position = new Vector2(cell.x, cell.y);
                    cell.SetBubble(bubble);
                }
            }
        }
    }

    public static void CreateGroups(Group newGroupInput = null)
    {
        for (int rowIndex = 0; rowIndex < bubbles.Count; rowIndex++)
        {
            for (int colIndex = 0; colIndex < bubbles[rowIndex].Count; colIndex++)
            {
                Cell cell = bubbles[rowIndex][colIndex];
                Color cellColor = cell.color;
                Group newGroup = newGroupInput == null ? new Group(cellColor, cell.bubble) : newGroupInput;
                if (newGroupInput == null)
                {
                    groups.Add(newGroup);
                }

                int[] siblingsIndexOnSameRow = { colIndex - 1, colIndex + 1 };
                foreach (int index in siblingsIndexOnSameRow)
                {
                    if (index >= 0 && index < bubbles[rowIndex].Count && (bubbles[rowIndex][index].color == cellColor) && !newGroup.Contains(bubbles[rowIndex][index].bubble))
                    {
                        newGroup.Add(bubbles[rowIndex][index].bubble);
                        CreateGroups(newGroup);
                    }

                }


                //if (colIndex + 1 < bubbles[rowIndex].Count && (bubbles[rowIndex][colIndex + 1].color == cellColor) && !newGroup.Contains(bubbles[rowIndex][colIndex + 1].bubble))
                //{
                //    newGroup.Add(bubbles[rowIndex][colIndex - 1].bubble);
                //    CheckGroups(newGroup);
                //}

                bool isEvenRow = (rowIndex + 1) % 2 == 0;
                int siblingRowsPrevIndex = isEvenRow ? colIndex - 1 : colIndex;
                int siblingRowsNexIndex = isEvenRow ? colIndex : colIndex + 1;

                List<List<Cell>> siblingRows = new List<List<Cell>>();
                if (rowIndex - 1 >= 0) {
                    siblingRows.Add(bubbles[rowIndex - 1]);
                }
                if (rowIndex + 1 < bubbles.Count)
                {
                    siblingRows.Add(bubbles[rowIndex + 1]);
                }
                foreach (List<Cell> row in siblingRows)
                {
                    if (row != null)
                    {
                        int[] siblingsIndexOnRow = { siblingRowsPrevIndex, siblingRowsNexIndex };
                        foreach (int index in siblingsIndexOnRow)
                        {
                            if (index >= 0 && index < row.Count && (row[index].color == cellColor) && !newGroup.Contains(row[index].bubble))
                            {
                                newGroup.Add(row[index].bubble);
                                CreateGroups(newGroup);
                            }

                        }
                    }
                }
            }
        }
    }

    public static void DeleteBubbles()
    {
        foreach (Group group in groups)
        {
            if (group.bubbles.Count >= maxNumberOfBubblesInGroup)
            {
                foreach (Bubble bubble in group.bubbles)
                {
                    Destroy(bubble);
                }
            }
        }
        groups = new List<Group>();
    }

    void Update()
    {
    }
}
