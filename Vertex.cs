using OpenTK.Mathematics;

namespace LD4
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 Color;
        public Vector2 TexUv;

        public Vertex(Vector3 position, Vector3 colors, Vector2 texCoord) =>
            (Position, Color, TexUv) = (position, colors, texCoord);
    }
}