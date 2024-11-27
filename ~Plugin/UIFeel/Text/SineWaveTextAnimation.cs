using UnityEngine;
using TMPro;

namespace OSK
{
    // Use <b><link=wave>WASD</link></b> or arrow keys to move!
    public class SineWaveTextAnimation : MonoBehaviour
    {
        public TMP_Text textMesh;
        public float frequency = 5.0f;
        public float amplitude = 5.0f;
        private TMP_TextInfo textInfo;
        private bool[] animateCharFlags;

        void Awake()
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TMP_Text>();
            }

            textInfo = textMesh.textInfo;
        }
        
        void Update()
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

            // Custom logic to detect <wave> tags and set flags
            DetectWaveTags();

            if (characterCount == 0) return;

            for (int i = 0; i < characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                // Check if this character is marked for animation
                if (!animateCharFlags[i]) continue;

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;
                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                float wave = Mathf.Sin(i + Time.time * frequency) * amplitude;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 offset = vertices[vertexIndex + j];
                    offset.y += wave;
                    vertices[vertexIndex + j] = offset;
                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
        }

        private void DetectWaveTags()
        {
            // Initialize or reset the animateCharFlags array
            if (animateCharFlags == null || animateCharFlags.Length < textMesh.textInfo.characterCount)
            {
                animateCharFlags = new bool[textMesh.textInfo.characterCount];
            }

            for (int i = 0; i < animateCharFlags.Length; i++)
            {
                animateCharFlags[i] = false; // Reset the flag for each character
            }

            TMP_TextInfo textInfo = textMesh.textInfo;
            for (int i = 0; i < textInfo.linkCount; i++)
            {
                TMP_LinkInfo linkInfo = textInfo.linkInfo[i];
                if (linkInfo.GetLinkID() == "wave") // Check if the link ID is "wave"
                {
                    for (int j = 0; j < linkInfo.linkTextLength; j++)
                    {
                        int characterIndex = linkInfo.linkTextfirstCharacterIndex + j;
                        if (characterIndex < animateCharFlags.Length)
                        {
                            animateCharFlags[characterIndex] =
                                true; // Mark characters within "wave" links for animation
                        }
                    }
                }
            }
        }
    }
}