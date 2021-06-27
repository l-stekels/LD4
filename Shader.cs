using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace LD4
{
    public class Shader
    {
        public readonly int Handle;

        private readonly Dictionary<string, int> _uniformLocations = new();

        public Shader(string vertexPath, string fragmentPath)
        {
            var shaderSource = File.ReadAllText(vertexPath);
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, shaderSource);
            CompileShader(vertexShader);

            shaderSource = File.ReadAllText(fragmentPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                var location = GL.GetUniformLocation(Handle, key);
                _uniformLocations.Add(key, location);
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void SetInt(string name, int data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                return;
            }
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        public void SetFloat(string name, float data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                return;
            }
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                return;
            }
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }

        public void SetVector3(string name, Vector3 data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                return;
            }
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }

        public void SetVector4(string name, Vector4 data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                return;
            }
            GL.UseProgram(Handle);
            GL.Uniform4(_uniformLocations[name], data);
        }

        public void Delete()
        {
            GL.DeleteProgram(Handle);
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code == (int) All.True)
            {
                return;
            }

            var infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code == (int) All.True)
            {
                return;
            }

            var infoLog = GL.GetProgramInfoLog(program);
            throw new Exception($"Error occurred whilst linking Program({program}).\n\n{infoLog}");
        }
    }
}