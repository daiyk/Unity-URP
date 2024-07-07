using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

//store all functions only meaningful in editor mode
public partial class CameraRenderer
{
	//get the shader tags for the legacy shaders
	static ShaderTagId[] legacyShaderTagIds = {
		new ShaderTagId("Always"),
		new ShaderTagId("ForwardBase"),
		new ShaderTagId("PrepassBase"),
		new ShaderTagId("Vertex"),
		new ShaderTagId("VertexLMRGBM"),
		new ShaderTagId("VertexLM")
	};
	static Material _editorErrorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
	partial void DrawUnsupportedShaders();
	partial void DrawGizmos();

	partial void EmitUIGeometryForRender();

#if UNITY_EDITOR
	partial void DrawUnsupportedShaders()
	{
		//draw unsupported shaders

		var sortingSettings = new SortingSettings(_camera);
		var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], sortingSettings)
		{
			overrideMaterial = _editorErrorMaterial
		};

		for (int i = 1; i < legacyShaderTagIds.Length; i++)
		{
			drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
		}
		var filteringSettings = FilteringSettings.defaultValue;
		_context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);
	}

	partial void DrawGizmos()
	{
		if (Handles.ShouldRenderGizmos())
		{
			_context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
			_context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);
		}
	}

	partial void EmitUIGeometryForRender()
	{
		//if (_camera.cameraType == CameraType.SceneView)
		//{
			ScriptableRenderContext.EmitWorldGeometryForSceneView(_camera);
		//}
	}
#endif
}
