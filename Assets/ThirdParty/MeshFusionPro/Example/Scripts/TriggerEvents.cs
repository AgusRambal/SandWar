using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NGS.MeshFusionPro.Example
{
    public class TriggerEvents : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _onEnter;

        [SerializeField]
        private UnityEvent _onExit;

        private void OnTriggerEnter(Collider other)
        {
            _onEnter.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            _onExit.Invoke();
        }
    }
}
