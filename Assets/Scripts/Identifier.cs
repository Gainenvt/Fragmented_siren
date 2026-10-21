using UnityEngine;
using TMPro;

public class ObjectIdentifier : MonoBehaviour
{
    public TMP_Text objectNameText;
    public float distance = 5f;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            TargetInfo target =
                hit.collider.GetComponentInParent<TargetInfo>();

            if (target != null)
            {
                objectNameText.gameObject.SetActive(true);
                objectNameText.text = target.displayName;
                return;
            }
        }

        objectNameText.gameObject.SetActive(false);
    }
}