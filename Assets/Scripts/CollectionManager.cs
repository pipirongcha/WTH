using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CollectionManager : MonoBehaviour
{
    public GameObject CodePopup; //수집한 코드의 버튼을 누르면 켜지는 코드팝업
    AudioSource audioSource;
    public AudioClip PopupClose;
    public AudioClip Back;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void BackButtonPressed()
    {
        audioSource.clip = Back;
        audioSource.Play();
        StartCoroutine(WaitAndLoadScene("TitleScene", Back.length));
}
    public void CloseButtonPressed()
    {
        audioSource.clip = PopupClose;
        audioSource.Play();
        CodePopup.SetActive(false);
    }
    IEnumerator WaitAndLoadScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); // 효과음 길이만큼 대기
        SceneManager.LoadScene(sceneName); // 씬 전환
    }


}
