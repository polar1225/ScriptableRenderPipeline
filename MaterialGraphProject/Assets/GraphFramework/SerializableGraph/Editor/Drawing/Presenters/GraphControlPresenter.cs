using System;
using UIElements.GraphView;
using UnityEditor.MaterialGraph.Drawing;
using UnityEngine;
using UnityEngine.Graphing;

namespace UnityEditor.Graphing.Drawing
{
    [Serializable]
    public abstract class GraphControlPresenter : MaterialNodePresenter
    {
        public INode node { get; private set; }

        protected GraphControlPresenter()
        {}

        public void Initialize(INode inNode)
        {
            node = inNode;
        }

        public virtual void OnGUIHandler()
        {
            if (node == null)
                return;

            GUIUtility.GetControlID(node.guid.GetHashCode(), FocusType.Passive);
        }

        public abstract float GetHeight();
    }
}
