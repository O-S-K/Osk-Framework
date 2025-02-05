using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        protected void Start() => _root = SetupTree();
        private void Update() => _root?.Evaluate();
        protected abstract Node SetupTree();
    }
}