using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class QueueManager : MonoBehaviour
{
    private readonly List<ShitterQueueItem> Shitters = new List<ShitterQueueItem>();
    public GameObject PeoplePrefab;
    public HorizontalLayoutGroup LayoutGroup;

    public void AddPeople(Shitter shitter)
    {
        if (Shitters.Exists(i => i.Shitter == shitter))
            return;

        var people = (GameObject)Instantiate(PeoplePrefab);
        people.transform.SetParent(LayoutGroup.transform);
        var shitterItem = people.GetComponent<ShitterQueueItem>();
        shitterItem.Setup(shitter);
        
        people.transform.localScale = Vector3.zero;
        people.transform.DOScale(Vector3.one, .3f).OnComplete(() =>
        {
            Shitters.Add(shitterItem);
        });
    }

    public void RemovePeople(Shitter shitter)
    {
        var shitterItem = Shitters.Find(i => i.Shitter == shitter);

        if (shitter == null)
            return;

        Shitters.Remove(shitterItem);
        shitterItem.FadeOut();
        shitterItem.transform.DOScale(Vector3.zero, .3f).OnComplete(() =>
        {
            Destroy(shitterItem.gameObject);
        });
    }
}
