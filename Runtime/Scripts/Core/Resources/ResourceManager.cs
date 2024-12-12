using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
    public class ResourceManager : GameFrameworkComponent
    {
        private Dictionary<string, Object> k_ResourceCache = new Dictionary<string, Object>();
        private Dictionary<string, int> k_ReferenceCount = new Dictionary<string, int>();
        private Dictionary<string, AssetBundle> k_AssetBundleCache = new Dictionary<string, AssetBundle>();

        public override void OnInit()
        {
        }

        #region Load from Resources folder

        public T Load<T>(string path) where T : Object
        {
            if (k_ResourceCache.TryGetValue(path, out var value))
            {
                k_ReferenceCount[path]++;
                return (T)value;
            }

            T resource = Resources.Load<T>(path);
            if (resource != null)
            {
                k_ResourceCache[path] = resource;
                k_ReferenceCount[path] = 1;
            }
            else
            {
                Logg.LogError("Resource not found at path: " + path);
            }

            return resource;
        }

        public T LoadSO<T>(string path) where T : ScriptableObject
        {
            if (k_ResourceCache.TryGetValue(path, out var value))
            {
                k_ReferenceCount[path]++;
                return (T)value;
            }

            T resource = Resources.Load<T>(path);
            if (resource != null)
            {
                k_ResourceCache[path] = resource;
                k_ReferenceCount[path] = 1;
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

        #endregion

        #region Load from AssetBundle

        public IEnumerator LoadAssetFromBundle<T>(string bundlePath, string assetName, System.Action<T> onLoaded)
            where T : Object
        {
            if (!k_AssetBundleCache.TryGetValue(bundlePath, out var bundle))
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

                k_AssetBundleCache[bundlePath] = bundle;
            }

            var assetRequest = bundle.LoadAssetAsync<T>(assetName);
            yield return assetRequest;

            T asset = assetRequest.asset as T;
            if (asset != null)
            {
                string cacheKey = $"{bundlePath}/{assetName}";
                k_ResourceCache[cacheKey] = asset;
                k_ReferenceCount[cacheKey] = 1;
            }

            onLoaded?.Invoke(asset);
        }

        public void Unload(string path)
        {
            if (k_ResourceCache.TryGetValue(path, out var value))
            {
                k_ReferenceCount[path]--;
                if (k_ReferenceCount[path] <= 0)
                {
                    Resources.UnloadAsset(value);
                    k_ResourceCache.Remove(path);
                    k_ReferenceCount.Remove(path);
                }
            }
        }

        public void UnloadAssetBundle(string bundlePath, bool unloadAllLoadedObjects = false)
        {
            if (k_AssetBundleCache.ContainsKey(bundlePath))
            {
                k_AssetBundleCache[bundlePath].Unload(unloadAllLoadedObjects);
                k_AssetBundleCache.Remove(bundlePath);
            }
        }

        public void ClearCache()
        {
            foreach (var resource in k_ResourceCache.Values)
            {
                Resources.UnloadAsset(resource);
            }

            foreach (var bundle in k_AssetBundleCache.Values)
            {
                bundle.Unload(true);
            }

            k_ResourceCache.Clear();
            k_ReferenceCount.Clear();
            k_AssetBundleCache.Clear();
        }

        #endregion

        #region Load from Addressables
        #endregion
    }
}