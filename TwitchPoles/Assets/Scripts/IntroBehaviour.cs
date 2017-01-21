using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class IntroBehaviour : MonoBehaviour
    {
        TinyCoro _shake;

        void Awake()
        {
            _shake = TinyCoro.SpawnNext(ShakeArt);
            TinyCoro.SpawnNext(DoIntro);
        }

        private IEnumerator ShakeArt()
        {
            var art = transform.FindChild("Title Card");
            const float ShakeSpeed = 10f;
            const float ShakeAmount = 10f;
            while (true)
            {

                art.transform.localRotation = Quaternion.identity;
                art.transform.Rotate(Vector3.forward, Mathf.Sin(Time.time * ShakeSpeed) * ShakeAmount, Space.Self);

                yield return null;
            }
        }

        private IEnumerator DoIntro()
        {
            var height = 1000;

            var startPos = transform.position + (Vector3.down * height);

            float lerpTime = 8;

            for (float i = 0; i < lerpTime; i += Time.deltaTime)
            {
                var n = i / lerpTime;
                transform.position = startPos + (Vector3.up * height * n);
                yield return null;
            }

            yield return TinyCoro.WaitSeconds(5f);

            lerpTime = 5;

            //GameObject.Destroy(transform.FindChild("Text VS").gameObject);
            GameObject.Destroy(transform.FindChild("Text Presents").gameObject);

            for (float i = 0; i < lerpTime; i += Time.deltaTime)
            {
                var n = 1f - (i / lerpTime);
                transform.position = startPos + (Vector3.up * height * n);
                yield return null;
            }

            _shake.Kill();

            SceneManager.LoadScene("main");
        }

        void Update()
        {
            TinyCoro.StepAllCoros();            
        }
    }
}
