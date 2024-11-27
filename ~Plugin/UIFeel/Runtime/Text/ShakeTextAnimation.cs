using UnityEngine;
using TMPro;

namespace OSK
{
    public class ShakeTextAnimation : MonoBehaviour
    {
        public TMP_Text textMesh; // Reference to the TextMeshPro component
        public float maxShakeAmount = 0.5f; // The maximum distance characters can move during the shake
        public float decayRate = 1.0f; // Rate at which shake amount decays

        private TMP_TextInfo textInfo;
        private bool[] shakeCharFlags; // Flags to indicate which characters should shake
        private float currentShakeAmount; // Current shake amount, which will decay over time

        private void Awake()
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TMP_Text>();
            }

            textInfo = textMesh.textInfo;
            currentShakeAmount = maxShakeAmount;
        }

        private void TextWriterDelta(string chr)
        {
            if (chr == " ") return;
            currentShakeAmount = maxShakeAmount;
        }

        private void Update()
        {
            DetectShakeLinks(); // Detect which characters should be shaken
            ApplyShakeEffect(); // Apply shake effect to those characters
            // Update the current shake amount for the next frame
            currentShakeAmount = Mathf.Max(0.0f, currentShakeAmount - decayRate * Time.deltaTime);
        }

        private void DetectShakeLinks()
        {
            textMesh.ForceMeshUpdate();
            string targetLink = "shake";
            TMP_TextInfo textInfo = textMesh.textInfo;
            shakeCharFlags = new bool[textInfo.characterCount]; // Reset and prepare flag array for current frame

            for (int i = 0; i < textInfo.linkCount; i++)
            {
                TMP_LinkInfo linkInfo = textInfo.linkInfo[i];
                if (linkInfo.GetLinkID() == targetLink)
                {
                    for (int j = 0; j < linkInfo.linkTextLength; j++)
                    {
                        int characterIndex = linkInfo.linkTextfirstCharacterIndex + j;
                        if (characterIndex < shakeCharFlags.Length)
                        {
                            shakeCharFlags[characterIndex] = true;
                        }
                    }
                }
            }
        }

        private void ApplyShakeEffect()
        {
            textInfo = textMesh.textInfo;
            int characterCount = textInfo.characterCount;

            if (characterCount == 0 || currentShakeAmount <= 0.0f) return;

            for (int i = 0; i < characterCount; i++)
            {
                if (!shakeCharFlags[i]) continue; // Only apply effect to marked characters

                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue; // Skip invisible characters

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;
                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                // Apply shake effect
                Vector3 offset = Random.insideUnitCircle * currentShakeAmount;

                vertices[vertexIndex + 0] += offset;
                vertices[vertexIndex + 1] += offset;
                vertices[vertexIndex + 2] += offset;
                vertices[vertexIndex + 3] += offset;
            }

            // Update the mesh with the new vertex positions
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
        }
    }
}