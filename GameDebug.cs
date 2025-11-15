using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Hacknet.Effects;

namespace PortHackDebug
{
    public class GameDebug : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        
        private MouseState _previousMouseState;
        private bool _isDragging = false;
        private Point _dragStart;
        
        private KeyboardState _previousKeyboardState;
        
        public static float Rechts = 0f;
        public static float Oben = 0f;
        public static float Distanz = 17.2f;
        public static float Pan = 135f;
        public static float Tilt = 35.4f;
        public static float Roll = 0f;
        
        private float mouseSensitivity = 0.3f;
        
        public GameDebug()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            Cube3DDebug.Initialize(GraphicsDevice);
            Cube3DDebug.UpdateCamera(Rechts, Oben, Distanz, Pan, Tilt, Roll);
            _previousMouseState = Mouse.GetState();
            _previousKeyboardState = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            try
            {
                _font = Content.Load<SpriteFont>("DebugFont");
            }
            catch
            {
                _font = null;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();
            
            _previousKeyboardState = keyboardState;
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            RenderAllCubes();

            base.Draw(gameTime);
        }
        
        private void RenderAllCubes()
        {
            Cube3DDebug.RenderCubeWithDiagonals(
                new Vector3(0f, 0f, 0f),
                3.0f,
                new Vector3(0f, 0f, 0f),  // Keine Rotation - Würfel bleibt achsenausgerichtet
                Color.White
            );
        }
        

    }
}