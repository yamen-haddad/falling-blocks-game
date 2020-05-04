using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Windows.Forms;

namespace _2nd_homework
{
    class FallingBlocks : GameWindow
    {
        static string TITLE = "Falling Blocks";
        static int WIDTH = 400;
        static int HEIGHT = 600;
        //the dimentions of the feild array:
        static int n = 20, m = 10;
        //gameMap[i,j]==1;    this block is occupied by player 1
        //gameMap[i, j]==2;   this block is occupied by player 2
        //gameMap[i, j]==0;   this block is not occupied by any player 
        static int[,] gameMap = new int[n + 1, m + 1];
        //the dimention of a single square
        static double len = Math.Max((double)WIDTH / m, (double)HEIGHT / n);
        static double viewlen = 2 * len / windowWidth;
        //the dimentions of the game window
        static double windowWidth=len*m, windowHeight=len*n;
        //to set the speed of the game
        static int speed = 7;
        //to save the position of the current moving square
        static int currentI = 0, currentJ = 0;
        //to determine when we need to make a new cube to start falling
        static bool cubeLanded = true;
        //to determine which player is currently playing
        //players are numbered from: 0 to numOfPlayers-1
        static int numOfPlayers = 2;
        static int currentPlayer = 0;
        static int[] results = new int[numOfPlayers + 1];
        static Color[] playersColors = new Color[] { Color.Red,Color.Green,Color.Blue,Color.Yellow,Color.Cyan,Color.Magenta,Color.Black};
        //the minimum number of adjacent blocks
        static int minCubes = 3;
        //to make the game faster in responding to keyboard & slower in motion
        static int rate = 0;
        //the bigger the responcitivity the higher the game sense the keyboard strokes
        static int responcitivity = 3;
        //to save the status of the game (paused or not paused)
        bool gamePaused = false;
        public FallingBlocks(int num,int resp,int numOfCubes) : base(WIDTH, HEIGHT, GraphicsMode.Default, TITLE)
        {
            numOfPlayers = num;
            responcitivity = resp;
            rate = 0;
            results = new int[numOfPlayers + 1];
            minCubes = numOfCubes;
            gamePaused = false;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.LightGray);
            currentI = 0;
            currentJ = m / 2;
            cubeLanded = true;
            currentPlayer = 0;
        }

        //to save the map after the current cube landed some were
        void saveGameMape()
        {
            gameMap[currentI, currentJ] = currentPlayer + 1;
        }
        bool checkLines()
        {
            bool res = false;
            for (int i = n - 1; i >= 0; i--)
            {
                res=res || checkOneLine(i);
            }
            return res;
        }
        //to check one line for horizontal win
        bool checkOneLine(int line)
        {
            int count = 1;
            bool res = false;
            int p1=0;
            int p2 = 1;
            while (true)
            {
                while (p2 < m && gameMap[line, p1]!=0 && gameMap[line, p1]==gameMap[line,p2])
                {
                    count++;
                    p2++;
                }
                if (count >= minCubes)
                {
                    deleteRow(line, p1, p1+count);
                    res = true;
                }
                if (p2 == m)
                    break;
                p1 = p2;
                p2++;
                count = 1;
            }
            return res;
        }
        //to delete the row number Line from column start to column end
        void deleteRow(int line, int start, int end)
        {
            //before deleting the row add the result to the player result
            results[gameMap[line, start] - 1]++;
            Console.WriteLine("detect a hit on line: " + line);
            printResults();
            for (int i = line; i > 0; i--)
            {
                for (int j = start; j < end; j++)
                {
                    gameMap[i, j] = gameMap[i - 1, j];
                }
            }
            for (int j = start; j < end; j++)
            {
                gameMap[0, j] = 0;
            }
        }

