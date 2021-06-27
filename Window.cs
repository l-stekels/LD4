using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LD4
{
    public class Window : GameWindow
    {
        private readonly string[] _vertexShaderPath = {"Shaders", "default.vert.glsl"};
        private readonly string[] _fragmentShaderPath = {"Shaders", "default.frag.glsl"};
        private readonly string[] _vertexLightShaderPath = {"Shaders", "light.vert.glsl"};
        private readonly string[] _fragmentLightShaderPath = {"Shaders", "light.frag.glsl"};

        private readonly string[] _containerTexturePath = {"Resources", "container.png"};
        private readonly string[] _planksPath = {"Resources", "planks.png"};
        private readonly string[] _planksSpecPath = {"Resources", "planksSpec.png"};

        private Vertex[] _pyramidVertices =
        {
            new(
                new Vector3(-0.5f, 0.0f,  0.5f),
                new Vector3(0.83f, 0.70f, 0.44f),
                new Vector2(0.0f, 0.0f)
            ),
            new(
                new Vector3(-0.5f, 0.0f, -0.5f),
                new Vector3(0.83f, 0.70f, 0.44f),
                new Vector2(5.0f, 0.0f)
            ),
            new(
                new Vector3(0.5f, 0.0f, -0.5f),
                new Vector3(0.83f, 0.70f, 0.44f),
                new Vector2(0.0f, 0.0f)
            ),
            new(
                new Vector3(0.5f, 0.0f,  0.5f),
                new Vector3(0.83f, 0.70f, 0.44f),
                new Vector2(5.0f, 0.0f)
            ),
            new(
                new Vector3(0.0f, 0.8f,  0.0f),
                new Vector3(0.83f, 0.70f, 0.44f),
                new Vector2(2.5f, 5.0f)
            )
        };

        private int[] _pyramidIndices =
        {
            0, 1, 2,
            0, 2, 3,
            0, 1, 4,
            1, 2, 4,
            2, 3, 4,
            3, 0, 4
        };

        private Shader _shader;

        private Mesh _pyramid;

        private double _time;

        private Camera _camera;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(
            gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            GL.Viewport(0, 0, Size.X, Size.Y);

            var pyramidTexture = new Texture(Path.Combine(_containerTexturePath), 0);

            _shader = new Shader(Path.Combine(_vertexShaderPath), Path.Combine(_fragmentShaderPath));

            _pyramid = new Mesh(
                _pyramidVertices.ToList(),
                _pyramidIndices.ToList(),
                new List<Texture> {pyramidTexture}
            );


            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(Size.X, Size.Y, new Vector3(0f, 0f, 2.0f));

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            _time += 16.0 * args.Time;
            GL.ClearColor(Color.DarkSlateGray);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _camera.UpdateMatrix(45f, 0.1f, 100f);

            _pyramid.Draw(ref _shader, ref _camera);

            GL.Flush();
            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.Height = Size.X;
            _camera.Width = Size.Y;
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (!IsFocused)
            {
                return;
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            _camera.Inputs(KeyboardState, args.Time);

            base.OnUpdateFrame(args);
        }
    }
}