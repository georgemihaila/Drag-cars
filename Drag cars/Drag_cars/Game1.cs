using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using Extensions;

namespace Drag_cars
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        #region Variables

        const int carCount = 8;
        const int randomSeed = 20170522;
        const int carWidth = 40;
        const int carHeight = 20;
        const float carLength = 4.5f;
        
        readonly Vector2 startingPosition = new Vector2(0, 155 + carHeight);
        GeneticCar[] car = new GeneticCar[carCount];
        SpriteFont font1;
        SpriteFont font2;
        Texture2D carTexture;
        Texture2D background;

        #endregion

        protected override void Initialize()
        {
            IsMouseVisible = true;
           // Window.AllowUserResizing = true;
            base.Initialize();
        }

        Random random = new Random((int)DateTime.Now.Ticks);
       
        protected override void LoadContent()
        {
            int c = 0;
            for (int i = 0; i < car.Length; i++)
            {
                car[i] = new GeneticCar(random.Next(), startingPosition);
                car[i].Position.Y = (i != 0) ? car[i - 1].Position.Y + 35 : startingPosition.Y;
                if (++c == 3)
                {
                    c = 0;
                    car[i].Position.Y += 10;
                }
            
            }
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            font2 = Content.Load<SpriteFont>("SpriteFont2");
            carTexture = Content.Load<Texture2D>("blueRaceCar");
            background = Content.Load<Texture2D>("background");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            
        }
        
        bool pause = false;
        bool ignore = false;
        int generation = 1;
        double frame = 0;
        int sGens = 0;

        protected override void Update(GameTime gameTime)
        {
            do
            {
                if (Keyboard.GetState().IsKeyDown(Keys.P) && !pause)
                {
                    pause = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.P) && pause)
                {
                    pause = false;
                }
                if (!pause)
                {
                    frame++;
                    for (int i = 0; i < car.Length; i++)
                    {
                        if (!car[i].ReachedDestination)
                        {
                            car[i].Speed += car[i].Acceleration / 60;
                            //float avs = (float)car[i].Position.X / (float)stopwatch.Elapsed.TotalSeconds;
                            //car[i].Position.X = (float)(car[i].Speed * stopwatch.Elapsed.TotalSeconds + (car[i].Acceleration * Math.Pow(stopwatch.Elapsed.TotalSeconds, 2) / 2));
                            car[i].Position.X += car[i].Speed / 60;
                            if (car[i].Position.X >= 800)
                            {
                                car[i].ReachedDestination = true;
                                car[i].TotalTime = frame / 60;
                            }
                        }
                    }
                }
                if (car.NumberOfCarsThatHaveFinishedTheRace() >= 4)
                {
                    AppendFile("results.txt", (frame / 60).ToString());
                    SelectionPool selectionPool = new SelectionPool(car, random.Next());
                    car = selectionPool.Select();
                    if (++sGens >= 500)
                    {
                        ignore = true;
                        sGens = 0;
                    }
                    frame = 0;
                    generation++;
                }
            }
            while (Keyboard.GetState().IsKeyDown(Keys.S) && (!ignore));
            if (ignore && Keyboard.GetState().IsKeyUp(Keys.S)) ignore = false;  //Set the index of the currently focused carrs
            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            for (int i = 0; i < car.Length; i++)
            {
                if ((mousePosition.Y >= car[i].Position.Y) && (mousePosition.Y <= car[i].Position.Y + carHeight))
                {
                    indexOfFocused = i;
                    break;
                }
            }
            base.Update(gameTime);
        }
      
        int indexOfFocused = 0;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 30), Color.White);
            for (int i = 0; i < car.Length; i++)
            {
                spriteBatch.Draw(carTexture, new Rectangle((int)(car[i].Position.X), (int)car[i].Position.Y, carWidth, carHeight), Color.White);
            }
            spriteBatch.DrawString(font1, "Currenly tracking car #" + indexOfFocused + "\n" + "Time elapsed: " + frame / 60 + "s\n" + car[indexOfFocused].ToString() + "\nMeters traveled: " + (car[indexOfFocused].Position.X)
                +"\nAverage speed: " + car[indexOfFocused].Position.X * 3.6 / car[indexOfFocused].TotalTime, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font2, "Generation " + generation, new Vector2(600, 10), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        private void AppendFile(string filename, string s)
        {
            if (System.IO.File.Exists(filename))
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
            {
                    s = sr.ReadToEnd() + s;
                    sr.Close();
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
            {
                sw.WriteLine(s);
                sw.Close();
            }
        }
    }
}
