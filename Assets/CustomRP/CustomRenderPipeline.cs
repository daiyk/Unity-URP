using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    // implement the Render function
    // entry point for custom URPs
    ScriptableRenderContext _context;
    CameraRenderer _renderer = new CameraRenderer();
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        
    }
    //override functions which will call the first render function
    protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
    {
        foreach(Camera camera in cameras)
        {
            _renderer.Render(context, camera);
        }
    }
}
