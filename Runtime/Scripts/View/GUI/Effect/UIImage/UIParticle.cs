using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Collections;

namespace OSK
{
    public class UIParticle : MonoBehaviour
    {
        private readonly Dictionary<string, Coroutine> _activeCoroutines = new Dictionary<string, Coroutine>();
        private List<GameObject> _parentEffects = new List<GameObject>();
        private List<EffectSetting> _effectSettings = new List<EffectSetting>();

        private Transform _canvasTransform;
        private Camera _mainCamera;
        private Camera _uiCamera;

        public enum ETypeSpawn
        {
            UIToUI,
            UIToWorld,
            WorldToUI,
            WorldToWorld,
            WorldToWorld3D,
        }

        private void Start() => Initialize();
         
        private void Initialize()
        {
            if (Main.Configs.init == null|| Main.Configs.init.data == null || Main.Configs.init.data.uiParticleSO == null)
                return;

            _mainCamera = Camera.main;
            _uiCamera = Main.UI.GetCanvas.worldCamera;
            _canvasTransform = Main.UI.GetCanvas.transform;

            _effectSettings = Main.Configs.init.data.uiParticleSO.EffectSettings.ToList();
            if (_effectSettings.Count == 0)
                return;

            _parentEffects = new List<GameObject>();
            for (int i = 0; i < _effectSettings.Count; i++)
            {
                _parentEffects.Add(new GameObject(_effectSettings[i].name));
                _parentEffects[i].transform.SetParent(_canvasTransform);
                _parentEffects[i].transform.localScale = Vector3.one;

                if (_effectSettings[i].typeMove == TypeMove.Beziers ||
                    _effectSettings[i].typeMove == TypeMove.CatmullRom ||
                    _effectSettings[i].typeMove == TypeMove.Path)
                {
                    AddPaths(_effectSettings[i]);
                }
            }
        }

        public void Spawn(ETypeSpawn typeSpawn, string name, Transform from, Transform to, int num = -1,
            System.Action onCompleted = null)
        {
            string key = $"{name}_{from.GetInstanceID()}_{to.GetInstanceID()}";
            if (_activeCoroutines.ContainsKey(key))
            {
                StopCoroutine(_activeCoroutines[key]);
                _activeCoroutines.Remove(key);
            }

            Coroutine coroutine = StartCoroutine(SpawnCoroutine(typeSpawn, name, from, to, num, onCompleted));
            _activeCoroutines[key] = coroutine;
        }

        private IEnumerator SpawnCoroutine(ETypeSpawn typeSpawn, string name, Transform from, Transform to, int num,
            System.Action onCompleted)
        {
            Vector3 fromPosition = from.position;
            Vector3 toPosition = to.position;
            bool is3D = false;

            switch (typeSpawn)
            {
                case ETypeSpawn.UIToWorld:
                    toPosition = ConvertToUICameraSpace(to);
                    break;
                case ETypeSpawn.WorldToUI:
                    fromPosition = ConvertToUICameraSpace(from);
                    break;
                case ETypeSpawn.WorldToWorld:
                    fromPosition = ConvertToUICameraSpace(from);
                    toPosition = ConvertToUICameraSpace(to);
                    break;
                case ETypeSpawn.WorldToWorld3D:
                    is3D = true;
                    break;
            }

            SpawnEffect(is3D, name, fromPosition, toPosition, num, onCompleted);
            yield return null;
        }

