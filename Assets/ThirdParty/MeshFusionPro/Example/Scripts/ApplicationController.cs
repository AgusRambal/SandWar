using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.MeshFusionPro.Example
{
    public class ApplicationController : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 300;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }
}
