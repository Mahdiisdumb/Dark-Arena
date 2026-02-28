using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTrans : MonoBehaviour
{
    public string NewLevel = "";
    public string Tag = "";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag))
        {
            SceneManager.LoadScene(NewLevel);
        }
    }
}