using UnityEngine;

[CreateAssetMenu(fileName = "TutorialInfo", menuName = "Tutorial/Tutorial Info")]
public class TutorialInfo : ScriptableObject
{
    [TextArea]
    public string tutorialText;

    public Sprite tutorialImage; // Opsiyonel, resim eklemek için
}