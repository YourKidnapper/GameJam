using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    public static TooltipController Instance;

    public RectTransform tooltipPanel;
    public TMP_Text skillNameText;
    public TMP_Text skillDescriptionText;

    private void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    public void ShowTooltip(string name, string description, RectTransform target)
    {
        tooltipPanel.gameObject.SetActive(true);
        skillNameText.text = name;
        skillDescriptionText.text = description;

        // Позиціонуємо над іконкою
        Vector3 newPos = target.position; // світова позиція іконки
        newPos.y += 180f; // зсув вгору
        tooltipPanel.position = newPos;
    }

    public void HideTooltip()
    {
        tooltipPanel.gameObject.SetActive(false);
    }

}
