using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NGS.MeshFusionPro.Example
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        private int _frames;
        private float _elapsedSeconds;

        private void Update()
        {
            _frames++;
            _elapsedSeconds += Time.deltaTime;

            if (_elapsedSeconds > 1f)
            {
                _text.text = "FPS : " + _frames;

                _frames = 0;
                _elapsedSeconds = 0f;
            }
        }
    }
}
