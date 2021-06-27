using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace LD4
{
    public class EBO
    {
        public int Id;

        public EBO(List<int> indices)
        {
            GL.GenBuffers(1, out Id);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
            var size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vertex));
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * size, indices.ToArray(), BufferUsageHint.StaticDraw);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Delete()
        {
            GL.DeleteBuffers(1, ref Id);
        }
    }
}