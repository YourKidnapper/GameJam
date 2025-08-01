using UnityEngine;

public class GambleButton : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject bowl;
    [SerializeField] private GameObject ButtonsPanel;
    [SerializeField] private GameObject BetCounter;

    public void OnClick()
    {
        bowl.SetActive(true);
        BetCounter.SetActive(true);
        ButtonsPanel.SetActive(false);
        dialogueManager.ShowMessage("Чекаю пожертву...", true);
    }
}
