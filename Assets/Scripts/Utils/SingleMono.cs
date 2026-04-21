using System;
using UnityEngine;

namespace Utils
{
    public class SingleMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        public bool destroyOnLoad;
        public static T Instance
        {
            get
            {
                if (!_instance) _instance = FindAnyObjectByType<T>();
                return _instance;
            }
        }
        private static T _instance;
        
        private void Awake()
        {
            if (_instance == this) return;
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this as T;
            if (!destroyOnLoad) DontDestroyOnLoad(gameObject.transform.root.gameObject);
        }

        private void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }
}
