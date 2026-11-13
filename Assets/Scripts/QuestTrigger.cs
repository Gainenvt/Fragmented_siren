
using UnityEngine;
using TMPro;

public class QuestTrigger : MonoBehaviour
{
    [Header("Quest UI")]
    [SerializeField] private GameObject questPopup;
    [SerializeField] private TMP_Text questProgress;

    [Header("Enemies")]
    [SerializeField] private Basesiren[] sirens;

    private bool questStarted = false;

    private void Start()
    {
        questPopup.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
    

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered quest trigger!");

            questStarted = true;
            questPopup.SetActive(true);
            UpdateQuestProgress();
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

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.CompareTag("Player") && !questStarted)
        {
            Debug.Log("PLAYER DETECTED INSIDE QUEST TRIGGER!");

            questStarted = true;
            questPopup.SetActive(true);
            UpdateQuestProgress();
        }
    }


}
