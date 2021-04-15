using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameLibrary
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class WallHackCameraScript : MonoBehaviour
    {
        private Camera WallhackCamera;


        private void Start()
        {
            WallhackCamera = GetComponent<Camera>();
            WallhackCamera.gameObject.SetActive(GameManager.IsOwningSkill(SkillType.WallHack));
        }

        void OnEnable()
        {
            RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
            RenderPipelineManager.beginCameraRendering += RenderPipelineManager_beginCameraRendering;
        }

        void OnDisable()
        {
            RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
            RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_beginCameraRendering;
        }

        private void RenderPipelineManager_beginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (camera == WallhackCamera)
                GL.wireframe = true;
        }

        private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (camera == WallhackCamera)
                GL.wireframe = false;
        }
    }
}