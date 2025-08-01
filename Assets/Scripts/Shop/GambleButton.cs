using UnityEngine;

public class GambleButton : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject bowl;
    [SerializeField] private GameObject ButtonsPanel;
    [SerializeField] private GameObject BetCounter;
    [SerializeField] private GameObject SpawnArea;

    [SerializeField] private GameObject BetButton;

    public void OnClick()
    {
        bowl.SetActive(true);
        BetButton.SetActive(true);
        SpawnArea.SetActive(true);
        BetCounter.SetActive(true);
        ButtonsPanel.SetActive(false);
        dialogueManager.ShowMessage("Чекаю пожертву...", true);
    }
}
