using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemGetPanel : MonoBehaviour
{
    public static ItemGetPanel Instance { get; private set; }

    public TextMeshProUGUI ItemNameLabel;
    public TextMeshProUGUI ItemDescriptionLabel;
    public Image ItemIcon;

    private void Awake()
    {
        //Set scale to 0 for it to be invisible before start
        Hide();

        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Show()
    {
        gameObject.transform.localScale = Vector3.one;
    }

    public void SetData(CraneGamePrizeData data)
    {
        ItemNameLabel.text = data.DisplayName;
        ItemDescriptionLabel.text = data.Description;
        ItemIcon.sprite = data.Icon;
    }

    public void Hide()
    {
        gameObject.transform.localScale = Vector3.zero;
    }
}
