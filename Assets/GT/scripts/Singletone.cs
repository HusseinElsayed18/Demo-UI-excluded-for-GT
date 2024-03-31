using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GT
{
    public class Singletone<T> : MonoBehaviour
    {
        public static Singletone<T> instance;
        private void Awake()
        {
            instance = this;
            if (instance == null)
            {
                Debug.LogError("there is an error");
            }
        }
    }
}

