using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ResolutionScaler : MonoBehaviour
{
	[Range(3f, 70f)]
	public float Scale = 2f;

	private Camera cameraComponent;

	private RenderTexture texture;

	private void Start()
	{
		CreateTexture();
	}

	private void CreateTexture()
	{
		int width = Mathf.RoundToInt((float)Screen.width / Scale);
		int height = Mathf.RoundToInt((float)Screen.height / Scale);
		texture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		texture.antiAliasing = 1;
		cameraComponent = GetComponent<Camera>();
	}

	private void OnPreRender()
	{
		cameraComponent.targetTexture = texture;
	}

	private void OnPostRender()
	{
		cameraComponent.targetTexture = null;
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		src.filterMode = FilterMode.Point;
		Graphics.Blit(src, dest);
	}
}
