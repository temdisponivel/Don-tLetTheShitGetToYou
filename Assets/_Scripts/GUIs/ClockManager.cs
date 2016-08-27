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
        GameManager.Instance.OnUpdateTime += () =>
        {
            this.transform.rotation = Quaternion.Euler(_initialAngle + new Vector3(0, 0, (float) -(((15*8)/180f) * GameManager.Instance.CurrentHour)));
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
