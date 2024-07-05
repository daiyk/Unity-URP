using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    //get the default shader tag id for unlit shader
    static ShaderTagId _unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
    //get the shader tags for the legacy shaders
    static ShaderTagId[] legacyShaderTagIds = {
		new ShaderTagId("Always"),
		new ShaderTagId("ForwardBase"),
		new ShaderTagId("PrepassBase"),
		new ShaderTagId("Vertex"),
		new ShaderTagId("VertexLMRGBM"),
		new ShaderTagId("VertexLM")
	};

    ScriptableRenderContext _context;
    Camera _camera;
    const string bufferName = "Render Camera";
    CullingResults _cullingResult;
    //buffer object for storing rendering commands
    //render buffer stores the current rendering commands
    CommandBuffer _buffer = new CommandBuffer
    {
        name = bufferName
    };
    //render function that all geometry this camera can see.
    public void Render(ScriptableRenderContext context, Camera camera)
    {
        _context = context;
        _camera = camera;
        if(!Cull())
            return;
        SetUpCameraProperties();
        _buffer.ClearRenderTarget(true, true, Color.clear);
        _buffer.BeginSample(bufferName);
        ExecuteBuffer();
        
        DrawVisibleGeometry();
        DrawUnsupportedShaders();

        _buffer.EndSample(bufferName);
        ExecuteBuffer();

        Submit();
    }

    
    void DrawVisibleGeometry(){
        
        //draw skybox for this camera
        _context.DrawSkybox(_camera);
        var sortSetting = new SortingSettings(_camera){criteria = SortingCriteria.CommonOpaque}; // use camera's setting to define the drawing order
        var drawingSettings = new DrawingSettings(_unlitShaderTagId, sortSetting); //it only draws the unlit shader
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

         _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings); 
        
        sortSetting = new SortingSettings(_camera){criteria = SortingCriteria.CommonTransparent}; // use camera's setting to define the drawing order
        drawingSettings = new DrawingSettings(_unlitShaderTagId, sortSetting); //it only draws the unlit shader
        filteringSettings = new FilteringSettings(RenderQueueRange.transparent); 
        _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);
        
    }

    void DrawUnsupportedShaders(){
        //draw unsupported shaders
        var sortingSettings = new SortingSettings(_camera);
        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], sortingSettings);
        for (int i = 1; i < legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        var filteringSettings = FilteringSettings.defaultValue;
        _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);
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

    bool Cull(){
        if(_camera.TryGetCullingParameters(out ScriptableCullingParameters p)){
            _cullingResult = _context.Cull(ref p);
            return true;
        }
        return false;
    }


    
    
}
