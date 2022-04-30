using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace FinalGameProject
{
    public class Bullet
    {
        public static Texture2D texture;
        public static int screenWidth;
        public static int screenHeight;
        static int bulletWidth;
        static int bulletHeight;
        public static double speed;
        public static Vector2 origin;

        public bool isActive;
        public double X;
        public double Y;
        public CircleHitBox hitBox;

        public bool dead = false;

        public double rotation;

        public Bullet(FinalGameProject game, ContentManager content)
        {
            texture = content.Load<Texture2D>("projectile");
            screenHeight = game._graphics.PreferredBackBufferHeight;
            screenWidth = game._graphics.PreferredBackBufferWidth;
            bulletWidth = texture.Width;
            bulletHeight = texture.Height;
            speed = 18;
            origin = new Vector2(bulletWidth / 2, bulletHeight / 2);

            isActive = false;
        }

        public Bullet SpawnBullet(double playerX, double PlayerY, double playerRotation)
        {
            isActive = true;
            dead = false;
            rotation = playerRotation;
            X = playerX;
            Y = PlayerY;
            hitBox = new CircleHitBox(10, X, Y);
            return this;
        }

        public void Update()
        {
            X += Math.Sin(ConvertToRadians(rotation)) * speed;
            Y -= Math.Cos(ConvertToRadians(rotation)) * speed;

            hitBox.X = X;
            hitBox.Y = Y;

            if (X > screenWidth + 10 || X < -10 || Y > screenHeight + 10 || Y < -10)
            {
                dead = true;
            }
        }

        private float ConvertToRadians(double degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)X, (int)Y, 70, 70), null, Color.White, (float)ConvertToRadians(rotation), origin, SpriteEffects.None, 0);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("projectile");
        }
    }
}
