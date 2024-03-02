
using UnityEngine;
using UnityEngine.UI;

public class ArenaPortrait : MonoBehaviour
{
    [SerializeField] ArenaSO arena;
    [SerializeField] Image previewImageComponent;
    private Button button;

    public ArenaSO Arena => arena;

    private void Awake() {
        previewImageComponent.sprite = arena.smallPreviewImage;
        button = GetComponent<Button>();
    }

    public void SelectButton() {
        button.Select();
    }

}
