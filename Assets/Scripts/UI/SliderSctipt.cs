using UnityEngine;
using UnityEngine.UI;

public class SliderSctipt : MonoBehaviour
{

    [SerializeField] Image progressBar;
    [SerializeField]private float totalTime;
    [SerializeField]private float currentTime;
    private bool started = false;


    private void Awake()
    {
        WorkDayManager.startDay += StartTimer;
    }

    void StartTimer(float duration)
    {
        totalTime = duration;
        currentTime = totalTime;
        progressBar.fillAmount = 1;
        started = true;
    }


    private void Update()
    {
        if(started)
        {
            currentTime -= Time.deltaTime;
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, currentTime / totalTime, Time.deltaTime * 10);

            if (currentTime <= totalTime * 0.3)
                progressBar.color = Color.red;

            if (currentTime <= 0)
                Destroy(gameObject);
        }
        
    }
}
