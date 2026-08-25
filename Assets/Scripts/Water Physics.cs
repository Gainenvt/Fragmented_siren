using UnityEngine;

public class WaterPhsiysc : MonoBehaviour
{   
   private void OnTriggerEnter(Collider other)
{
   

    Player player = other.GetComponent<Player>();

  

    if (player != null)
    {
        
        player.isSubmerged = true;
      
    }
}
   private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.isSubmerged = false;

        }
      
    }
}
