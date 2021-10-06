using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Esteria
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D adventureAnimationSheetRight;
        Texture2D adventureAnimationSheetLeft;
        Texture2D charDirection;

        Vector2 charPos = new Vector2(600,600);
        Vector2 velocity;
        bool hasJumped;


        //A Timer that stores milliseconds
        float timer;
        //An int that is the threshold for the timer
        int threshold;
        //An array that stores sourceRectangles for animations.
        Rectangle[] idleRectanglesRight;
        Rectangle[] idleRectanglesLeft;
        Rectangle[] runningRightRectangles;
        Rectangle[] runningLeftRectangles;
        Rectangle[] jumpingRight;
        Rectangle[] jumpingLeft;

        Rectangle[] charState;

        //These bytes tell the spriteBatch.Draw() what sourceRectangle to display
        byte currentAnimationIndex;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
         
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            base.Initialize();
            hasJumped = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            adventureAnimationSheetRight = Content.Load<Texture2D>("adventurer-Sheetx3");
            adventureAnimationSheetLeft = Content.Load<Texture2D>("04100222_adventurer-Sheetx3Flip");

            timer = 0;

            threshold = 250;

            idleRectanglesRight = new Rectangle[4];
            idleRectanglesRight[0] = new Rectangle(0, 0, 150, 111);
            idleRectanglesRight[1] = new Rectangle(150, 0, 150, 111);
            idleRectanglesRight[2] = new Rectangle(300, 0, 150, 111);
            idleRectanglesRight[3] = new Rectangle(450, 0, 150, 111);

            idleRectanglesLeft = new Rectangle[4];
            idleRectanglesLeft[0] = new Rectangle(900, 0, 150, 111);
            idleRectanglesLeft[1] = new Rectangle(750, 0, 150, 111);
            idleRectanglesLeft[2] = new Rectangle(600, 0, 150, 111);
            idleRectanglesLeft[3] = new Rectangle(450, 0, 150, 111);

            runningRightRectangles = new Rectangle[6];
            runningRightRectangles[0] = new Rectangle(150, 111, 150, 111);
            runningRightRectangles[1] = new Rectangle(300, 111, 150, 111);
            runningRightRectangles[2] = new Rectangle(450, 111, 150, 111);
            runningRightRectangles[3] = new Rectangle(600, 111, 150, 111);
            runningRightRectangles[4] = new Rectangle(750, 111, 150, 111);
            runningRightRectangles[5] = new Rectangle(900, 111, 150, 111);

            runningLeftRectangles = new Rectangle[6];
            runningLeftRectangles[0] = new Rectangle(0, 111, 150, 111);
            runningLeftRectangles[1] = new Rectangle(150, 111, 150, 111);
            runningLeftRectangles[2] = new Rectangle(300, 111, 150, 111);
            runningLeftRectangles[3] = new Rectangle(450, 111, 150, 111);
            runningLeftRectangles[4] = new Rectangle(600, 111, 150, 111);
            runningLeftRectangles[5] = new Rectangle(750, 111, 150, 111);

            jumpingRight = new Rectangle[10];
            jumpingRight[0] = new Rectangle(0, 222, 150, 111);
            jumpingRight[1] = new Rectangle(150, 222, 150, 111);
            jumpingRight[2] = new Rectangle(300, 222, 150, 111);
            jumpingRight[3] = new Rectangle(450, 222, 150, 111);
            jumpingRight[4] = new Rectangle(600, 222, 150, 111);
            jumpingRight[5] = new Rectangle(750, 222, 150, 111);
            jumpingRight[6] = new Rectangle(900, 222, 150, 111);
            jumpingRight[7] = new Rectangle(0, 333, 150, 111);
            jumpingRight[8] = new Rectangle(150, 333, 150, 111);
            jumpingRight[9] = new Rectangle(300, 333, 150, 111);

            jumpingLeft = new Rectangle[10];
            jumpingLeft[0] = new Rectangle(900, 222, 150, 111);
            jumpingLeft[1] = new Rectangle(750, 222, 150, 111);
            jumpingLeft[2] = new Rectangle(600, 222, 150, 111);
            jumpingLeft[3] = new Rectangle(450, 222, 150, 111);
            jumpingLeft[4] = new Rectangle(300, 222, 150, 111);
            jumpingLeft[5] = new Rectangle(150, 222, 150, 111);
            jumpingLeft[6] = new Rectangle(0, 222, 150, 111);
            jumpingLeft[7] = new Rectangle(900, 333, 150, 111);
            jumpingLeft[8] = new Rectangle(750, 333, 150, 111);
            jumpingLeft[9] = new Rectangle(600, 333, 150, 111);


            // This tells the animation to start on the left-side sprite.
            currentAnimationIndex = 0;

        }

        protected override void Update(GameTime gameTime)
        {
            charPos.Y += velocity.Y;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (charDirection == adventureAnimationSheetRight)
            {
                charDirection = adventureAnimationSheetRight;
            }
            else if (charDirection == adventureAnimationSheetLeft)
            {
                charDirection = adventureAnimationSheetLeft;
            }
            else
            {
                charDirection = adventureAnimationSheetRight;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                charDirection = adventureAnimationSheetLeft;
                charPos.X -= 5;
                charState = runningLeftRectangles;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                charDirection = adventureAnimationSheetRight;
                charPos.X += 5;
                charState = runningRightRectangles;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && hasJumped == false)
            {
                
                charPos.Y -= 10;
                velocity.Y = -8;
                hasJumped = true;
            }

            if (hasJumped == true)
            {
                float i = 1;
                velocity.Y += 0.3f * i;

                if (charDirection == adventureAnimationSheetRight)
                {
                    charState = jumpingRight;
                }
                else if (charDirection == adventureAnimationSheetLeft)
                {
                    charState = jumpingLeft;
                }
            }
            if (charPos.Y >= 600)
            {
                hasJumped = false;
            }
            if (hasJumped == false)
            {
                velocity.Y = 0;
            }





            if (!(Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A) || hasJumped == true))
            {
                if (charDirection == adventureAnimationSheetRight)
                {
                    if (charState != idleRectanglesRight)
                    {
                        currentAnimationIndex = 0;
                    }
                    charState = idleRectanglesRight;
                    

                }
                else if (charDirection == adventureAnimationSheetLeft)
                {

                    if (charState != idleRectanglesLeft)
                    {
                        currentAnimationIndex = 0;
                    }
                    charState = idleRectanglesLeft;
                    

                }

            }
          
            


            // Check if the timer has exceeded the threshold.
            if (timer > threshold)
            {

                if (charState == jumpingRight || charState == jumpingLeft)
                {
                    if (currentAnimationIndex >= jumpingRight.Length - 1)
                    {
                        currentAnimationIndex = 0;
                    }
                    else
                    {
                        currentAnimationIndex++;
                    }
                }

                else if (charState == idleRectanglesLeft || charState == idleRectanglesRight)
                {
                    if (currentAnimationIndex >= idleRectanglesLeft.Length - 1)
                    {
                        currentAnimationIndex = 0;
                    }
                    else
                    {
                        currentAnimationIndex++;
                    }
                }

               
                else if (charState == runningLeftRectangles || charState == runningRightRectangles)
                {
                    if (currentAnimationIndex >= runningRightRectangles.Length - 1)
                    {
                        currentAnimationIndex = 0;
                    }
                    else
                    {
                        currentAnimationIndex++;
                    }
                }

               

                // Reset the timer.
                timer = 0;
            }
            // If the timer has not reached the threshold, then add the milliseconds that have past since the last Update() to the timer.
            else
            {
                timer += (float)gameTime.ElapsedGameTime.Milliseconds + 30;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(charDirection, charPos, charState[currentAnimationIndex], Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
