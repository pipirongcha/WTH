using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingManager : MonoBehaviour
{
    public TextMeshProUGUI TextSub;
    public TextMeshProUGUI TextMain;
    AudioSource audioSource;
    public AudioClip GameStart;
    public AudioClip Back;
    int LockedCodeNum;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        TextSub.text = GameManager.Instance.TryNum + "번 만에 클리어 하셨습니다!";
        TextMain.text = "지금까지 " + GameManager.Instance.CodeCheckNum +"번 코드를 확인하셨습니다.\r\n아직 확인되지 않은 코드가 "+ GameManager.Instance.LockedCodeNum +"개 있습니다.";
    }

    public void RetryButtonPressed()
    {
        audioSource.clip = GameStart;
        audioSource.Play();
        StartCoroutine(WaitAndLoadScene("GameScene",GameStart.length));
    }

    public void TitleButtonPressed()
    {
        audioSource.clip = Back;
        audioSource.Play();
        StartCoroutine(WaitAndLoadScene("TitleScene", Back.length));
    }

    IEnumerator WaitAndLoadScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); // 효과음 길이만큼 대기
        SceneManager.LoadScene(sceneName); // 씬 전환
    }
}
