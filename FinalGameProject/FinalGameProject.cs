using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FinalGameProject
{
    public class FinalGameProject : Game
    {
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Player player;
        List<Mig25> migs;
        int level;
        Texture2D background;
        public float soundEffectVolume;
        public int FRAME_RATE = 60;
        int score;
        SpriteFont _font;
        bool isGameOver;
        int width;
        int height;
        int drawCount;
        ModelBullet bulletModel;
        bool nearX;
        bool nearY;

        public FinalGameProject()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            player = new Player(this, Content);
        }

        protected override void LoadContent()
        {
            background = Content.Load<Texture2D>("tilesetOpenGameBackground");
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _font = Content.Load<SpriteFont>("GameFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);
            _spriteBatch.Begin();
            Rectangle r = new Rectangle(new Point(0, 0), new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
            _spriteBatch.Draw(background, r, Color.White);

            if (!isGameOver)
            {
                player.Draw(_spriteBatch);
                _spriteBatch.DrawString(_font, "Score: " + score, new Vector2(_graphics.PreferredBackBufferWidth / 2, 10), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
            }
            else
            {
                _spriteBatch.DrawString(_font, "Score: " + score, new Vector2((_graphics.PreferredBackBufferWidth / 2) - 200, (_graphics.PreferredBackBufferHeight / 2) - 100), Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1);
                _spriteBatch.DrawString(_font, "Press R to play again or ESC to quit", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 530, (_graphics.PreferredBackBufferHeight / 2)), Color.White, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1);
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void GameOver()
        {
            migs = null;
            player = null;
            isGameOver = true;
        }
    }
}
