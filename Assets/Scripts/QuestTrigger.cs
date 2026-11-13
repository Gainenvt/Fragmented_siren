
using UnityEngine;
using TMPro;

public class QuestTrigger : MonoBehaviour
{
    [Header("Quest UI")]
    [SerializeField] private GameObject questPopup;
    [SerializeField] private TMP_Text questProgress;

    [Header("Enemies")]
    [SerializeField] private Basesiren[] sirens;
    [SerializeField] private Dialog dialogue;

    private bool questStarted = false;

    private void StartQuest()
    {
        if (questStarted) return;

        Debug.Log("Player entered quest trigger!");

        questStarted = true;

        questPopup.SetActive(true);

        Debug.Log("Quest Popup Active: " + questPopup.activeSelf);
        Debug.Log("Quest Popup Visible in Hierarchy: " + questPopup.activeInHierarchy);
        dialogue.PlayDialogue(Dialog.DialogueSequence.CityKey);

        UpdateQuestProgress();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            StartQuest();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            StartQuest();
        }
    }

    private void Update()
    {
        if (questStarted && questPopup.activeSelf)
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
            questPopup.SetActive(false);
        }

    }



}


   