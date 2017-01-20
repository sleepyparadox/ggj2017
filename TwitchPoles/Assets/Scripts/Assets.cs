using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Assets
    {
        static Dictionary<string, object> _loaded = new Dictionary<string, object>();
        public static T Spawn<T>(string path)
            where T : UnityEngine.Object
        {
            if (_loaded.ContainsKey(path))
                return ((T)_loaded[path]).Clone();

            var asset = Resources.Load<T>(path);
            _loaded.Add(path, asset);
            return asset.Clone();
        }

        static T Clone<T>(this T src)
            where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate<T>(src);
        }
    }
}
