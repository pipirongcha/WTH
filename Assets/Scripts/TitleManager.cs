using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PopupCanvas;
    AudioSource audioSource;
    public AudioClip Click;
    public AudioClip GameStart;
    public AudioClip Quit;

    public GameObject InstPic;
    public Animator InstPicAnimator;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PowerPressed()
    {
        audioSource.clip = Quit;
        audioSource.Play();
        StartCoroutine(WaitAndQuit(Quit.length));
    }

    public void InstructionPressed()
    {
        audioSource.clip = Click;
        audioSource.Play();
        PopupCanvas.SetActive(true);
        InstPic.SetActive(true);
        InstPicAnimator.SetTrigger("InstructionPic");
    }

    public void PopupClosePressed()
    {
        audioSource.clip = Click;
        audioSource.Play();
        InstPicAnimator.SetTrigger("Inst_Idle");
        InstPic.SetActive(false);
        PopupCanvas.SetActive(false);
    }

    public void StartPressed()
    {
        audioSource.clip = GameStart;
        audioSource.Play();
        StartCoroutine(WaitAndLoadScene("GameScene",GameStart.length));
    }

    public void CollectionPressed()
    {
        audioSource.clip = Click;
        audioSource.Play();
        StartCoroutine(WaitAndLoadScene("CollectionScene", Click.length));
    }


    IEnumerator WaitAndQuit(float delay)
    {
        yield return new WaitForSeconds(delay); // 효과음 길이만큼 대기
        Application.Quit();
    }

    IEnumerator WaitAndLoadScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); // 효과음 길이만큼 대기
        SceneManager.LoadScene(sceneName); // 씬 전환
    }

}
