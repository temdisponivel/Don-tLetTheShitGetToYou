using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    private Vector3 _initialAngle;
    public Color TargetColorEndOfDay;

    const float anglesPerHour = 180 / 8f; //180 angles in 8 hours

    void Start()
    {
        this.gameObject.transform.parent.gameObject.SetActive(false);
        _initialAngle = this.transform.rotation.eulerAngles;

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name == "Work")
            {
                this.gameObject.transform.parent.gameObject.SetActive(true);
                GameManager.Instance.OnUpdateTime += UpdateClock;
            }
            else
            {
                this.gameObject.transform.parent.gameObject.SetActive(false);
                GameManager.Instance.OnUpdateTime -= UpdateClock;
            }
        };
    }

    private void UpdateClock()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, _initialAngle.z + (-anglesPerHour * (GameManager.Instance.CurrentHour - 9)));
        var targetColor = Color.Lerp(Camera.main.backgroundColor, TargetColorEndOfDay, (1 / 8f) * (GameManager.Instance.CurrentHour - 9));
        DOTween.To(() => Camera.main.backgroundColor, (color) =>
                {
                    Camera.main.backgroundColor = color;
                }, targetColor, .5f);
    }
}