        private void SpawnEffect(bool is3D, string name, Vector3 from, Vector3 to, int num, System.Action onCompleted)
        {
            var effectSetting = _effectSettings.Find(x => x.name == name).Clone();
            effectSetting.pointSpawn = from;
            effectSetting.pointTarget = to;

            if (num > 0)
                effectSetting.numberOfEffects = num;
            effectSetting.OnCompleted = onCompleted;

            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(IESpawnEffect(is3D, effectSetting));
            }
        }

    
        private IEnumerator IESpawnEffect(bool is3D, EffectSetting effectSetting)
        {
            var parent = _parentEffects.Find(x => x.name == effectSetting.name)?.transform;
            if (parent == null || !parent.gameObject.activeInHierarchy)
                yield break;

            for (int i = 0; i < effectSetting.numberOfEffects; i++)
            {
                 var effect = Main.Pool.Spawn(KeyGroupPool.UIEffect, effectSetting.icon, _canvasTransform, 1);
                if (effect.transform.parent != parent)
                    effect.transform.SetParent(parent);
                
                effect.transform.position = effectSetting.pointSpawn;

                if (!is3D) effect.transform.localScale = Vector3.one;

                if (effectSetting.isDrop)
                {
                    DoDropEffect(effect, effectSetting);
                }
                else
                {
                    DoMoveTarget(effect, effectSetting);
                }
            }

            float totalTimeOnCompleted = effectSetting.timeDrop.TimeAverage +
                                         effectSetting.delayDrop.TimeAverage +
                                         effectSetting.timeMove.TimeAverage +
                                         effectSetting.delayMove.TimeAverage;
            yield return new WaitForSeconds(totalTimeOnCompleted - 0.1f);
            effectSetting.OnCompleted?.Invoke();
        }

        private void DoDropEffect(GameObject effect, EffectSetting effectSetting)
        { 
            Tween tween = effect.transform
                .DOMove(effectSetting.pointSpawn +
                        Random.insideUnitSphere * effectSetting.sphereRadius, effectSetting.timeDrop.RandomValue)
                .SetDelay( effectSetting.delayDrop.RandomValue);

            if (tween != null)
            {
                if (effectSetting.typeAnimationDrop == TypeAnimation.Ease)
                {
                    tween.SetEase(effectSetting.easeDrop);
                }
                else if (effectSetting.typeAnimationDrop == TypeAnimation.Curve)
                {
                    tween.SetEase(effectSetting.curveDrop);
                }
                else
                {
                    tween.SetEase(Ease.Linear);
                }

                tween.OnComplete(() => { DoMoveTarget(effect, effectSetting); });
            }
        }

