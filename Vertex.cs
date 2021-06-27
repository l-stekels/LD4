using OpenTK.Mathematics;

namespace LD4
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Color;
        public Vector2 TexUv;

        public Vertex(Vector3 position, Vector3 color, Vector3 normal, Vector2 texCoord) => (Position, Color, Normal, TexUv) = (position, color, normal, texCoord);

        public Vertex(Vector3 position, Vector3 colors, Vector2 texCoord)
        {
            (Position, Color, TexUv) = (position, colors, texCoord);
            Normal = Vector3.One;
        }

        public Vertex(Vector3 position)
        {
            Position = position;
            Normal = Vector3.One;
            Color = Vector3.Zero;
            TexUv = Vector2.Zero;
        }

        public Vertex(Vector3 position, Vector2 texCoord)
        {
            Position = position;
            Normal = Vector3.One;
            Color = Vector3.One;
            TexUv = texCoord;
        }
    }
}