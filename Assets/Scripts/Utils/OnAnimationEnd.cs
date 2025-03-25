using UnityEngine;

public class OnAnimationEnd : MonoBehaviour
{
    void AnimationEnd()
    {
        Destroy(gameObject);
    }
}
