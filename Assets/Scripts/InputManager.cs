using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private GameManager gameManager_;

    private void Awake()
    {
        gameManager_ = GetComponent<GameManager>();
    }

    public void OnReset(InputAction.CallbackContext callback)
    {
        if(callback.started)
            gameManager_.StartGame();
    }

    public void OnClick(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, 11f);
            if (hit.collider)
            {
                gameManager_.RegisterHit(hit.transform.gameObject);
            }
        }
    }

}
