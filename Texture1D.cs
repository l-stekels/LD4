using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace LD4
{
    public class Texture1D
    {
        public int Id;
        
        public Texture1D()
        {
            Id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture1D, Id);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            List<byte> pixels = new();
            for (var i = 0; i < 128; i += 4)
            {
                pixels.Add((byte)new Random().Next(1, 255));
                pixels.Add((byte)new Random().Next(1, 255));
                pixels.Add((byte)new Random().Next(1, 255));
                pixels.Add((byte)new Random().Next(1, 255));
            }
            GL.TexImage1D(TextureTarget.Texture1D, 0, PixelInternalFormat.Rgba, pixels.Count * sizeof(byte), 0, PixelFormat.Bgra, PixelType.Byte, pixels.ToArray());
        }
        
        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture1D, Id);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture1D, 0);
        }

        public void Delete()
        {
            GL.DeleteTextures(1, ref Id);
        }
    }
}