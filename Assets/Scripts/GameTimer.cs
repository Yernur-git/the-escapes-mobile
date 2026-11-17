using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] Text label;
    [SerializeField] LevelResult result;
    float remaining;
    bool running;

    public void Begin(float timeLimit)
    {
        remaining = timeLimit;
        running = true;
        RefreshLabel();
    }

    void Update()
    {
        if (!running || result.IsFinished) return;
        remaining = Mathf.Max(0f, remaining - Time.deltaTime);
        RefreshLabel();
        if (remaining <= 0f)
        {
            running = false;
            result.Finish(false);
        }
    }

    void RefreshLabel()
    {
        label.text = "time: " + Mathf.CeilToInt(remaining);
    }
}
