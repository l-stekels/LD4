using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace LD4
{
    public class Mesh
    {
        public List<Vertex> Vertices;
        public List<int> Indices;
        public List<Texture> Textures;
        public VAO Vao;

        public Mesh(List<Vertex> vertices, List<int> indices, List<Texture> textures)
        {
            (Vertices, Indices, Textures) = (vertices, indices, textures);
            Vao = new VAO();
            Vao.Bind();
            VBO vbo = new(Vertices);
            EBO ebo = new(Indices);
            var size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vertex));
            Vao.LinkAttrib(ref vbo, 0, 3, VertexAttribPointerType.Float,size, IntPtr.Zero);
            Vao.LinkAttrib(ref vbo, 1, 3, VertexAttribPointerType.Float,size, new IntPtr(3 * sizeof(float)));
            Vao.LinkAttrib(ref vbo, 2, 3, VertexAttribPointerType.Float,size, new IntPtr(6 * sizeof(float)));
            Vao.LinkAttrib(ref vbo, 3, 2, VertexAttribPointerType.Float,size, new IntPtr(9 * sizeof(float)));
            Vao.Unbind();
            vbo.Unbind();
            ebo.Unbind();
        }

        public void Draw(ref Shader shader, ref Camera camera)
        {
            shader.Use();
            Vao.Bind();

            var numDiffuse = 0;
            var numSpecular = 0;
            
            for (var i = 0; i < Textures.Count; i++)
            {
                var type = Textures[i].Type;
                var num = type switch
                {
                    "diffuse" => Convert.ToString(numDiffuse++),
                    "specular" => Convert.ToString(numSpecular++),
                    _ => ""
                };
                shader.SetInt(type + Convert.ToString(num), i);
                Textures[i].Bind();
            }
            shader.SetVector3("camPos", camera.Position);
            shader.SetMatrix4("camMatrix", camera.CameraMatrix);
            GL.DrawElements(BeginMode.Triangles, Indices.Count, DrawElementsType.UnsignedInt, 0);
        }
    }
}