using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace FinalGameProject
{
    class Player
    {
        Texture2D texture;
        SoundEffect shoot;
        float soundEffectVolControl;
        public CircleHitBox hitbox;
        double rotation;
        Vector2 origin;

        public double X;
        public double Y;
        static int screenWidth;
        static int screenHeight;

        public int playerWidth;
        public int playerHeight;

        double speed;

        public Stack<Bullet> inactiveBullets;
        public List<Bullet> activeBullets;
        ContentManager content;
        Game1 game;

        bool shootLock = false;
        static int maxBullets = 20;

        public Player(Game1 game, ContentManager content)
        {
            screenHeight = game._graphics.PreferredBackBufferHeight;
            screenWidth = game._graphics.PreferredBackBufferWidth;
            this.content = content;
            LoadContent(content);
            this.game = game;
            this.soundEffectVolControl = game.soundEffectVolume;
            X = screenWidth / 2;
            Y = screenHeight / 2;
            rotation = 0;
            inactiveBullets = new Stack<Bullet>();
            activeBullets = new List<Bullet>();
            InitializeBullets(maxBullets);
            playerWidth = texture.Width;
            playerHeight = texture.Height;

            origin = new Vector2(playerWidth / 2, playerHeight / 2);

            speed = 8;

            hitbox = new CircleHitBox(40, X, Y);

        }

        private void InitializeBullets(int n)
        {
            int counter = 0;
            while (counter < n)
            {
                inactiveBullets.Push(new Bullet(game, content));
                counter++;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)X, (int)Y, 300, 200), null, Color.White, ConvertToRadians(rotation + 180), origin, SpriteEffects.None, 0);
            foreach (Bullet b in activeBullets)
            {
                if (b.isActive)
                {
                    b.Draw(spriteBatch);
                }
            }
        }

        private float ConvertToRadians(double degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        private double ConvertToDegrees(double degrees)
        {
            return -1;
        }

        public void Update()
        {

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                rotation -= speed * 0.7;

            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                rotation += speed * 0.7;

            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                X += Math.Sin(ConvertToRadians(rotation)) * speed;
                Y -= Math.Cos(ConvertToRadians(rotation)) * speed;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                X -= Math.Sin(ConvertToRadians(rotation)) * speed;
                Y += Math.Cos(ConvertToRadians(rotation)) * speed;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (!shootLock)
                {
                    Shoot();
                    shootLock = true;
                }

            }
            else
            {
                shootLock = false;
            }

            if (X < 0)
            {
                X = 0;
            }
            if (X > screenWidth)
            {
                X = screenWidth;
            }
            if (Y < 0)
            {
                Y = 0;
            }
            if (Y > screenHeight)
            {
                Y = screenHeight;
            }

            hitbox.X = X;
            hitbox.Y = Y;

            List<Bullet> temp = new List<Bullet>();
            foreach (Bullet b in activeBullets)
            {
                b.Update();
                if (b.dead)
                {
                    temp.Add(b);
                }
            }
            foreach (Bullet b in temp)
            {
                b.dead = false;
                b.isActive = false;
                inactiveBullets.Push(b);
                activeBullets.Remove(b);
            }
        }

        private void Shoot()
        {
            Console.WriteLine("active: " + activeBullets.Count + "inactive: " + inactiveBullets.Count);
            try
            {
                Bullet b = inactiveBullets.Pop().SpawnBullet(X, Y, rotation);
                b.isActive = true;
                activeBullets.Add(b);

            }
            catch (Exception e)
            {
                Console.WriteLine("Failed");
                return;
            }
            shoot.Play(soundEffectVolControl * (float)0.4, 0, 0);

        }


        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("f-16_blank");
            shoot = content.Load<SoundEffect>("Bullet_Shot");
        }
    }
}
