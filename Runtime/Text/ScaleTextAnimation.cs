using UnityEngine;
using TMPro;

namespace OSK
{
    public class ScaleTextAnimation : MonoBehaviour
    {
        public TMP_Text textMesh;
        public float frequency = 5.0f; // Frequency of the scale effect

        // Amplitude of the scale effect, 0.5 means scaling between 0.5x and 1.5x of the original size.
        public float  scaleAmplitude = 0.5f; 

        private TMP_TextInfo textInfo;
        private bool[] animateCharFlags;

        private void Awake()
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TMP_Text>();
            }

            textInfo = textMesh.textInfo;
        }

        private void Update()
        {
            textMesh.ForceMeshUpdate();
            textInfo = textMesh.textInfo;
            int characterCount = textInfo.characterCount;

            // Initialize or update our flags array
            if (animateCharFlags == null || animateCharFlags.Length < characterCount)
            {
                animateCharFlags = new bool[characterCount];
            }

            // Reset flags
            for (int i = 0; i < animateCharFlags.Length; i++) animateCharFlags[i] = false;

            // Detect scaling links
            DetectScalingLinks();

            if (characterCount == 0) return;

            for (int i = 0; i < characterCount; i++)
            {
                if (!animateCharFlags[i]) continue;

                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;
                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
                Vector3 charMidBaseline = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2;
                float scale = 1 + Mathf.Sin(Time.time * frequency + i) * scaleAmplitude;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 offset = vertices[vertexIndex + j] - charMidBaseline;
                    offset *= scale;
                    vertices[vertexIndex + j] = charMidBaseline + offset;
                }
            }

            // Update the mesh with the new vertex positions
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
        }

        void DetectScalingLinks()
        {
            string targetLink = "scale";
            TMP_TextInfo textInfo = textMesh.textInfo;

            for (int i = 0; i < textInfo.linkCount; i++)
            {
                TMP_LinkInfo linkInfo = textInfo.linkInfo[i];
                if (linkInfo.GetLinkID() == targetLink)
                {
                    for (int j = 0; j < linkInfo.linkTextLength; j++)
                    {
                        int characterIndex = linkInfo.linkTextfirstCharacterIndex + j;
                        if (characterIndex < animateCharFlags.Length)
                        {
                            animateCharFlags[characterIndex] = true;
                        }
                    }
                }
            }
        }
    }
}