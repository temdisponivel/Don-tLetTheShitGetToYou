using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockManager : MonoBehaviour
{
    private Vector3 _initialAngle;
    public Color TargetColorEndOfDay;

    void Start()
    {
        this.gameObject.transform.parent.gameObject.SetActive(false);
        _initialAngle = this.transform.rotation.eulerAngles;

        GameManager.Instance.OnSceneLoad += SceneChangeCallback;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnSceneLoad -= SceneChangeCallback;
    }

    private void UpdateClock()
    {
        var hoursPerDay = (ScriptableObjectHolder.Instance.GameConfiguration.HoursPerDay * 1.0f);
        float anglesPerHour = 180 / hoursPerDay; //180 angles in 8 hours
        this.transform.rotation = Quaternion.Euler(0, 0, _initialAngle.z + (-anglesPerHour * (GameManager.Instance.CurrentHour - 9)));
        var targetColor = Color.Lerp(Camera.main.backgroundColor, TargetColorEndOfDay, (1 / hoursPerDay) * (GameManager.Instance.CurrentHour - 9));
        DOTween.To(() => Camera.main.backgroundColor, (color) =>
                {
                    Camera.main.backgroundColor = color;
                }, targetColor, .5f);
    }

    private void SceneChangeCallback(string name)
    {
        if (name == "Work")
        {
            this.gameObject.transform.parent.gameObject.SetActive(true);
            
            GameManager.Instance.OnUpdateTime += UpdateClock;
        }
        else
        {
            this.gameObject.transform.parent.gameObject.SetActive(false);
            GameManager.Instance.OnUpdateTime -= UpdateClock;
        }
    }
}
