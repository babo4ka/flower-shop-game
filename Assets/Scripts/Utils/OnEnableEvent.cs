using UnityEngine;

public class OnEnableEvent : MonoBehaviour
{
    public delegate void OnEnableDelegate();
    public OnEnableDelegate enabled;

    public delegate void OnDisableDelegate();
    public OnDisableDelegate disabled;

    private void OnEnable()
    {
        enabled?.Invoke();
    }


    private void OnDisable()
    {
        disabled?.Invoke();
    }
}
