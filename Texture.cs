using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace LD4
{
    public class Texture
    {
        public int Id;

        public Texture(
            string path,
            int slot,
            PixelFormat pixelFormat = PixelFormat.Bgra,
            PixelType pixelType = PixelType.UnsignedByte
        )
        {
            GL.GenTextures(1, out Id);

            GL.ActiveTexture(TextureUnit.Texture0 + slot);
            GL.BindTexture(TextureTarget.Texture2D, Id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);

            var pixels = Image(path, out var image);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                pixelFormat,
                pixelType,
                pixels
            );
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void TexUnit(ref Shader shader, string uniform, int unit)
        {
            shader.SetInt(uniform, unit);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Delete()
        {
            GL.DeleteTextures(1, ref Id);
        }

        private static byte[] Image(string path, out Image<Rgba32> image)
        {
            image = SixLabors.ImageSharp.Image.Load<Rgba32>(path);
            image.Mutate(x => x.Flip(FlipMode.Vertical));
            List<byte> pixels = new(4 * image.Width * image.Height);

            for (var y = 0; y < image.Height; y++)
            {
                var row = image.GetPixelRowSpan(y);
                for (var x = 0; x < image.Width; x++)
                {
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].A);
                }
            }

            return pixels.ToArray();
        }
    }
}