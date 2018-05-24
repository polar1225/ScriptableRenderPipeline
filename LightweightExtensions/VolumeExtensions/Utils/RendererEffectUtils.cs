﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
	public enum EffectScope { Local, Global }

	[Serializable]
	public class EffectData
	{
		public string variableName;
		public object data;
		public Type dataType;

		public EffectData(string name, object obj, Type type)
		{
			variableName = name;
			data = obj;
			dataType = type;
		}
	}

	public static class RendererEffectUtils 
	{
		public static string GetKeyword(EffectScope scope, IRendererEffect effect)
		{
			// Get a full keyword name from an IMaterialStyle
			string prefix = scope == EffectScope.Global ? "_GLOBAL_" : "_VOLUME_";
			return prefix + effect.GetKeywordName();
		}

		public static string GetVariable(EffectScope scope, EffectData effectData)
		{
			// Get a full variable name from a MaterialStyleData
			string prefix = scope == EffectScope.Global ? "_Global" : "_Volume";
			return prefix + effectData.variableName;
		}

		public static void SetKeyword(EffectScope scope, object obj, IRendererEffect effect, bool state)
        {
			switch(scope)
			{
				case EffectScope.Global:
					CommandBuffer cmd;
					if(!CastToCommandBuffer(obj, out cmd))
						return;

					string globalKeyword = GetKeyword(EffectScope.Global, effect);
					SetKeyword(cmd, globalKeyword, state);
					break;
				case EffectScope.Local:
					Material mat;
					if(!CastToMaterial(obj, out mat))
						return;
					
					string localKeyword = GetKeyword(EffectScope.Local, effect);
					SetKeyword(mat, localKeyword, state);
					break;
			}           
        }

		public static void SetVariable(EffectScope scope, object obj, EffectData effectData)
		{
			var variableName = GetVariable(scope, effectData);
			switch(scope)
			{
				case EffectScope.Global:
					CommandBuffer cmd;
					if(!CastToCommandBuffer(obj, out cmd))
						return;

					if(effectData.dataType == typeof(Color))
						cmd.SetGlobalColor(variableName, (Color)effectData.data);
					else if(effectData.dataType == typeof(Texture2D))
						cmd.SetGlobalTexture(variableName, (Texture2D)effectData.data);
					else if(effectData.dataType == typeof(int))
						cmd.SetGlobalInt(variableName, (int)effectData.data);
					else if(effectData.dataType == typeof(float))
						cmd.SetGlobalFloat(variableName, (float)effectData.data);
					else if(effectData.dataType == typeof(Vector2))
						cmd.SetGlobalVector(variableName, (Vector2)effectData.data);
					else if(effectData.dataType == typeof(Vector3))
						cmd.SetGlobalVector(variableName, (Vector3)effectData.data);
					else if(effectData.dataType == typeof(Vector4))
						cmd.SetGlobalVector(variableName, (Vector4)effectData.data);
					break;
				case EffectScope.Local:
					Material mat;
					MaterialPropertyBlock block;
					if(CastToMaterial(obj, out mat))
						SetVariableOnMaterial(mat, effectData, variableName);
					else if(CastToPropertyBlock(obj, out block))
						SetVariableOnPropertyBlock(block, effectData, variableName);
					else
						Debug.LogError("Local material effect object must be of types \"Material\" or \"MaterialPropertyBlock\"");
					break;
			}
		}

		private static void SetVariableOnMaterial(Material mat, EffectData effectData, string variableName)
		{
			if(effectData.dataType == typeof(Color))
				mat.SetColor(variableName, (Color)effectData.data);
			else if(effectData.dataType == typeof(Texture2D))
				mat.SetTexture(variableName, (Texture2D)effectData.data);
			else if(effectData.dataType == typeof(int))
				mat.SetInt(variableName, (int)effectData.data);
			else if(effectData.dataType == typeof(float))
				mat.SetFloat(variableName, (float)effectData.data);
			else if(effectData.dataType == typeof(Vector2))
				mat.SetVector(variableName, (Vector2)effectData.data);
			else if(effectData.dataType == typeof(Vector3))
				mat.SetVector(variableName, (Vector3)effectData.data);
			else if(effectData.dataType == typeof(Vector4))
				mat.SetVector(variableName, (Vector4)effectData.data);
		}

		private static void SetVariableOnPropertyBlock(MaterialPropertyBlock block, EffectData effectData, string variableName)
		{
			if(effectData.dataType == typeof(Color))
				block.SetColor(variableName, (Color)effectData.data);
			else if(effectData.dataType == typeof(Texture2D))
				block.SetTexture(variableName, (Texture2D)effectData.data);
			else if(effectData.dataType == typeof(int))
				block.SetInt(variableName, (int)effectData.data);
			else if(effectData.dataType == typeof(float))
				block.SetFloat(variableName, (float)effectData.data);
			else if(effectData.dataType == typeof(Vector2))
				block.SetVector(variableName, (Vector2)effectData.data);
			else if(effectData.dataType == typeof(Vector3))
				block.SetVector(variableName, (Vector3)effectData.data);
			else if(effectData.dataType == typeof(Vector4))
				block.SetVector(variableName, (Vector4)effectData.data);
		}

		/*public static void SetVariable(EffectScope scope, object obj, EffectData effectData)
		{
			var variableName = GetVariable(scope, effectData);
			switch(scope)
			{
				case EffectScope.Global:
					CommandBuffer cmd;
					if(!CastToCommandBuffer(obj, out cmd))
						return;

					if(effectData.dataType == typeof(Color))
						cmd.SetGlobalColor(variableName, (Color)effectData.data);
					else if(effectData.dataType == typeof(Texture2D))
						cmd.SetGlobalTexture(variableName, (Texture2D)effectData.data);
					else if(effectData.dataType == typeof(int))
						cmd.SetGlobalInt(variableName, (int)effectData.data);
					else if(effectData.dataType == typeof(float))
						cmd.SetGlobalFloat(variableName, (float)effectData.data);
					else if(effectData.dataType == typeof(Vector2))
						cmd.SetGlobalVector(variableName, (Vector2)effectData.data);
					else if(effectData.dataType == typeof(Vector3))
						cmd.SetGlobalVector(variableName, (Vector3)effectData.data);
					else if(effectData.dataType == typeof(Vector4))
						cmd.SetGlobalVector(variableName, (Vector4)effectData.data);
					break;
				case EffectScope.Local:
					Material mat;
					if(!CastToMaterial(obj, out mat))
						return;
					
					if(effectData.dataType == typeof(Color))
						mat.SetColor(variableName, (Color)effectData.data);
					else if(effectData.dataType == typeof(Texture2D))
						mat.SetTexture(variableName, (Texture2D)effectData.data);
					else if(effectData.dataType == typeof(int))
						mat.SetInt(variableName, (int)effectData.data);
					else if(effectData.dataType == typeof(float))
						mat.SetFloat(variableName, (float)effectData.data);
					else if(effectData.dataType == typeof(Vector2))
						mat.SetVector(variableName, (Vector2)effectData.data);
					else if(effectData.dataType == typeof(Vector3))
						mat.SetVector(variableName, (Vector3)effectData.data);
					else if(effectData.dataType == typeof(Vector4))
						mat.SetVector(variableName, (Vector4)effectData.data);
					break;
			}
		} */

		public static void SetKeyword(CommandBuffer cmd, string keyword, bool state)
        {
			// Set keyword on a Command Buffer
            if(state == true)
                cmd.EnableShaderKeyword(keyword);
            else
                cmd.DisableShaderKeyword(keyword);
        }

		private static void SetKeyword(Material mat, string keyword, bool state)
		{
			// Set keyword on a Material
			if(state == true)
				mat.EnableKeyword(keyword);
			else
				mat.DisableKeyword(keyword);
		}

		private static bool CastToCommandBuffer(object obj, out CommandBuffer cmd)
		{
			// Test cast object to Command Buffer returning error in failure case
			cmd = obj as CommandBuffer;
			if(cmd == null)
				Debug.LogError("Global material style object must be of type \"Command Buffer\"");
			return cmd != null;
		}

		private static bool CastToMaterial(object obj, out Material mat)
		{
			// Test cast object to Material returning error in failure case
			mat = obj as Material;
			//if(mat == null)
			//	Debug.LogError("Local material style object must be of type \"Material\"");
			return mat != null;
		}

		private static bool CastToPropertyBlock(object obj, out MaterialPropertyBlock block)
		{
			// Test cast object to Material returning error in failure case
			block = obj as MaterialPropertyBlock;
			//if(block == null)
			//	Debug.LogError("Local material style object must be of type \"MaterialPropertyBlock\"");
			return block != null;
		}
	}
}

