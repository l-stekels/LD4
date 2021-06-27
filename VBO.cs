using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace LD4
{
    public class VBO
    {
        public int Id;

        public VBO(List<Vertex> vertices)
        {
            GL.GenBuffers(1, out Id);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
            var size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vertex));
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * size, vertices.ToArray(), BufferUsageHint.StaticDraw);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Delete()
        {
            GL.DeleteBuffers(1, ref Id);
        }
    }
}