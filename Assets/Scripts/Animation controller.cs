using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator animator;

    public  void Awake()
    {
       animator = GetComponent<Animator>();
    }


}