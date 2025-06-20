using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageData : ScriptableObject
{
    public bool Locked 
    {
        get
        {
            // PlayerPrefs에서 잠금 상태를 불러옵니다 (기본값: true)
            return PlayerPrefs.GetInt(CodeTitleTXT + "_Locked", 1) == 1;
        }
        set
        {
            // PlayerPrefs에 잠금 상태를 저장합니다
            PlayerPrefs.SetInt(CodeTitleTXT + "_Locked", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public Sprite CodeImage;    //870*320
    public string CodeTitleTXT;
    public string CodeMainTXT;

    public bool IsItGood;
    public TextAsset StageCodeFile;
    public string StageNameCode;
    public string StageCode;

    private void OnValidate() //유니티 에디터 모드에서 호출(값 변경시), 에디터에서 필드값 검증 및 초기화, 동기화할 때 사용(=에디터에서 값 수정시 즉시 반영)
    {
        if (StageCodeFile != null)
        {
            StageCode = StageCodeFile.text;
        }
        else
        {
            StageCode = string.Empty;
        }
    }

    
    public string StageCodeMaking()
    {
        return "\n"+GameManager.Instance.Code + "\n"+StageNameCode+GameManager.Instance.CurrentStageNum + StageCode;
    }

    public string StageCodeRemaking()
    {
        return "\n" + GameManager.Instance.Code + "\n/*\n" + StageNameCode + GameManager.Instance.CurrentStageNum + StageCode+ "\n*/\n";
    }
}
