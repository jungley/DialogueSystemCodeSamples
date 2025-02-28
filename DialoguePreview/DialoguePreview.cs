using Assets.RydenCam.Scripts.BranchCamCC;
using RydenCam.Common;
using UnityEngine;
using Assets.RydenCam.Scripts.BranchCamEditor.PreviewRender;
using System.Collections.Generic;
using RydenCam.BranchCamEditor.BranchCam;

namespace RydenCam.BranchCamEditor.PreviewRender
{
    /// <summary>
    /// DialoguePreview class is responsible for rendering the preview window
    /// A Property to a NodeDrawer that is associated with a node that is ITalkable
    /// </summary>
    /// <typeparam name="N"></typeparam>
    public class DialoguePreview<N> where N : Node, ITalkable
    {

        private PreviewCameraWrapper cameraWrapper;
        private PreviewRenderer previewRenderer;

        private N node;

        public List<PreviewActorData> ActorDatas { get; set; }

        public DialoguePreview(N node)
        {
            this.node = node;
            Initailize();
        }


        public void Initailize()
        {
            previewRenderer = new PreviewRenderer();

            ActorDatas = SetupPreviewSceneData.Initialize();
            cameraWrapper = new PreviewCameraWrapper(ActorDatas);


        }

        public void UpdateShotRender()
        {
            Initailize();
        }

        /// <summary>
        /// Draws the preview window
        /// </summary>
        public void DrawPreviewWindow()
        {
            
            var windowRect = new Rect(node.EditorPosition.x + node.NodeWidth, node.EditorPosition.y, node.NodeWidth, 120);
            
            if (previewRenderer.CachedRenderTexture != null)
            {
                GUI.DrawTexture(windowRect, previewRenderer.CachedRenderTexture);
                return;
            } 
            
            
            ComposePreviewImage(windowRect);
            
        }

        public void ComposePreviewImage(Rect windowRect)
        {
            CamShotConfig shot = node.NodeConvodata.ShotConfig;

            //TODO Preview should handle local custom set shots
            if (shot.IsCustomSet) return;

            Pose camPose = cameraWrapper.CalculateCameraShot(shot);

            previewRenderer.RenderPreview(windowRect, camPose, ActorDatas);

        }
    }
}