using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QueueManager : MonoBehaviour
{
    private readonly Queue<GameObject> People = new Queue<GameObject>();
    public GameObject PeoplePrefab;
    public HorizontalLayoutGroup LayoutGroup;

    public void AddPeople()
    {
        var people = Instantiate(PeoplePrefab, LayoutGroup.transform);
        People.Enqueue((GameObject)people);
    }

    public void RemovePeople()
    {
        var people = People.Dequeue();
        Destroy(people);
    }
}
