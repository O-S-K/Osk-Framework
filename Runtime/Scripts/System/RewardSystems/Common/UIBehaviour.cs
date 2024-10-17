using UnityEngine;

namespace OSK
{
	public class UIBehaviourOptions
	{
		public int index;
		public string itemId;
	}

	public class UIBehaviour : MonoBehaviour
	{
		[HideInInspector]
		public RectTransform rectTransform;
		protected UIBehaviourOptions _options;

		public virtual void SetData(UIBehaviourOptions options)
		{
			this._options = options;
		}

		private void Awake()
		{
			this.rectTransform = this.GetComponent<RectTransform>();
		} 
		
		public virtual void Refresh()
		{
		}
	}
}