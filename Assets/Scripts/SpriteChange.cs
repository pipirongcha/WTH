using UnityEngine;
using UnityEngine.EventSystems;


public class SpriteChanger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SpriteRenderer targetImage;        // 변경할 Image
    public Sprite normalSprite;     // 기본 상태 스프라이트
    public Sprite pressedSprite;    // 눌렸을 때 스프라이트
    public Animator targetAnimator; // 애니메이션 컨트롤러 연결

    public void OnPointerDown(PointerEventData eventData)
    {
            targetImage.sprite = pressedSprite;
            targetAnimator.enabled = false;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
            targetImage.sprite = normalSprite;
            targetAnimator.enabled = true;
    }
}
