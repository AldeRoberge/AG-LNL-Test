using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ResourcesLoader
{
    /// <summary>
    /// Provides an utility mapping of Object types to 'fallback' resources.
    /// </summary>
    public static class ResourcesFallback
    {
        private static readonly GameObject _gameObject;
        private static readonly Material _material;
        private static readonly AudioClip _audioClip;

        static ResourcesFallback()
        {
            _gameObject = ResourceLoader.Load<GameObject>("_Fallback/Material", false);
            _material = ResourceLoader.Load<Material>("_Fallback/Material", false);
            _audioClip = ResourceLoader.Load<AudioClip>("_Fallback/AudioClip", false);
        }

        public static Object GetFallback<T>()
        {
            if (typeof(T).IsAssignableFrom(typeof(GameObject)))
                return _gameObject;

            if (typeof(T).IsAssignableFrom(typeof(Material)))
                return _material;

            if (typeof(T).IsAssignableFrom(typeof(AudioClip)))
                return _audioClip;

            Debug.LogError("[ResourcesFallback] Could not find a fallback for the type : " + typeof(T) + ".");
            return default;
        }
    }
    
    public class ResourceLoader
    {
        public static T Load<T>(string path, bool tryFallback = true) where T : Object
        {
            if (!string.IsNullOrEmpty(path))
            {
                var Resource = Resources.Load<T>(path);

                if (Resource != null)
                    return Resource;

                Debug.Log(
                    "[LoadableResource] Resource loaded from '" + path + "' is null.\n" +
                    "Possible causes : The file extension is included in the resource file path.\n" +
                    "The path does not exist.");
            }

            Debug.LogError("[LoadableResource] Path for resource is null or empty.");

            if (tryFallback)
                return ResourcesFallback.GetFallback<T>() as T;

            return default;
        }
    }
}