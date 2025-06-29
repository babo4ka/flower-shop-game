using UnityEngine;
using UnityEngine.EventSystems;

public class MoveScript : MonoBehaviour
{
    [SerializeField] GameObject camera;

    [SerializeField] MovingManager movingManager;
    [SerializeField] UIManager uiManager;

    [SerializeField] float duration;
    private float elapsedTime = -1;

    private Vector3 from;
    private Vector3 to;
    private Quaternion fromRotation;
    private Quaternion toRotation;

    [SerializeField] float backIndent = 24;
    [SerializeField] float xRotation = 30f;

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            movingManager.AddPosition(gameObject.name);
            MoveToPos();
        } 
    }

    private void Awake()
    {
        MovingManager.move += BackHere;
    }

    private void BackHere(string pos)
    {
        if (pos == gameObject.name)
        {
            Debug.Log($"here! {gameObject.name}");
            MoveToPos();
        }
    }

    private void MoveToPos()
    {
        if (gameObject.name == "CameraInitPos" || gameObject.name.Contains("mark"))
            uiManager.ToggleMainButtons("show");
        else
            uiManager.ToggleMainButtons("hide");

        from = camera.transform.position;
        to = new Vector3(transform.position.x, camera.transform.position.y, transform.position.z - backIndent);
        fromRotation = camera.transform.rotation;
        toRotation = Quaternion.Euler(xRotation, 0, 0);
        elapsedTime = 0;
        
    }

    private void Update()
    {
        if(elapsedTime != -1 && elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            camera.transform.SetPositionAndRotation(Vector3.Lerp(from, to, t), Quaternion.Lerp(fromRotation, toRotation, t));
        }
        else if (elapsedTime != -1 && elapsedTime > duration)
        {
            elapsedTime = -1;
        }
    }
}
