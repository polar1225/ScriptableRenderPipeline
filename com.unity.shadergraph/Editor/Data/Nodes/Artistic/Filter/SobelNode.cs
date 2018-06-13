using System.Reflection;
using UnityEngine;

namespace UnityEditor.ShaderGraph
{
    [Title("Artistic", "Filter", "Sobel")]
    public class SobelNode : CodeFunctionNode, IMayRequireMeshUV
    {
        public SobelNode()
        {
            name = "Sobel";
        }

        public override string documentationURL
        {
            get { return "https://github.com/Unity-Technologies/ShaderGraph/wiki/Rectangle-Node"; }
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("Sobel", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static string Sobel(
            [Slot(0, Binding.None)] Texture2D TextureIn,
            [Slot(1, Binding.MeshUV0)] Vector2 UV,
            [Slot(2, Binding.None, 0.005f, 0, 0, 0)] Vector1 Offset,
            [Slot(3, Binding.None, ShaderStageCapability.Fragment)] out Vector4 Out)
            
        {
            Out = Vector4.zero;
            return
                @"
{
    sampler2D _Texture = Texture;
    float2 delta = float2(Offset, Offset);
			
    float4 hr = float4(0, 0, 0, 0);
    float4 vt = float4(0, 0, 0, 0);
    
    hr += tex2D(_Texture, (UV + float2(-1.0, -1.0) * delta)) *  1.0;
    hr += tex2D(_Texture, (UV + float2( 0.0, -1.0) * delta)) *  0.0;
    hr += tex2D(_Texture, (UV + float2( 1.0, -1.0) * delta)) * -1.0;
    hr += tex2D(_Texture, (UV + float2(-1.0,  0.0) * delta)) *  2.0;
    hr += tex2D(_Texture, (UV + float2( 0.0,  0.0) * delta)) *  0.0;
    hr += tex2D(_Texture, (UV + float2( 1.0,  0.0) * delta)) * -2.0;
    hr += tex2D(_Texture, (UV + float2(-1.0,  1.0) * delta)) *  1.0;
    hr += tex2D(_Texture, (UV + float2( 0.0,  1.0) * delta)) *  0.0;
    hr += tex2D(_Texture, (UV + float2( 1.0,  1.0) * delta)) * -1.0;
    
    vt += tex2D(_Texture, (UV + float2(-1.0, -1.0) * delta)) *  1.0;
    vt += tex2D(_Texture, (UV + float2( 0.0, -1.0) * delta)) *  2.0;
    vt += tex2D(_Texture, (UV + float2( 1.0, -1.0) * delta)) *  1.0;
    vt += tex2D(_Texture, (UV + float2(-1.0,  0.0) * delta)) *  0.0;
    vt += tex2D(_Texture, (UV + float2( 0.0,  0.0) * delta)) *  0.0;
    vt += tex2D(_Texture, (UV + float2( 1.0,  0.0) * delta)) *  0.0;
    vt += tex2D(_Texture, (UV + float2(-1.0,  1.0) * delta)) * -1.0;
    vt += tex2D(_Texture, (UV + float2( 0.0,  1.0) * delta)) * -2.0;
    vt += tex2D(_Texture, (UV + float2( 1.0,  1.0) * delta)) * -1.0;
    
    float result = sqrt(hr * hr + vt * vt);
    Out = float4(result, result, result, 1.0);
}";
        }

        public bool RequiresMeshUV(UVChannel channel, ShaderStageCapability stageCapability)
        {
            return true;
        }
    }
}
