// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GifLoader
{
    /// <summary>
    /// Loads a sequence of PNGs to play on a quad.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class PngAnimation : MonoBehaviour
    {
        [SerializeField]
        private string resourceFolder = null;

        private float frameCounter;
        private int currentFrameIndex;

        private Material material;
        private Texture2D[] pngAnimations;

        private void Awake()
        {
            var thisRenderer = GetComponent<Renderer>();

            if (thisRenderer == null)
            {
                throw new MissingComponentException($"Missing {nameof(Renderer)} Component on {name}");
            }

            material = thisRenderer.material;

            var resourceFolderPath = Application.dataPath + resourceFolder;

            if (!Directory.Exists(resourceFolderPath))
            {
                throw new ArgumentException("Invalid resource folder specified");
            }

            var pngFiles = Directory.GetFiles(resourceFolderPath, "*.png").OrderBy(f => f.Length).ToArray();

            pngAnimations = new Texture2D[pngFiles.Length];

            Debug.Assert(pngFiles.Length == pngAnimations.Length);

            for (var i = 0; i < pngFiles.Length; i++)
            {
                var pngFile = pngFiles[i].Replace($"{Application.dataPath}/Resources/", string.Empty);
                pngFile = pngFile.Replace("\\", "/");
                pngFile = pngFile.Replace(".png", string.Empty);
                var texture = Resources.Load<Texture2D>(pngFile);
                Debug.Assert(texture != null);
                pngAnimations[i] = texture;
            }
        }

        private void Update()
        {
            frameCounter += Time.deltaTime;

            if (frameCounter >= 1f / 24f)
            {
                currentFrameIndex = Mathf.Min(currentFrameIndex + 1, pngAnimations.Length);
                frameCounter = 0f;
            }

            if (currentFrameIndex >= pngAnimations.Length)
            {
                currentFrameIndex = 0;
            }

            material.mainTexture = pngAnimations[currentFrameIndex];
        }

        private void OnDestroy()
        {
            if (material != null)
            {
                if (Application.isEditor && Application.isPlaying)
                {
                    Destroy(material);
                }
                else
                {
                    DestroyImmediate(material);
                }
            }
        }
    }
}
