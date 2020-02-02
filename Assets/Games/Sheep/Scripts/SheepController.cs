using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    public float speed = 5f, distance = 1f;
    public GameObject activeIndicator;
    public GameObject[] bodiesPart;

    bool isActive;
    Vector3 startingPos;
    Rigidbody2D rigid;

    public Transform shakeTarget;
    public float shakeDuration = 0.2f;
    public Vector3 shakeStrength = new Vector3(0.15f, 0.15f, 0);
    private float shakeTimeRemaining;

    private void Start()
    {
        startingPos = transform.position;
        rigid = GetComponent<Rigidbody2D>();
        shakeTarget = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;

        if (!isActive)
        {
            Vector3 direction = worldPos - transform.position;
            if (direction.magnitude <= 0.5f)
            {
                isActive = true;
                activeIndicator.SetActive(false);
            }
        }
        else
        {
            Vector3 direction = worldPos - transform.position;
            if (direction.magnitude > 0.01f)
            {
                transform.right = direction;
            }
            rigid.MovePosition(Vector3.Lerp(transform.position, worldPos, speed * Time.deltaTime));
        }

        shakeTimeRemaining -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 pos = transform.position;
        transform.position = startingPos;
        for (int i = 0; i < bodiesPart.Length; i++)
        {
            Vector3 diff = bodiesPart[i].transform.position - pos;
            bodiesPart[i].transform.position = diff + startingPos;
        }

        isActive = false;
        activeIndicator.SetActive(true);

        if (shakeTimeRemaining < 0)
        {
            shakeTarget.DOShakePosition(shakeDuration, shakeStrength, 20, 200f);
            shakeTimeRemaining = shakeDuration;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.EndGame(GameType.Sleep, 3);
    }
}
