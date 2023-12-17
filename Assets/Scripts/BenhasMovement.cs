using UnityEngine;

public class BenhasMovement : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.LookAt(target);
        }
    }
}
