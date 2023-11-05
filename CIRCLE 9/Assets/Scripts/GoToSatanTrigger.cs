using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CameraFading;

public class GoToSatanTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Camera.main.gameObject.AddComponent<CameraFade>();
        CameraFade.Alpha = 0;
        if (other.gameObject.layer == 6)
        {
            CameraFade.Out(() =>
            {
                SceneManager.LoadScene(2); // satan scene
            }
            , 1f);
        }
    }
}
