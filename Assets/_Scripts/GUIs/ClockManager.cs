using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    private Vector3 _initialAngle;

    void Start()
    {
        this.gameObject.transform.parent.gameObject.SetActive(false);
        _initialAngle = this.transform.rotation.eulerAngles;
        const float anglesPerHour = 180/8f; //180 angles in 8 hours
        GameManager.Instance.OnUpdateTime += () =>
        {
            this.transform.rotation = Quaternion.Euler(0, 0, _initialAngle.z + (-anglesPerHour * (GameManager.Instance.CurrentHour - 9)));
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
