
using UnityEngine;
using TMPro;

public class QuestTrigger : MonoBehaviour
{
    [Header("Quest UI")]
    [SerializeField] private GameObject questPopup;
    [SerializeField] private TMP_Text questProgress;

    [Header("Enemies")]
    [SerializeField] private Basesiren[] sirens;

    [Header("Dialogue")]
    [SerializeField] private Dialog dialogue;
    [SerializeField] private Dialog.DialogueSequence dialogueSequence;

    private bool questStarted = false;
    private bool questCompleted = false;

    private void StartQuest()
    {
        if (questStarted || questCompleted) return;

        Debug.Log("Player entered quest trigger!");

        questStarted = true;

        questPopup.SetActive(true);

        dialogue.PlayDialogue(dialogueSequence);

        UpdateQuestProgress();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            StartQuest();
        }
    }

    private void Update()
    {
        if (questStarted && !questCompleted)
        {
            UpdateQuestProgress();
        }
    }

    private void UpdateQuestProgress()
    {
        if (sirens == null || sirens.Length == 0)
        {
            Debug.LogWarning("No enemies assigned to QuestTrigger!");
            return;
        }

        int defeated = 0;

        foreach (Basesiren siren in sirens)
        {
            if (siren == null || siren.isEnemyDead)
            {
                defeated++;
            }
        }

        questProgress.text = defeated + "/" + sirens.Length;

        if (defeated >= sirens.Length)
        {
            questCompleted = true;
            questPopup.SetActive(false);

            Debug.Log("Quest completed!");
        }
    }
}
