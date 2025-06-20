using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionButton : MonoBehaviour
{
    public GameObject CodePopup;
    public StageData CollectData;
    public TextMeshProUGUI PopupTitleTXT; //코드팝업에 쓰일 코드제목
    public TextMeshProUGUI PopupMainTXT; //코드팝업에 쓰일 코드내용
    public Image PopupImage; //코드팝업에 쓰일 코드캡쳐 이미지
    AudioSource audioSource ;
    public AudioClip InvaildClick;
    public AudioClip PopupOpen;

    private Button button;
    public TextMeshProUGUI ButtonText;
    private Image buttonImage;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();

        // 버튼 클릭 이벤트 등록
        button.onClick.AddListener(OnButtonClick);

        if(!CollectData.Locked)
        {
            ButtonText.text = CollectData.CodeTitleTXT;
            buttonImage.sprite = CollectData.CodeImage;
        }
    }

    void OnButtonClick()
    {
        if (CollectData.Locked)
        {
            audioSource.clip = InvaildClick;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = PopupOpen;
            audioSource.Play();
            PopupImage.sprite = CollectData.CodeImage;
            PopupTitleTXT.text = CollectData.CodeTitleTXT;
            PopupMainTXT.text = CollectData.CodeMainTXT;
            CodePopup.SetActive(true);
        }
           
    }
}
