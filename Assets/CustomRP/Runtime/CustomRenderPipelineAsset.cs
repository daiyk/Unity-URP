using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline Asset")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
    // return pipeline instance
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline();
    }
}
