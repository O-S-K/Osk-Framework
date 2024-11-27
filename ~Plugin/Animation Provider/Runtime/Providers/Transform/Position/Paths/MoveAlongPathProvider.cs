using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent]
    public class MoveAlongPathProvider : DoTweenBaseProvider
    {
        public List<Path> paths = new List<Path>();
        public PathType typePath = PathType.CatmullRom;
        public PathMode pathMode = PathMode.Full3D;

        public bool isClosedPath = false;
        public bool isLocal = true;
        public bool isStartFirstPoint = true;
        public bool isLookAt = false;

        public override void ProgressTween()
        {
            if (paths == null || paths.Count == 0)
            {
                Logg.LogWarning("Path points are not defined!");
                return;
            }

            if (isStartFirstPoint)
            {
                transform.position = paths[0].position;
                transform.eulerAngles = paths[0].rotation.eulerAngles;
            }

            Vector3[] pathPositions;
            if (isClosedPath)
            {
                pathPositions = paths.Select(p => p.position).ToArray().Concat(new Vector3[] { paths[0].position })
                    .ToArray();
            }
            else
            {
                pathPositions = paths.Select(p => p.position).ToArray();
            }

            PathType pathType = typePath == PathType.Linear ? PathType.Linear : PathType.CatmullRom;
            bool applyLookAt = isLookAt || !isLocal;

            if (isLocal)
            {
                tweener = (applyLookAt)
                    ? transform.DOLocalPath(pathPositions, settings.duration, pathType).SetLookAt(0.01f)
                    : transform.DOLocalPath(pathPositions, settings.duration, pathType);
            }
            else
            {
                tweener = (applyLookAt)
                    ? transform.DOPath(pathPositions, settings.duration, pathType).SetLookAt(0.01f)
                    : transform.DOPath(pathPositions, settings.duration, pathType);
            }
            
            base.ProgressTween();
        }

  
        public override void Play()
        {
            base.Play();
        }


        public void AddPathPoint()
        {
            var idx = paths.Count;
            Path newPath = new Path(idx, transform.position, transform.rotation);
            paths.Add(newPath);
        }

        public void AddPathPoint(Vector3 position, Quaternion rotation)
        {
            Path newPath = new Path(paths.Count, position, rotation);
            paths.Add(newPath);
        }

        public void RemovePathPoint(Path path)
        {
            if (paths.Contains(path))
            {
                paths.Remove(path);
            }
        }
    }
}