using System.Collections;
using System.Collections.Generic;
using FrontLineDefense.Global;
using UnityEngine;

namespace FrontLineDefense
{
    public class LookRotationTest : MonoBehaviour
    {
        private Vector3 _SpeedVec;
        protected float _turnSpeed = 0.75f;

        // Update is called once per frame
        void Update()
        {
            _SpeedVec = (GameManager.Instance.PlayerTransform.position - transform.position).normalized;
            float zRotationAngle = Mathf.Atan2(_SpeedVec.y, _SpeedVec.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.LookRotation(_SpeedVec);
            // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_SpeedVec), _turnSpeed * Time.deltaTime);      //Not Effective
            // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, zRotationAngle - 180), _turnSpeed * Time.deltaTime);      //Not Effective
        }
    }
}
