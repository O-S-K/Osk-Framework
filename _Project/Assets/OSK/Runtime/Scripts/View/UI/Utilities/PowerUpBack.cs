using UnityEngine;

public class PowerUpBack : MonoBehaviour
{
	public float RotationSpeed = 4f;

	public float DecaySpeed = 2f;

	public AnimationCurve ScaleCurve;

	private float DefaultScale;

	private float rotSpeed;

	private float BeatEffectRatio;

	private void Start()
	{
		DefaultScale = base.transform.localScale.x;
	}

	private void Update()
	{
		BeatEffectRatio = Mathf.Lerp(BeatEffectRatio, 0f, Time.deltaTime * DecaySpeed);
		base.transform.Rotate(0f, 0f, RotationSpeed * BeatEffectRatio * Time.deltaTime);
		float num = ScaleCurve.Evaluate(BeatEffectRatio);
		base.transform.localScale = Vector3.one * (DefaultScale * (1f + num));
	}

	private void RotateBeat()
	{
		float num = 1f;
		// if (PlayerCar.instance.CurrentSpeedRatio < 1f)
		// {
		// 	num = PlayerCar.instance.CurrentSpeedRatio;
		// }
		BeatEffectRatio = 1f * num;
	}
}
