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
        int WINDOW_WIDTH = 1920;
        int WINDOW_HEIGHT = 1080;
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Player player;
        List<Mig25> migs;
        List<Mig25> closeMigs;
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
        int maxMigs;
        int migCount;
        SoundEffect migDestroyed;

        public FinalGameProject()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            SetGraphics();
            migCount = 0;
            migs = new List<Mig25>();
            closeMigs = new List<Mig25>();
            migCount = 0;
            width = _graphics.PreferredBackBufferWidth;
            height = _graphics.PreferredBackBufferHeight;
            SetMigs();

            // TODO: Add your initialization logic here

            base.Initialize();
            player = new Player(this, Content);
            score = 0;
            isGameOver = false;
        }

        protected override void LoadContent()
        {
            background = Content.Load<Texture2D>("tilesetOpenGameBackground");
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _font = Content.Load<SpriteFont>("GameFont");
            migDestroyed = Content.Load<SoundEffect>("PlaneExploding");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!isGameOver)
            {
                Console.WriteLine(closeMigs.Count);
                player.Update();
                List<Mig25> MigsDestroyedByPlayer = new List<Mig25>();
                List<Mig25> MigsOffScreen = new List<Mig25>();
                List<Bullet> BulletsHitMig = new List<Bullet>();
                foreach (Mig25 mig in migs)
                {
                    if (((mig.hitBox.X + (mig.hitBox.radius) > (player.X - player.hitbox.radius)) // right side of tango and left side of player
                        && (mig.hitBox.X + (mig.hitBox.radius) < (player.X + player.hitbox.radius))) // right side of tango and right side of player
                        || ((mig.hitBox.X - (mig.hitBox.radius) < (player.X + player.hitbox.radius)) // left side of tango and right side of player
                        && (mig.hitBox.X - (mig.hitBox.radius) > (player.X - player.hitbox.radius)))) // left side of tango and left side of player
                    {
                        nearX = true;
                    }
                    else
                    {
                        nearX = false;
                    }
                    if (((mig.hitBox.Y + (mig.hitBox.radius) > (player.Y - player.hitbox.radius)) // bottom of tango and top of player
                        && (mig.hitBox.Y + (mig.hitBox.radius) < (player.Y + player.hitbox.radius))) // bottom of tango and bottom of player
                        || ((mig.hitBox.Y - (mig.hitBox.radius) < (player.Y + player.hitbox.radius)) // top of tango and bottom of player
                        && (mig.hitBox.Y - (mig.hitBox.radius) > (player.Y - player.hitbox.radius)))) // top of tango and top of player
                    {
                        nearY = true;
                    }
                    else
                    {
                        nearY = false;
                    }
                    if (nearX && nearY)
                    {
                        if (!closeMigs.Contains(mig))
                        {
                            closeMigs.Add(mig);
                        }
                    }
                    else
                    {
                        try
                        {
                            closeMigs.Remove(mig);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    mig.Update(gameTime);

                    foreach (Bullet b in player.activeBullets)
                    {
                        if (mig.hitBox.CollidesWith(b.hitBox) && !mig.Hit)
                        {
                            MigsDestroyedByPlayer.Add(mig);
                            b.dead = true;
                            mig.Exploding = true;
                            mig.Hit = true;
                            migDestroyed.Play(soundEffectVolume, 0, 0);
                            level++;
                            score += 10;
                        }
                    }

                    if (mig.OffScreen)
                    {
                        MigsOffScreen.Add(mig);
                    }

                }

                foreach (Mig25 mig in closeMigs)
                {
                    if (mig.hitBox.CollidesWith(player.hitbox) && !mig.Exploding) // game over
                    {
                        GameOver();
                        return;
                    }
                }
                RemoveMigs(MigsDestroyedByPlayer);
                RemoveMigs(MigsOffScreen);
                if (migs.Count < level && migs.Count < maxMigs)
                {
                    migs.Add(new Mig25(this, Content, player.X, player.Y));

                }

                base.Update(gameTime);
            }
            else
            {
                // game is over here..

                var k = Keyboard.GetState();
                if (k.IsKeyDown(Keys.R))
                {
                    Initialize();
                }
                if (drawCount == 2)
                {
                    SuppressDraw();
                }
                else
                {
                    drawCount++;
                }

            }

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
                foreach (Mig25 m in migs)
                {
                    m.Draw(_spriteBatch);
                }
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

        private void SetGraphics()
        {
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            _graphics.ApplyChanges();
        }

        private void SetMigs()
        {
            if (WINDOW_WIDTH <= 2000)
            {
                maxMigs = 4;

            }
            else if (WINDOW_WIDTH <= 2500)
            {
                maxMigs = 8;
            }
            else if (WINDOW_WIDTH <= 3000)
            {
                maxMigs = 10;
            }
            else
            {
                maxMigs = 12;
            }
        }
        private void RemoveMigs(List<Mig25> ToDeleteMig)
        {
            foreach (Mig25 m in ToDeleteMig)
            {
                if (!m.Exploding)
                {
                    migs.Remove(m);
                    try
                    {
                        closeMigs.Remove(m);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }
    }
}
