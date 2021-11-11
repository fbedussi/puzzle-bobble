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

    public static List<Group> groups = new List<Group>();

    public static int maxNumberOfBubblesInGroup = 3;

    public static GameManager Instance { get; private set; }


    void Start()
    {
        Instance = this;
    }

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
