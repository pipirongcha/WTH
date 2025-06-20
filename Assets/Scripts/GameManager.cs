using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI CodeBox; //Code가 적히는 TMPro

    public List<StageData> AllStages = new List<StageData>(); //전체 게임 스테이지

    public int CurrentStageNum = 1; //게임 내 스테이지명에 쓰일 현재의 스테이지
    public bool goodCode; //현재 스테이지 코드의 동작 가능 여부를 확인해주는 bool값
    public string Code; //코드
    Queue<StageData> SelectedStages = new Queue<StageData>(); //선택된 게임 스테이지
    public StageData CurrentStage; //현재 스테이지
    public int CurrentCodeLen; //코드의 길이
    public int TryNum = 1; //게임 내 클래스명에 쓰일 Try 횟수

 
    public Button ExecuteButton; //실행버튼
    public Button WTHButton; //님대체뭐함버튼
    public TextMeshProUGUI ExecuteText;
    public TextMeshProUGUI WTHText;
    public Animator JudgeAnimator; // 정답 오답 판별 애니메이터
    public Animator RightAnimator; //정답 애니메이터
    public Animator WrongAnimator; //오답 애니메이터

    public int CodeCheckNum; //이제까지 이 게임을 플레이하면서 확인한 코드의 개수
    public int LockedCodeNum;

    public Animator GPT;
    public Animator Paint;
    public Animator Doodle;

    public GameObject GPT_obj;
    public GameObject Paint_obj;
    public GameObject Doodle_obj;

    public ScrollRect ScrollRect;

    AudioSource audioSource;
    public AudioClip RightSound;
    public AudioClip WrongSound;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < AllStages.Count; i++)
        {
            if (AllStages[i].Locked)
            {
                LockedCodeNum++;
            }
        }
        CodeCheckNum = PlayerPrefs.GetInt("CodeCheckNum", 0);
        for (int i = 0; i < 8; i++)
        {
            int randomNum = Random.Range(0, AllStages.Count);
            SelectedStages.Enqueue(AllStages[randomNum]);
            AllStages.RemoveAt(randomNum);
        }
        CurrentCodeLen = 0;
        Code = "using System; \nusing System.Collections.Generic; \n\npublic class Try_" + TryNum + "\n{";
        LoadStage();
    }

    IEnumerator Typing(string code)
    {
            for (int i = CurrentCodeLen; i < code.Length; i++)
            {
                CodeBox.text += code[i] + "";
                ScrollDown();
                yield return new WaitForSeconds(0.01f);
            }

            CurrentCodeLen = code.Length;
            ExecuteButton.interactable = true;
            WTHButton.interactable = true;
            RestoreButtonTXTColor(); 
    }
   void Erase()
    {
        CodeBox.text = "";
    }


    void LoadStage()
    {
        CurrentStage = SelectedStages.Dequeue();

        switch (CurrentStage.CodeTitleTXT)
        {
            case "코드 대신 짜줘(G)" or "코드 대신 짜줘(B)":
                JudgeIdleTrigger();
                GPTAniCode();
                break;

            case "그림판 코딩(B)":
                JudgeIdleTrigger();
                PaintAniCode();
                break;

            case "낙서 주의(B)":
                JudgeIdleTrigger();
                DoodleAniCode();
                break;

            default:
                JudgeIdleTrigger();
                goodCode = CurrentStage.IsItGood;
                StartCoroutine(Typing(CurrentStage.StageCodeMaking()));
                break;
        }
    }

    void TextReload()
    {
        CodeBox.text = Code.Substring(0, Code.Length - 1);
        Code = Code.Substring(0, Code.Length - 1);
    }


    public void ExecutePressed()
    {
        CodeCheckNum++;
        PlayerPrefs.SetInt("CodeCheckNum",CodeCheckNum);
        PlayerPrefs.Save();
        if (goodCode)
        {
            Correct();
        }
        else
        {
            Wrong();
        }
    }
    public void WTHPressed()
    {
        CodeCheckNum++;
        PlayerPrefs.SetInt("CodeCheckNum", CodeCheckNum);
        PlayerPrefs.Save();
        if (goodCode)
        {
            Wrong();
        }
        else
        {
            Fix();
        }
    }

    void Correct()
    {
        Code = CurrentStage.StageCodeMaking();
        TextReload();
        RightAnswer();

    }

    void Fix()
    {
        CodeBox.text = CurrentStage.StageCodeRemaking();
        Code = CurrentStage.StageCodeRemaking();
        CurrentCodeLen = Code.Length+1;
        RightAnswer();
    }

    void RightAnswer()
    {
        audioSource.clip = RightSound;
        audioSource.Play();
        if (CurrentStage.CodeTitleTXT == "그림판 코딩(B)")
        {
            Paint.SetTrigger("Cleaner");
        }
        if (CurrentStage.CodeTitleTXT == "낙서 주의(B)")
        {
            Doodle_obj.SetActive(false);
        }
        JudgeAnimator.SetTrigger("True");
        RightAnimator.SetTrigger("Success");
        ExecuteButton.interactable = false;
        WTHButton.interactable = false;
        ChangeButtonTXTColor();
        CurrentStageNum++;
        if (CurrentStage.Locked)
        {
            PlayerPrefs.SetInt(CurrentStage.name + "_Locked", 0);
            PlayerPrefs.Save();
            CurrentStage.Locked = false;
            LockedCodeNum--;
        }
        if (CurrentStageNum <= 8)
        {
            SetStage();
            LoadStage();
        }
        else
        {
            SceneManager.LoadScene("EndingScene");
        }
    }

    void Wrong()
    {
        if (CurrentStage.CodeTitleTXT == "그림판 코딩(B)")
        {
            Paint.SetTrigger("Cleaner");
        }
        if (CurrentStage.CodeTitleTXT == "낙서 주의(B)")
        {
            Doodle_obj.SetActive(false);
        }
        audioSource.clip = WrongSound;
        audioSource.Play();
        JudgeAnimator.SetTrigger("False");
        WrongAnimator.SetTrigger("Fail");
        Erase();
        ExecuteButton.interactable = false;
        WTHButton.interactable = false;
        ChangeButtonTXTColor();
        SetStage();
        TryNum++;
        CurrentStageNum = 1;
        CurrentCodeLen = 0;
        Code = "using System; \nusing System.Collections.Generic; \n\npublic class Try_" + TryNum + "\n{";
        Invoke("LoadStage", CodeBox.text.Length * 0.012f);
    }

    void SetStage()
    {
        int randomNum = Random.Range(0, AllStages.Count - 1);
        SelectedStages.Enqueue(AllStages[randomNum]);
        AllStages.RemoveAt(randomNum);
        AllStages.Add(CurrentStage);
    }


     void GPTAniCode()
    {
        GPT.SetTrigger("GPT");
        goodCode = CurrentStage.IsItGood;
        //후작업들은 GPT End에서 실행해줄거임!
    }


     void PaintAniCode()
    {
        Paint.SetTrigger("Paint");
        goodCode = CurrentStage.IsItGood;
    }

     void DoodleAniCode()
    {
        goodCode = CurrentStage.IsItGood;
        StartCoroutine(Typing(CurrentStage.StageCodeMaking()));
        Doodle_obj.SetActive(true);
        Doodle.SetTrigger("Doodle");

    }

    void AniCleaner()
    {

        Doodle_obj.SetActive(false);
    }



    public void ScrollDown()
    {

        Canvas.ForceUpdateCanvases();
        ScrollRect.verticalNormalizedPosition = 0f;
    }

    void ChangeButtonTXTColor()
    {
        ExecuteText.color = new Color32(15,111,50,255);
        WTHText.color = new Color32(119,2,2,255);

    }

    public void RestoreButtonTXTColor()
    {
        ExecuteText.color = new Color32(36, 239, 114, 255);
        WTHText.color = new Color32(255, 0, 0, 255);
    }

    void JudgeIdleTrigger()
    {
        RightAnimator.SetTrigger("Success_Idle");
        WrongAnimator.SetTrigger("Fail_Idle");

    }
}
