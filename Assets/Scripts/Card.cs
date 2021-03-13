using UnityEngine;

public class Card : MonoBehaviour
{
    public void InitializeCard(int id, int pairId, GameObject cardObj, Sprite front)
    {
        id_ = id;
        pairId_ = pairId;
        cardObj_ = cardObj;
        isFlipped_ = false;
        anim_ = cardObj_.GetComponent<Animator>();
        renderer_ = cardObj.transform.GetChild(0).GetComponent<SpriteRenderer>();
        back_ = renderer_.sprite;
        front_ = front;
    }

    [HideInInspector]
    public int id_;
    [HideInInspector]
    public int pairId_;
    [HideInInspector]
    public bool isFlipped_;
    [HideInInspector]
    public GameObject cardObj_;

    private Animator anim_;
    private SpriteRenderer renderer_;
    private Sprite back_;
    private Sprite front_;

    public void Flip()
    {
        isFlipped_ = true;
        anim_.SetTrigger("Flip");
    }

    public void Unflip()
    {
        isFlipped_ = false;
        anim_.SetTrigger("Unflip");
    }

    public void Disappear()
    {
        anim_.SetTrigger("Disappear");
    }

    public void DisableCard()
    {
        if (isFlipped_)
        {
            cardObj_.SetActive(false);
            isFlipped_ = false;
        }
    }

    public void SwapSprite()
    {
        if (isFlipped_)
            renderer_.sprite = front_;
        else
            renderer_.sprite = back_;
    }

    public void Restore()
    {
        cardObj_.SetActive(false);
        cardObj_.transform.localScale = Vector2.one;
        cardObj_.transform.rotation = Quaternion.Euler(Vector3.zero);
        isFlipped_ = false;
        renderer_.sprite = back_;
        renderer_.color = new Color(renderer_.color.r, renderer_.color.g, renderer_.color.b, 1f);
        anim_.ResetTrigger("Flip");
        anim_.ResetTrigger("Unflip");
        anim_.ResetTrigger("Disappear");
    }
}
