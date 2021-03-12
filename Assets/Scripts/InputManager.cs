using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private GameManager gameManager_;

    private void Awake()
    {
        gameManager_ = GetComponent<GameManager>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, 11f);
            if(hit.collider)
            {
                gameManager_.RegisterHit(hit.transform.gameObject);
            }
        }
    }
}
