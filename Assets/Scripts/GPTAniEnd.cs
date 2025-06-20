using UnityEngine;

public class GPTAniEnd : MonoBehaviour
{
    public void GPTAnimationEnd()
    {

        GameManager.Instance.CodeBox.text = GameManager.Instance.CurrentStage.StageCodeMaking();
        GameManager.Instance.CurrentCodeLen = GameManager.Instance.CodeBox.text.Length;
        GameManager.Instance.ScrollDown();
        GameManager.Instance.ExecuteButton.interactable = true;
        GameManager.Instance.WTHButton.interactable = true;
        GameManager.Instance.RestoreButtonTXTColor();
    }
}
