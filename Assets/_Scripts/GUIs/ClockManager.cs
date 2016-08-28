using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    private Vector3 _initialAngle;
    public Color TargetColorEndOfDay;

    void Start()
    {
        this.gameObject.transform.parent.gameObject.SetActive(false);
        _initialAngle = this.transform.rotation.eulerAngles;
        const float anglesPerHour = 180/8f; //180 angles in 8 hours
        GameManager.Instance.OnUpdateTime += () =>
        {
            this.transform.rotation = Quaternion.Euler(0, 0, _initialAngle.z + (-anglesPerHour * (GameManager.Instance.CurrentHour - 9)));
            var targetColor = Color.Lerp(Camera.main.backgroundColor, TargetColorEndOfDay, (1/8f) * (GameManager.Instance.CurrentHour - 9));
            DOTween.To(() => Camera.main.backgroundColor, (color) =>
            {
                Camera.main.backgroundColor = color;
            }, targetColor, .5f);
        };

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.name == "Work")
                this.gameObject.transform.parent.gameObject.SetActive(true);
            else
                this.gameObject.transform.parent.gameObject.SetActive(false);
        };
    }
}
