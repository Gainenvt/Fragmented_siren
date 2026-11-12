
using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    public Slider slider;
    public Player player;
    public Slider easeSlider;
    public float lerpSpeed = 5f;
    [SerializeField] private HitEffect hitEffect;
    void Start()
    {
        // Only search for Player if one wasn't assigned
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }

        // Check our references.
        if (player == null)
        {
            Debug.LogError("HP Slider: Player reference is missing!");
            return;
        }

        if (slider == null || easeSlider == null)
        {
            Debug.LogError("HP Slider: Slider references are missing!");
            return;
        }

        slider.value = player.CurrentHP;
        easeSlider.value = player.CurrentHP;

        Debug.Log("HP Slider initialized. Player HP: " + player.CurrentHP);
    }

    void Update()
    {
        if (player == null || slider == null || easeSlider == null)
            return;

        // Main HP bar updates immediately.
        slider.value = player.CurrentHP;

        // Ease bar follows gradually.
        easeSlider.value = Mathf.Lerp(
            easeSlider.value,
            player.CurrentHP,
            lerpSpeed * Time.deltaTime
        );
    }
}
