using System.Collections;
using TMPro;
using UnityEngine;

public class TemporaryText : MonoBehaviour {
  public TextMeshProUGUI tmpText; 

  
  public void ShowTempText (string message, float duration) {
    StopAllCoroutines (); // Stop any existing timers
    StartCoroutine (DisplayRoutine (message, duration));
  }

  private IEnumerator DisplayRoutine (string message, float duration) {
    tmpText.text = message;
    tmpText.enabled = true;

    yield return new WaitForSeconds (duration);

    tmpText.text = ""; 
  }
}
