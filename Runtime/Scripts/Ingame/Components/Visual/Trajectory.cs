using UnityEngine;

namespace OSK
{
public class Trajectory : MonoBehaviour
{
	[SerializeField]
	private int dotsNumber;

	[SerializeField]
	private GameObject dotsParent;

	[SerializeField]
	private GameObject dotPrefab;

	[SerializeField]
	private float dotSpacing;

	[SerializeField]
	[Range(0.01f, 0.3f)]
	private float dotMinScale;

	[SerializeField]
	[Range(0.3f, 1f)]
	private float dotMaxScale;

	private Transform[] dotsList;

	private Vector2 pos;

	private float timeStamp;

	private void Start()
	{
		Hide();
		PrepareDots();
	}
	
	/*
	 private void OnDrag()
	 {
		  endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
		  distance = Vector2.Distance(startPoint, endPoint);
		  direction = (startPoint - endPoint).normalized;
		  force = direction * distance * pushForce;
		  Debug.DrawLine(startPoint, endPoint);
		  trajectory.UpdateDots(ball.pos, force);
	 }
	 */

	private void PrepareDots()
	{
		dotsList = new Transform[dotsNumber];
		dotPrefab.transform.localScale = Vector3.one * dotMaxScale;
		float num = dotMaxScale;
		float num2 = num / (float)dotsNumber;
		for (int i = 0; i < dotsNumber; i++)
		{
			dotsList[i] = Object.Instantiate(dotPrefab, null).transform;
			dotsList[i].parent = dotsParent.transform;
			dotsList[i].localScale = Vector3.one * num;
			if (num > dotMinScale)
			{
				num -= num2;
			}
		}
	}

	public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
	{
		timeStamp = dotSpacing;
		for (int i = 0; i < dotsNumber; i++)
		{
			pos.x = ballPos.x + forceApplied.x * timeStamp;
			pos.y = ballPos.y + forceApplied.y * timeStamp - Physics2D.gravity.magnitude * timeStamp * timeStamp / 2f;
			dotsList[i].position = pos;
			timeStamp += dotSpacing;
		}
	}

	public void Show()
	{
		dotsParent.SetActive(value: true);
	}

	public void Hide()
	{
		dotsParent.SetActive(value: false);
	}
}

}