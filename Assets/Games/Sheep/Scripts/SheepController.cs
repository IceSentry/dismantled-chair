using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    public float speed = 5f, distance = 1f;
    public GameObject activeIndicator;

    Rigidbody2D rigid;
    bool isActive = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;

        if (!isActive)
        {
            Vector3 direction = worldPos - transform.position;
            if (direction.magnitude <= distance)
            {
                isActive = true;
                activeIndicator.SetActive(false);
            }
        }
        else
        {
            rigid.MovePosition(Vector3.Lerp(transform.position, worldPos, speed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.EndGame(false);
        Debug.Log("Ouch!!");
    }
}
