using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace OSK
{
    [RequireComponent(typeof(Graphic))]
    public class GraphicColorChangeProvider : DoTweenBaseProvider
    {
        [Header("Color change, color mixing, gradual fading and gradual appearance, please use this component to achieve")]
        public Color endValue = default;
        [HideInInspector]
        public Graphic graphic;

        private void Awake() => graphic = GetComponent<Graphic>();

        #region Editor initialization and null reference prevention measures
        public override void OnValidate() => graphic = graphic ?? GetComponent<Graphic>();
        private void Reset()
        {
            graphic = GetComponent<Graphic>();
            endValue = graphic.color; // Capture the initial value
        }
        #endregion

        public override Tweener InitTween()
        {
            var arr = GetComponents<GraphicColorChangeProvider>();
            var blendable = null != arr && arr.Length > 1;
            if (blendable)
            {
                Debug.Log($"Found multiple {nameof(GraphicColorChangeProvider)} entering color mixing mode!");
            }
            return blendable ? graphic.DOBlendableColor(endValue, duration) : graphic.DOColor(endValue, duration);
        }
        public override void Stop()
        {
            base.Stop();
            tweener?.Rewind(); //Reset the changes made by Dotween
            tweener = null;
        }
    }
}