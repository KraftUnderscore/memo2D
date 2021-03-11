using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameManager gameManager_;

    private void Awake()
    {
        gameManager_ = GetComponent<GameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 11f);
            if(hit.collider)
            {
                gameManager_.RegisterHit(hit.transform.gameObject);
            }
        }
    }
}
