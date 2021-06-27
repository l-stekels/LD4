using System;
using OpenTK.Graphics.OpenGL;

namespace LD4
{
    public class VAO
    {
        public int Id;
        
        public VAO()
        {
            GL.GenVertexArrays(1, out Id);
        }

        public void LinkAttrib(ref VBO vbo, int layout, int numComponents, VertexAttribPointerType type, int stride, IntPtr offset)
        {
            vbo.Bind();
            GL.VertexAttribPointer(layout, numComponents, type, false, stride, offset);
            GL.EnableVertexAttribArray(layout);
            vbo.Unbind();
        }

        public void Bind()
        {
            GL.BindVertexArray(Id);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Delete()
        {
            GL.DeleteVertexArrays(1, ref Id);
        }
    }
}