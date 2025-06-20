using UnityEngine;

public class PaintAniEnd : MonoBehaviour
{
    public void PaintAnimeEnd()
    {
        GameManager.Instance. ExecuteButton.interactable = true;
        GameManager.Instance.WTHButton.interactable = true;
        GameManager.Instance.RestoreButtonTXTColor();
    }
}
