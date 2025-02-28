﻿using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.RydenCam.Scripts.BranchCamEditor.PreviewRender
{
    public class PreviewRenderer
    {
        public Texture CachedRenderTexture { get; set; }

        private PreviewRenderUtility _prevRenderUtility { get; set; }
        private PreviewRenderUtility previewRenderUtility
        {
            get
            {
                if (_prevRenderUtility == null || _prevRenderUtility.camera == null)
                {
                    _prevRenderUtility = new PreviewRenderUtility();
                    _prevRenderUtility.camera.fieldOfView = 40;
                    _prevRenderUtility.camera.nearClipPlane = 0.01f;
                    _prevRenderUtility.camera.farClipPlane = 20;
                }

                return _prevRenderUtility;
            }
        }

        /// <summary>
        /// Renders the preview window
        /// </summary>
        /// <param name="windowRect"></param>
        /// <param name="camPose"></param>
        /// <param name="actorsToRender"></param>
        public void RenderPreview(Rect windowRect, Pose camPose, List<PreviewActorData> actorsToRender)
        {
            previewRenderUtility.BeginPreview(windowRect, GUIStyle.none);

            previewRenderUtility.camera.transform.SetPositionAndRotation(camPose.position, camPose.rotation);


            foreach (var actor in actorsToRender)
            {

                foreach (var meshMat in actor.MeshMat)
                {
                    if (meshMat.Mesh == null) continue;

                    Matrix4x4 customMatrix = Matrix4x4.TRS(actor.ActorPositionData.MeshOriginPoint, actor.ActorPositionData.ActorRotation, Vector3.one);
                    previewRenderUtility.DrawMesh(meshMat.Mesh, customMatrix, meshMat.Mat, 0);
                }
            }

            previewRenderUtility.Render();

            CachedRenderTexture = previewRenderUtility.EndPreview();

            GUI.DrawTexture(windowRect, CachedRenderTexture);

            previewRenderUtility.Cleanup();
        }
    }
}