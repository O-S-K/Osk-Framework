using UnityEngine;

public class DrawBoundaries : MonoBehaviour
{
	public enum BoundaryType
	{
		Box = 0,
		Circle = 1
	}

	[SerializeField]
	private Color color = Color.red;

	[SerializeField]
	private BoundaryType boundaryType;

	private void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		if (boundaryType == BoundaryType.Box)
		{
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}
		else if (boundaryType == BoundaryType.Circle)
		{
			Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
		}
	}
}
