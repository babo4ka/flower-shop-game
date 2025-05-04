using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] GameObject camera;

    [SerializeField] MovingManager movingManager;


    [SerializeField] float duration;
    private float elapsedTime = -1;

    private Vector3 from;
    private Vector3 to;
    private Quaternion fromRotation;
    private Quaternion toRotation;

    [SerializeField] float backIndent = 24;
    [SerializeField] float xRotation = 30f;

    private void OnMouseDown()
    {
        movingManager.AddPosition(gameObject.name);
        MoveToPos(1);
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
            MoveToPos(-1);
        }
    }

    private void MoveToPos(float side)
    {
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
            camera.transform.position = Vector3.Lerp(from, to, t);
            camera.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, t);
        }else if (elapsedTime != -1 && elapsedTime > duration)
        {
            elapsedTime = -1;
        }
    }
}