        private void DoMoveTarget(GameObject effect, EffectSetting effectSetting)
        {
            Tween tween = null;
            var timeMove = effectSetting.timeMove.RandomValue;
            var timeMoveDelay = effectSetting.delayMove.RandomValue;

            switch (effectSetting.typeMove)
            {
                case TypeMove.Straight:
                    tween = effect.transform.DOMove(effectSetting.pointTarget, timeMove)
                        .SetDelay(timeMoveDelay);
                    break;
                case TypeMove.Beziers:
                    if (effectSetting.paths.Count % 3 != 0)
                    {
                        Logg.LogError("CubicBezier paths must contain waypoints in multiple of 3");
                        Main.Pool.Despawn(effect);
                        effectSetting.OnCompleted?.Invoke();
                        break;
                    }

                    tween = effect.transform
                        .DOPath(effectSetting.paths.Select(x => x).ToArray(), timeMove, PathType.CubicBezier)
                        .SetDelay(timeMoveDelay);
                    break;

                case TypeMove.CatmullRom:
                    if (effectSetting.paths.Count < 4)
                    {
                        Logg.LogError("CatmullRom paths must contain at least 4 waypoints");
                        Main.Pool.Despawn(effect);
                        effectSetting.OnCompleted?.Invoke();
                        break;
                    }

                    tween = effect.transform
                        .DOPath(effectSetting.paths.Select(x => x).ToArray(), timeMove, PathType.CatmullRom)
                        .SetDelay(timeMoveDelay);
                    break;

                case TypeMove.Path:
                    tween = effect.transform
                        .DOPath(effectSetting.paths.Select(x => x).ToArray(), timeMove, PathType.Linear)
                        .SetDelay(timeMoveDelay);
                    break;
                case TypeMove.DoJump:
                    tween = effect.transform
                        .DOJump(effectSetting.pointTarget, effectSetting.jumpPower.RandomValue,
                            effectSetting.jumpNumber, timeMove)
                        .SetDelay(timeMoveDelay);
                    break;
                case TypeMove.Around:
                    // Calculate a control point to define the curvature at the spawn point
                    Vector3 controlPoint = effectSetting.pointSpawn +
                                           new Vector3(effectSetting.midPointOffsetX.RandomValue,
                                               effectSetting.height.RandomValue,
                                               effectSetting.midPointOffsetZ.RandomValue);

                    // Create the path
                    Vector3[] path = new Vector3[]
                        { effectSetting.pointSpawn, controlPoint, effectSetting.pointTarget };
                    tween = effect.transform.DOPath(path, timeMove, PathType.CatmullRom)
                        .SetDelay(timeMoveDelay);
                    break;
                case TypeMove.Sin:
                    Vector3[] path1 = new Vector3[effectSetting.pointsCount];

                    for (int i = 0; i < effectSetting.pointsCount; i++)
                    {
                        float t = (float)i / (effectSetting.pointsCount - 1);
                        Vector3 point = Vector3.Lerp(effectSetting.pointSpawn, effectSetting.pointTarget, t);
                        point.y += Mathf.Sin(t * Mathf.PI * 2) * effectSetting.height.RandomValue;  
                        path1[i] = point;
                    }

                    tween = effect.transform.DOPath(path1, timeMove, PathType.CatmullRom)
                        .SetDelay(timeMoveDelay);
                    break; 
            }

            if (tween != null)
            {
                if (effectSetting.typeAnimationMove == TypeAnimation.Ease)
                    tween.SetEase(effectSetting.easeMove);
                else if (effectSetting.typeAnimationMove == TypeAnimation.Curve)
                    tween.SetEase(effectSetting.curveMove);
                else
                    tween.SetEase(Ease.Linear);

                // effect.transform.DOScale(effectSetting.scaleTarget, timeMove)
                //     .SetDelay(timeMoveDelay);
                tween.OnComplete(() => { Main.Pool.Despawn(effect); });
            }
        }

        private void AddPaths(EffectSetting effectSetting)
        {
            if (effectSetting.paths == null)
                effectSetting.paths = new List<Vector3>();
            if (!effectSetting.paths.Contains(effectSetting.pointSpawn))
                effectSetting.paths.AddFirstList(effectSetting.pointSpawn);
            if (!effectSetting.paths.Contains(effectSetting.pointTarget))
                effectSetting.paths.AddLastList(effectSetting.pointTarget);
        }

        public void DestroyEffect(string nameEffect)
        {
            if (Main.Pool.HasGroup(nameEffect))
            {
                Main.Pool.DestroyGroup(nameEffect);
            }
        }

        public void DestroyAllEffects()
        {
            Main.Pool.DestroyGroup(KeyGroupPool.UIEffect);
        }

        private Vector3 ConvertToUICameraSpace(Transform pointTarget)
        {
            Vector3 uiWorldPosition;
            if (Main.UI.GetCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                if (pointTarget is RectTransform rectTarget)
                {
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(
                        rectTarget, rectTarget.position, _uiCamera, out uiWorldPosition);
                }
                else uiWorldPosition = _mainCamera.WorldToScreenPoint(pointTarget.position);
            }
            else
            {
                Vector3 screenPoint = _mainCamera.WorldToScreenPoint(pointTarget.position);
                if (screenPoint.z < 0)
                {
                    Logg.LogWarning("Object is behind the main camera, unable to convert position.");
                    return Vector3.zero;
                }

                uiWorldPosition = _uiCamera.ScreenToWorldPoint(screenPoint);
                uiWorldPosition.z = 0;
            }
            return uiWorldPosition;
        }
    }
}