        bool checkColumns()
        {
            bool res = false;
            for (int i = m - 1; i >= 0; i--)
            {
                res=res || checkOneColumn(i);
            }
            return res;
        }
        //to check one line for horizontal win
        bool checkOneColumn(int col)
        {
            bool res = false;
            int count = 1;

            int p1 = n-1;
            int p2 = n-2;
            while (true)
            {
                while (p2 >= 0 && gameMap[p1, col] != 0 && gameMap[p1, col] == gameMap[p2, col])
                {
                    count++;
                    p2--;
                }
                if (count >= minCubes)
                {
                    deleteColumn(col, p1, p1-count);
                    res = true;
                }
                if (p2 == -1)
                    break;
                p1 = p2;
                p2--;
                count = 1;
            }
            return res;
        }
        //to delete the row number Line from column start to column end
        void deleteColumn(int col, int start, int end)
        {
            //before deleting the column add the result to the player result
            results[gameMap[start,col]-1]++;
            Console.WriteLine("detect a hit on column: " + col);
            printResults();
            int len = start - end;
            for (int i = start; i >= len; i--)
            {
                gameMap[i, col] = gameMap[i - len, col];
            }
            for (int j = len-1; j >=0; j--)
            {
                gameMap[j, col] = 0;
            }
        }
        void printResults()
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                Console.WriteLine("player[" + i + "] result is: " + results[i]);
            }
        }
        bool checkMap()
        {
            return checkColumns() ||checkLines();
        }
        void moveDown()
        {
            if (currentI == n - 1 || gameMap[currentI+1, currentJ] != 0)
            {
                cubeLanded = true;
                //to change the player
                saveGameMape();
                //in this loop we uarantee recursive block checking for multiple cube hits from one cube fall
                while(checkMap());
                currentPlayer = (currentPlayer + 1) % numOfPlayers;
                currentI = 0;
                currentJ = m / 2;
            }
            else
            {
                cubeLanded = false;
                currentI++;
            }
        }
        void moveLeft()
        {
            if (currentJ != 0 && gameMap[currentI, currentJ-1] == 0)
            {
                currentJ--;
            }
        }
        void moveRight()
        {
            if (currentJ != m-1 && gameMap[currentI, currentJ + 1] == 0)
            {
                currentJ++;
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);


            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard[OpenTK.Input.Key.Escape] || Keyboard[OpenTK.Input.Key.Q])
            {
                Exit();
            }
            if (Keyboard[OpenTK.Input.Key.P])
            {
                gamePaused = true;
            }
            if (Keyboard[OpenTK.Input.Key.R])
            {
                gamePaused = false;
            }
            if (!gamePaused)
            {
                if (Keyboard[OpenTK.Input.Key.Left])
                {
                    moveLeft();
                }
                if (Keyboard[OpenTK.Input.Key.Right])
                {
                    moveRight();
                }
                if (Keyboard[OpenTK.Input.Key.Down])
                {
                    moveDown();
                }
                if (rate == responcitivity)
                {
                    moveDown();
                    rate = 0;
                }
                rate++;
            }
        }
        //to draw a map
        void drawMap()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (gameMap[i, j] !=0)
                    {
                        drawCube(i, j, playersColors[gameMap[i, j]-1]);
                    }
                }
            }
        }
        //to draw a cube in the gameMap[i,j]
        void drawCube(int i,int j,Color c)
        {
            double xt=((j*len)/(windowWidth/2)) - 1, yt=-1*(((i*len) / (windowHeight / 2)) - 1);
            viewlen = 2 * len / windowWidth;
            drawSquare(xt, yt, viewlen,c);
        }
        //to draw a square in the xtop,ytop coordinates and with length=len
        void drawSquare(double xtop, double ytop, double len,Color c)
        {
            //here we suppose that the given length is the horizontal length
            double verticalLen = len * m / n;
            GL.Begin(BeginMode.Quads);
            GL.Color3(c);
            GL.Vertex3(xtop, ytop, 0);
            GL.Color3(c);
            GL.Vertex3(xtop+len, ytop, 0);
            GL.Color3(c);
            GL.Vertex3(xtop+len, ytop-verticalLen, 0);
            GL.Color3(c);
            GL.Vertex3(xtop, ytop- verticalLen, 0);
            GL.End();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            drawMap();
            drawCube(currentI, currentJ, playersColors[currentPlayer]);
            SwapBuffers();
        }
        static void Main(string[] args)
        {
            FallingBlockForm f = new FallingBlockForm();
            Application.Run(f);
        }
    }
}
