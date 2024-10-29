using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
    public class ResourceManager : GameFrameworkComponent
    {
        private Dictionary<string, Object> _resourceCache = new Dictionary<string, Object>();
        private Dictionary<string, int> _referenceCount = new Dictionary<string, int>();
        private Dictionary<string, AssetBundle> _assetBundleCache = new Dictionary<string, AssetBundle>();
 
        public override void OnInit() {}

        // Load from Resources folder
        public T Load<T>(string path) where T : Object
        {
            if (_resourceCache.TryGetValue(path, out var value))
            {
                _referenceCount[path]++;
                return (T)value;
            }

            T resource = Resources.Load<T>(path);
            if (resource != null)
            {
                _resourceCache[path] = resource;
                _referenceCount[path] = 1;
            }
            else
            {
                Logg.LogError("Resource not found at path: " + path);
            } 
            return resource;
        }

        public T LoadSO<T>(string path) where T : ScriptableObject
        {
            if (_resourceCache.TryGetValue(path, out var value))
            {
                _referenceCount[path]++;
                return (T)value;
            }

            T resource = Resources.Load<T>(path);
            if (resource != null)
            {
                _resourceCache[path] = resource;
                _referenceCount[path] = 1;
            }
            else
            {
                Logg.LogError("Resource not found at path: " + path);
            }
            return resource;
        }

        public T Spawn<T>(string path) where T : Object
        {
            T resource = Load<T>(path);
            if (resource != null)
            {
                return Instantiate(resource) as T;
            }
            else
            {
                OSK.Logg.LogError("Failed to spawn resource at path: " + path);
                return null;
            }
        } 
        
        // Load from AssetBundle
        public IEnumerator LoadAssetFromBundle<T>(string bundlePath, string assetName, System.Action<T> onLoaded)
            where T : Object
        {
            AssetBundle bundle = null;
            if (!_assetBundleCache.TryGetValue(bundlePath, out bundle))
            {
                var bundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
                yield return bundleRequest;

                bundle = bundleRequest.assetBundle;
                if (bundle == null)
                {
                    OSK.Logg.LogError("Failed to load AssetBundle from path: " + bundlePath);
                    onLoaded?.Invoke(null);
                    yield break;
                }

                _assetBundleCache[bundlePath] = bundle;
            }

            var assetRequest = bundle.LoadAssetAsync<T>(assetName);
            yield return assetRequest;

            T asset = assetRequest.asset as T;
            if (asset != null)
            {
                string cacheKey = $"{bundlePath}/{assetName}";
                _resourceCache[cacheKey] = asset;
                _referenceCount[cacheKey] = 1;
            }

            onLoaded?.Invoke(asset);
        }

        // Unload an asset
        public void Unload(string path)
        {
            if (_resourceCache.TryGetValue(path, out var value))
            {
                _referenceCount[path]--;
                if (_referenceCount[path] <= 0)
                {
                    Resources.UnloadAsset(value);
                    _resourceCache.Remove(path);
                    _referenceCount.Remove(path);
                }
            }
        }

        // Unload an entire AssetBundle
        public void UnloadAssetBundle(string bundlePath, bool unloadAllLoadedObjects = false)
        {
            if (_assetBundleCache.ContainsKey(bundlePath))
            {
                _assetBundleCache[bundlePath].Unload(unloadAllLoadedObjects);
                _assetBundleCache.Remove(bundlePath);
            }
        }

        // Clear all caches
        public void ClearCache()
        {
            foreach (var resource in _resourceCache.Values)
            {
                Resources.UnloadAsset(resource);
            }

            foreach (var bundle in _assetBundleCache.Values)
            {
                bundle.Unload(true);
            }

            _resourceCache.Clear();
            _referenceCount.Clear();
            _assetBundleCache.Clear();
        }
    }
}