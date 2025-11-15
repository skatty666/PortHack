using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Hacknet.Effects;

namespace PortHackUI
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private PortHackCubeSequence _sequence;
        private MatrixRain _matrixRain;
        private TerminalUI _terminalUI;
        private SpriteFont _font;
        private SpriteFont _uiFont;
        private float _totalTime = 8.6f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            // 65x25 Cells Terminal-Größe (ca. 8x16 pixel pro char)
            _graphics.PreferredBackBufferWidth = 65 * 12;  // 780
            _graphics.PreferredBackBufferHeight = 25 * 18; // 450
        }

        protected override void Initialize()
        {
            // Erstelle Komponenten VOR base.Initialize()
            _matrixRain = new MatrixRain();
            _sequence = new PortHackCubeSequence();
            _terminalUI = new TerminalUI();
            
            base.Initialize();
            
            Cube3D.Initialize(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Lade beide Fonts
            _font = CreateFont("Font");      // Größerer Font für Matrix
            _uiFont = CreateFont("UIFont");  // Kleinerer Font für UI
            
            if (_matrixRain != null)
            {
                _matrixRain.LoadContent(_font);
            }
            
            if (_terminalUI != null)
            {
                _terminalUI.LoadContent(_uiFont, GraphicsDevice);
            }
        }
        
        private SpriteFont CreateFont(string name)
        {
            try
            {
                return Content.Load<SpriteFont>(name);
            }
            catch
            {
                return null;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _matrixRain.Update(deltaTime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _terminalUI.Update(deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Rectangle dest = new Rectangle(0, 0, 
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);

            _sequence.DrawSequence(dest, deltaTime, _totalTime);

            // Matrix Rain und UI über den Würfeln
            _spriteBatch.Begin();
            _matrixRain.Draw(_spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _terminalUI.Draw(_spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}