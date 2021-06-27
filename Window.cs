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

        private readonly string[] _stonePath = {"Resources", "stone.png"};
        private readonly string[] _stoneSpecPath = {"Resources", "stoneSpec.png"};
        private readonly string[] _planksPath = {"Resources", "planks.png"};
        private readonly string[] _planksSpecPath = {"Resources", "planksSpec.png"};

        private Vertex[] _pyramidVertices =
        {
            new(
                new Vector3(-0.5f, 0.0f, 0.5f),
                new Vector3(0.83f, 0.70f, 0.44f),
                Vector3.One,
                new Vector2(0.0f, 0.0f)
            ),
            new(
                new Vector3(-0.5f, 0.0f, -0.5f),
                new Vector3(0.83f, 0.70f, 0.44f),
                Vector3.One,
                new Vector2(1.5f, 0.0f)
            ),
            new(
                new Vector3(0.5f, 0.0f, -0.5f),
                new Vector3(0.83f, 0.70f, 0.44f),
                Vector3.One,
                new Vector2(0.0f, 0.0f)
            ),
            new(
                new Vector3(0.5f, 0.0f, 0.5f),
                new Vector3(0.83f, 0.70f, 0.44f),
                Vector3.One,
                new Vector2(1.5f, 0.0f)
            ),
            new(
                new Vector3(0.0f, 0.8f, 0.0f),
                new Vector3(0.83f, 0.70f, 0.44f),
                Vector3.One,
                new Vector2(0.5f, 1.5f)
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

        private Vertex[] _floorVertices =
        {
            new(
                new Vector3(-1.0f, 0.0f, 1.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
                Vector3.One,
                new Vector2(0.0f, 0.0f)
            ),
            new(
                new Vector3(-1.0f, 0.0f, -1.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
                Vector3.One,
                new Vector2(0.0f, 1.0f)
            ),
            new(
                new Vector3(1.0f, 0.0f, -1.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
                Vector3.One,
                new Vector2(1.0f, 1.0f)
            ),
            new(
                new Vector3(1.0f, 0.0f, 1.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
                Vector3.One,
                new Vector2(1.0f, 0.0f)
            )
        };

        private int[] _floorIndices =
        {
            0, 1, 2,
            0, 2, 3
        };

        private Vertex[] _lightVertices =
        {
            new(new Vector3(-0.1f, -0.1f, 0.1f)),
            new(new Vector3(-0.1f, -0.1f, -0.1f)),
            new(new Vector3(0.1f, -0.1f, -0.1f)),
            new(new Vector3(0.1f, -0.1f, 0.1f)),
            new(new Vector3(-0.1f, 0.1f, 0.1f)),
            new(new Vector3(-0.1f, 0.1f, -0.1f)),
            new(new Vector3(0.1f, 0.1f, -0.1f)),
            new(new Vector3(0.1f, 0.1f, 0.1f)),
        };

        private int[] _lightIndices =
        {
            0, 1, 2,
            0, 2, 3,
            0, 4, 7,
            0, 7, 3,
            3, 7, 6,
            3, 6, 2,
            2, 6, 5,
            2, 5, 1,
            1, 5, 4,
            1, 4, 0,
            4, 5, 6,
            4, 6, 7
        };

        private Shader _shader;
        private Shader _lightShader;

        private Mesh _pyramid;

        private Mesh _floor;

        private Mesh _light;

        private double _time;

        private Camera _camera;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(
            gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            GL.Viewport(0, 0, Size.X, Size.Y);

            Texture[] pyramidTextures = 
            {
                new Texture(Path.Combine(_stonePath), "diffuse", 0),
                new Texture(Path.Combine(_stoneSpecPath), "specular", 1)
            };
            Texture[] floorTextures =
            {
                new(Path.Combine(_planksPath), "diffuse", 0),
                new(Path.Combine(_planksSpecPath), "specular", 1),
            };

            _shader = new Shader(Path.Combine(_vertexShaderPath), Path.Combine(_fragmentShaderPath));
            _lightShader = new Shader(Path.Combine(_vertexLightShaderPath), Path.Combine(_fragmentLightShaderPath));

            _floor = new Mesh(_floorVertices.ToList(), _floorIndices.ToList(), floorTextures.ToList());
            _light = new Mesh(_lightVertices.ToList(), _lightIndices.ToList(), new List<Texture>());
            _pyramid = new Mesh(_pyramidVertices.ToList(), _pyramidIndices.ToList(), pyramidTextures.ToList());

            var lightColor = Vector4.One;
            var lightPos = new Vector3(0.5f, 1.5f, 0f);
            var lightModel = Matrix4.CreateTranslation(lightPos);

            _lightShader.Use();
            _lightShader.SetMatrix4("model", lightModel);
            _lightShader.SetVector4("lightColor", lightColor);

            var objectPos = Vector3.Zero;
            var objectModel = Matrix4.CreateTranslation(objectPos);

            _shader.Use();
            _shader.SetMatrix4("model", objectModel);
            _shader.SetVector4("lightColor", lightColor);
            _shader.SetVector3("lightPos", lightPos);

            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(Size.X, Size.Y, new Vector3(0f, 1f, 4.0f));

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            _time += 16.0 * args.Time;
            GL.ClearColor(Color.DarkSlateGray);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _camera.UpdateMatrix(40, 0.1f, 100f);

            _floor.Draw(ref _shader, ref _camera);
            _pyramid.Draw(ref _shader, ref _camera);
            _light.Draw(ref _lightShader, ref _camera);

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