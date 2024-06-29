using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    ScriptableRenderContext _context;
    Camera _camera;
    const string bufferName = "Render Camera";
    //buffer object for storing rendering commands
    CommandBuffer _buffer = new CommandBuffer
    {
        name = bufferName
    };
    //render function that render the scene from a camera's view
    public void Render(ScriptableRenderContext context, Camera camera)
    {
        _context = context;
        _camera = camera;
        
        _buffer.ClearRenderTarget(true, true, Color.clear);
        _buffer.BeginSample(bufferName);
        ExecuteBuffer();
        SetUpCameraProperties();
        DrawVisibleGeometry();
        
        _buffer.EndSample(bufferName);
        ExecuteBuffer();

        Submit();
    }

    
    void DrawVisibleGeometry(){
        //draw skybox for this camera
        _context.DrawSkybox(_camera);
    }

    //submit all the rendering commands to the GPU
    void Submit(){
        _context.Submit();
    }

    void ExecuteBuffer(){
        _context.ExecuteCommandBuffer(_buffer);
        _buffer.Clear();
    }

    void SetUpCameraProperties(){
        //SetUp the camera properties
        _context.SetupCameraProperties(_camera);
    }


    
    
}
