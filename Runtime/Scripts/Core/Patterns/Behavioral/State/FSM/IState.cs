using UnityEngine;

namespace OSK
{
    public interface IState
    {
        public void OnEnter();
        public void Tick();
        public void FixedTick();
        public void OnExit();

        public Color GizmoState() => Color.clear;
    }
}