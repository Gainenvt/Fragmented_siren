using UnityEngine;
using UnityEngine.UI;
public class HPSlider : MonoBehaviour
{
     public Slider slider;
     public Player player;
     public Slider easeSlider;
     public float lerpSpeed = 5f;

     public void Start()
     {
         player = GetComponentInParent<Player>();
         slider.value = player.CurrentHP;
         easeSlider.value = player.CurrentHP;

     }

       public void Update()
{
    slider.value = player.CurrentHP;

    easeSlider.value = Mathf.Lerp(
        easeSlider.value,
        player.CurrentHP,
        lerpSpeed * Time.deltaTime
    );
}

}
