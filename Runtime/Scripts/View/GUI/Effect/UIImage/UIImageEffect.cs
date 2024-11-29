using UnityEngine;
using System.Linq;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
    public class UIImageEffect : MonoBehaviour
    {
        private EffectSetting[] _effectSettings;
        private List<GameObject> _parentEffects;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (Main.Configs.Game.data.isUseUIImage == false)
                return;
            if (Main.Configs.Game.data.uiImageSO == null)
                return;

            _effectSettings = Main.Configs.Game.data.uiImageSO.EffectSettings;
            if (_effectSettings.Length == 0)
                return;

            _parentEffects = new List<GameObject>();
            for (int i = 0; i < _effectSettings.Length; i++)
            {
                _parentEffects.Add(new GameObject(_effectSettings[i].name));
                _parentEffects[i].transform.SetParent(Main.UI.GetCanvas.transform);
                _parentEffects[i].transform.localScale = Vector3.one;
                AddPaths(_effectSettings[i]);
            }
        }

        public void SpawnEffect(string nameEffect, Vector3 pointSpawn, Vector3 pointTarget)
        {
            var effectSetting = _effectSettings.ToList().Find(x => x.name == nameEffect);
            effectSetting.pointSpawn = pointSpawn;
            effectSetting.pointTarget = pointTarget;

            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(IESpawnEffect(effectSetting));
            }
        }

        public void SpawnEffect(string nameEffect, Vector3 pointSpawn, Vector3 pointTarget, int numberOfEffects,
            System.Action OnCompleted)
        {
            var effectSetting = _effectSettings.ToList().Find(x => x.name == nameEffect);
            effectSetting.pointSpawn = pointSpawn;
            effectSetting.pointTarget = pointTarget;

            if (numberOfEffects > 0)
                effectSetting.numberOfEffects = numberOfEffects;
            effectSetting.OnCompleted = OnCompleted;

            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(IESpawnEffect(effectSetting));
            }
        }

        private IEnumerator IESpawnEffect(EffectSetting effectSetting)
        {
            var parent = _parentEffects.Find(x => x.name == effectSetting.name)?.transform;
            if (parent == null || !parent.gameObject.activeInHierarchy)
                yield break;

            for (int i = 0; i < effectSetting.numberOfEffects; i++)
            {
                var effect = Main.Pool.Spawn(KeyGroupPool.UIEffect, effectSetting.icon, 1);
                effect.gameObject.GetOrAdd<RectTransform>();
                effect.transform.SetParent(parent);
                effect.transform.localScale = Vector3.one;
                effect.transform.position = effectSetting.pointSpawn;

                if (effectSetting.isDrop)
                {
                    DoDropEffect(effect, effectSetting);
                }
                else
                {
                    DoMoveTarget(effect, effectSetting);
                }
            }

            var timedrop = (effectSetting.timeDrop.max + effectSetting.timeDrop.min) / 2;
            var timeDropDelay = (effectSetting.delayDrop.max + effectSetting.delayDrop.min) / 2;
            var timeMove = (effectSetting.timeMove.max + effectSetting.timeMove.min) / 2;
            var timeMoveDelay = (effectSetting.delayMove.max + effectSetting.delayMove.min) / 2;

            var totalTimeOnCompleted = timedrop + timeDropDelay + timeMove + timeMoveDelay;
            yield return new WaitForSeconds(totalTimeOnCompleted - 0.1f);
            effectSetting.OnCompleted?.Invoke();
        }

        private void DoDropEffect(GameObject effect, EffectSetting effectSetting)
        {
            Vector3 randomOffset = Random.insideUnitSphere * effectSetting.sphereRadius;
            Vector3 target = effectSetting.pointSpawn + randomOffset;
            var timeDrop = effectSetting.timeDrop.RandomValue;
            var timeDropDelay = effectSetting.delayDrop.RandomValue;

            Tween tween = effect.transform
                .DOMove(target, timeDrop)
                .SetDelay(timeDropDelay);

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
                    Vector3 controlPoint = effectSetting.pointSpawn + new Vector3(effectSetting.midPointOffsetX.RandomValue, effectSetting.height.RandomValue, effectSetting.midPointOffsetZ.RandomValue);

                    // Create the path
                    Vector3[] path = new Vector3[] { effectSetting.pointSpawn, controlPoint, effectSetting.pointTarget };
                    tween = effect.transform.DOPath(path, timeMove, PathType.CatmullRom)
                        .SetDelay(timeMoveDelay);
                    break;
                case TypeMove.Sin: 
                    Vector3[] path1 = new Vector3[effectSetting.pointsCount];

                    for (int i = 0; i < effectSetting.pointsCount; i++)
                    {
                        float t = (float)i / (effectSetting.pointsCount - 1);
                        Vector3 point = Vector3.Lerp(effectSetting.pointSpawn, effectSetting.pointTarget, t);
                        point.y += Mathf.Sin(t * Mathf.PI * 2) * effectSetting.height.RandomValue; // Apply sine wave offset
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

                effect.transform.DOScale(effectSetting.scaleTarget, timeMove)
                    .SetDelay(timeMoveDelay);
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
            var parent = _parentEffects.Find(x => x.name == nameEffect)?.transform;
            if (parent == null)
                return;
            Main.Pool.DestroyGroup(nameEffect);
        }

        public void DestroyAllEffects()
        {
            Main.Pool.DestroyGroup(KeyGroupPool.UIEffect);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // if (Application.isEditor)
            //     return;

            if (_effectSettings == null)
                return;
            if (_effectSettings.Length == 0)
                return;

            Color color = Color.magenta;
            for (int i = 0; i < _effectSettings.Length; i++)
            {
                if (_effectSettings[i].isDrop)
                {
                    if (_effectSettings[i].pointSpawn == null)
                        continue;

                    Gizmos.color = color;
                    Gizmos.DrawWireSphere(_effectSettings[i].pointSpawn, _effectSettings[i].sphereRadius);
                }

                switch (_effectSettings[i].typeMove)
                {
                    case TypeMove.Straight:
                        Gizmos.color = color;
                        if (_effectSettings[i].pointSpawn == null)
                            continue;
                        Gizmos.DrawLine(_effectSettings[i].pointSpawn, _effectSettings[i].pointTarget);
                        break;
                    case TypeMove.Beziers:
                        if (_effectSettings[i].pointSpawn == null)
                            continue;
                        for (int j = 0; j < _effectSettings[i].paths.Count - 3; j += 3)
                        {
                            Vector3 startPoint = _effectSettings[i].paths[j];
                            Vector3 controlPoint1 = _effectSettings[i].paths[j + 1];
                            Vector3 controlPoint2 = _effectSettings[i].paths[j + 2];
                            Vector3 endPoint = _effectSettings[i].paths[j + 3];

                            DrawCubicBezierCurve(startPoint, controlPoint1, controlPoint2, endPoint, Gizmos.color);
                        }

                        break;
                    case TypeMove.Path:
                        if (_effectSettings[i].pointSpawn == null)
                            continue;
                        if (_effectSettings[i].paths.Count < 2)
                        {
                            Logg.LogError("Path is not enough");
                        }

                        for (int j = 0; j < _effectSettings[i].paths.Count - 1; j++)
                        {
                            Gizmos.color = color;
                            Gizmos.DrawLine(_effectSettings[i].paths[j],
                                _effectSettings[i].paths[j + 1]);
                        }

                        break;
                }
            }
        }

        private void DrawCubicBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Color color,
            int segments = 20)
        {
            Vector3 previousPoint = p0;

            for (int i = 1; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector3 pointOnCurve = OSK.MathUtils.CalculateCubicBezierPoint(t, p0, p1, p2, p3);
                Gizmos.DrawLine(previousPoint, pointOnCurve);
                previousPoint = pointOnCurve;
            }
        }
#endif
    }
}