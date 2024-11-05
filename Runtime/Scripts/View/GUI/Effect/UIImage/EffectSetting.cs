using System.Collections.Generic;
using CustomInspector;
using DG.Tweening;
using UnityEngine;

namespace OSK
{
    public enum TypeMove
    {
        Straight,
        Beziers,
        CatmullRom,
        Path,
        DoJump
    }

    public enum TypeAnimation
    {
        Ease,
        Curve,
    }


    [System.Serializable]
    public class EffectSetting
    { 
        [Header("Setup")]
        public string name;
        public GameObject icon;
        public int numberOfEffects = 10;
        public bool isDrop = true;

        [Space(10)]

        #region Drop

        [Header("Drop")]
        [ShowIf(nameof(isDrop), true)]
        public float sphereRadius = 1;

        [MinMaxRange(0, 10)]
        [ShowIf(nameof(isDrop), true)]
        public MinMax delayDrop;

        [MinMaxRange(0, 10)]
        [ShowIf(nameof(isDrop), true)]
        public MinMax timeDrop;

        [ShowIf(nameof(isDrop), true)] public TypeAnimation typeAnimationDrop = TypeAnimation.Ease;
 
        private bool isShowEase => typeAnimationDrop == TypeAnimation.Ease && isDrop;
        [CustomInspector.ShowIf(nameof(isShowEase), true)]
        public Ease easeDrop = Ease.Linear;

        private bool isShowCurve => typeAnimationDrop == TypeAnimation.Curve && isDrop;
        [CustomInspector.ShowIf(nameof(isShowCurve), true)]
        public AnimationCurve curveDrop;

        #endregion

        [Space(10)]

        #region Move

        [Header("Move")]
        public TypeMove typeMove = TypeMove.Straight;
        
        [CustomInspector.ShowIf(nameof(typeMove), TypeMove.DoJump)]
        public float jumpPower = 1;
        private bool isShowPath => typeMove != TypeMove.DoJump && typeMove != TypeMove.Straight;
        [CustomInspector.ShowIf(nameof(isShowPath), true)]
        public List<Vector3> paths;

        public TypeAnimation typeAnimationMove = TypeAnimation.Ease;

        [CustomInspector.ShowIf(nameof(typeAnimationMove), TypeAnimation.Ease)]
        public Ease easeMove = Ease.Linear;

        [CustomInspector.ShowIf(nameof(typeAnimationMove), TypeAnimation.Curve)]
        public AnimationCurve curveMove;

        [MinMaxRange(0, 10)] public MinMax timeMove;
        [MinMaxRange(0, 10)] public MinMax delayMove;

        public float scaleTarget = 1;
        public System.Action OnCompleted;

        #endregion

        public Vector3 pointSpawn { get; set; }
        public Vector3 pointTarget { get; set; }
    }
}