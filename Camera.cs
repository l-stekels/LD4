using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LD4
{
    public class Camera
    {
        public Vector3 Position;
        public Vector3 Orientation = new(0.0f, 0.0f, -1.0f);
        public Vector3 Up = new(0.0f, 1.0f, 0.0f);
        public int Width;
        public int Height;
        public float Speed = 0.1f;
        public Matrix4 CameraMatrix = Matrix4.Identity;

        public Camera(int width, int height, Vector3 position) => (Width, Height, Position) = (width, height, position);

        public void UpdateMatrix(float foVdeg, float nearPlane, float farPlane)
        {
            var view = Matrix4.LookAt(Position, Position + Orientation, Up);
            var projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(foVdeg),
                Width / (float) Height,
                nearPlane,
                farPlane
            );
            CameraMatrix = view * projection;
        }

        public void Inputs(KeyboardState state, double deltaTime)
        {
            if (state.IsKeyDown(Keys.W))
            {
                Position += Speed * Orientation;
            }

            if (state.IsKeyDown(Keys.A))
            {
                Position += Speed * -Vector3.Normalize(Vector3.Cross(Orientation, Up));
            }

            if (state.IsKeyDown(Keys.S))
            {
                Position += Speed * -Orientation;
            }

            if (state.IsKeyDown(Keys.D))
            {
                Position += Speed * Vector3.Normalize(Vector3.Cross(Orientation, Up));
            }

            if (state.IsKeyDown(Keys.Space))
            {
                Position += Speed * Up;
            }

            if (state.IsKeyDown(Keys.LeftControl))
            {
                Position += Speed * -Up;
            }

            Speed = state.IsKeyDown(Keys.LeftShift) ? 0.4f : 0.1f;
        }
    }
}