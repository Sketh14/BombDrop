using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontLineDefense.Test
{
    public class CollisionTest_1 : MonoBehaviour
    {
        [SerializeField] private Transform[] _colliderObjects;
        // [SerializeField] private Rigidbody _colliderRigidObjects;
        public float FloatStrength = 0.05f, FloatSpeed = 2f;
        private const float _collidedObjectsYPos = 9.5f;

        private void Update()
        {
            for (int i = 0; i < _colliderObjects.Length; i++)
            {
                Vector3 newPos = _colliderObjects[i].position;
                newPos.y -= Mathf.Sin(FloatSpeed * Time.time) * FloatStrength;
                _colliderObjects[i].position = newPos;
            }
        }

        // private void FixedUpdate()
        // {
        //     Vector3 newPos = _colliderRigidObjects.position;
        //     newPos.y -= Mathf.Sin(FloatSpeed * Time.time) * FloatStrength;
        //     _colliderRigidObjects.MovePosition(newPos);
        // }

        // /*
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Triggered By Something | Other : {other.contactOffset}");
            // RaycastHit rayHit;
            for (int i = 0; i < _colliderObjects.Length; i++)
            {
                //This is totally different
                // if (Physics.CapsuleCast(_colliderObjects[i].position - new Vector3(0f, 0.75f, 0f),
                //         _colliderObjects[i].position + new Vector3(0f, 0.75f, 0f), 0.5f, _colliderObjects[i].up, out rayHit, 0.5f))
                // {
                //     Debug.Log($"Hit | Pos : {rayHit.point}");
                //     _colliderObjects[i].gameObject.SetActive(false);
                // }

                if (Physics.OverlapCapsule(_colliderObjects[i].position - new Vector3(0f, 0.75f, 0f),
                        _colliderObjects[i].position + new Vector3(0f, 0.75f, 0f),
                        0.5f) != null)
                {
                    _colliderObjects[i].gameObject.SetActive(false);
                    return;
                }
            }
        }
        // */


        /*private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Collided With Something | Name : {collision.GetContact(0).thisCollider.name}");
        }*/
    }
}
