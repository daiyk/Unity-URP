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

    //Was entry point for custom SRPs
    //not used, for abstract method implementation only
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        
    }
    //entry point for custom SRPs
    //override functions which will call the first render function and create a render pipeline
    protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
    {

        foreach(Camera camera in cameras)
        {
            //use separate class to render the camera
            _renderer.Render(context, camera);
        }
    }

}
