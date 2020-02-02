using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGameManager : MonoBehaviour
{
    public List<GameObject> popupGameList;
    public static PopupGameManager Instance { get; private set; }
    private GameObject currentPopup;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CreatePopup();
    }

    public void CreatePopup()
    {
        var popup = popupGameList[Random.Range(0, popupGameList.Count)];
        currentPopup = Instantiate(popup, transform, false);
        var renderer = currentPopup.GetComponent<SpriteRenderer>();
        renderer.enabled = true;

    }

    public void ClosePopup()
    {
        Debug.Log("Debug");
        Destroy(currentPopup);
        CreatePopup();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
