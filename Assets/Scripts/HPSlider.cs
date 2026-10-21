using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    public Slider slider;
    public Player player;
    public Slider easeSlider;
    public float lerpSpeed = 5f;

    private void Start()
    {
        if (player == null || slider == null || easeSlider == null)
        {
            Debug.LogError("HPSlider: Player or Slider references are missing!", this);
            enabled = false;
            return;
        }

        slider.minValue = player.MinHP;
        slider.maxValue = player.MaxHP;

        easeSlider.minValue = player.MinHP;
        easeSlider.maxValue = player.MaxHP;

        slider.value = player.CurrentHP;
        easeSlider.value = player.CurrentHP;
    }

    private void Update()
    {
        if (player == null || slider == null || easeSlider == null)
            return;

        slider.value = player.CurrentHP;

        easeSlider.value = Mathf.Lerp(
            easeSlider.value,
            player.CurrentHP,
            lerpSpeed * Time.deltaTime
        );
    }
}