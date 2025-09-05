using UnityEngine;

[CreateAssetMenu(fileName = "Readme", menuName = "Project/Readme")]
public class Readme : ScriptableObject
{
    [TextArea(5, 20)]
    public string text; // Proje hakkında bilgiler için

    public Sprite image; // Opsiyonel: Görsel eklemek için
}