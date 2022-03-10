using Game.Models;
using Accord.Math;
using AVXPerlinNoise;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Type = Game.Models.Type;
using System.Threading;

namespace Game.Web.Pages
{
    public class MapBase : ComponentBase, INotifyPropertyChanged
    {
        public List<Block> mapGrid = new List<Block>();
        public Player player;
        protected int mapWidth = 3000;
        protected int mapHeight = 2000;
        protected int renderWidth = 30;//30
        protected int renderHeight = 20;//20
        protected int renderWidthStart;
        protected int renderWidthEnd;
        protected int renderHeightStart;
        protected int renderHeightEnd;
        private Random rnd;
        private double seed;
        public int top;
        public int left;
        public PerlinNoise noise = new PerlinNoise();
        public float zoom = 9.0f;
        public int xOffset; //<- +>
        public int yOffset; //v- +^
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnInitialized()
        {
            rnd = new Random();
            player = new Player(mapWidth/2, mapHeight/2, "images/player.png");
            player.relativePositionX = renderWidth / 2;
            player.relativePositionY = renderHeight / 2;
            seed = rnd.NextDouble();
            xOffset = rnd.Next(1,10000)-10000;
            yOffset = rnd.Next(1,10000)-10000;
            top = player.relativePositionY * 40+3;
            left = player.relativePositionX * 40+3;
            renderHeightStart = player.positionY - renderHeight / 2;
            renderWidthStart = player.positionX - renderWidth / 2;
            renderHeightEnd = player.positionY + renderHeight / 2;
            renderWidthEnd = player.positionX + renderWidth / 2;
            generateMap();
            PropertyChanged += (o, e) => StateHasChanged();
            StateHasChanged();

        }
        public int mapFunction(int positonX, int positionY)
        {
            return positionY * mapWidth + positonX;
        }
        private bool canMove(int dir)
        {
            if (dir == 0 && player.positionY - 1 >= 0 && mapGrid[mapFunction(player.positionX, player.positionY - 1)].Type == Type.Grass) return true;
            if (dir == 1 && player.positionX + 1 < mapWidth && mapGrid[mapFunction(player.positionX + 1, player.positionY)].Type == Type.Grass) return true;
            if (dir == 2 && player.positionY + 1 < mapHeight && mapGrid[mapFunction(player.positionX, player.positionY + 1)].Type == Type.Grass) return true;
            if (dir == 3 && player.positionX - 1 >= 0 && mapGrid[mapFunction(player.positionX - 1, player.positionY)].Type == Type.Grass) return true;
            return false;
        }
        public async Task moveTo(int dir)
        {
            int border = 3;
            bool moveMap = false;
            if (canMove(dir))
            {
                switch (dir)
                {
                    case 0:
                        player.positionY--;
                        if (player.relativePositionY-1<border)
                        {
                            moveMap = true;
                            renderHeightStart--;
                            renderHeightEnd--;
                            await Task.Delay(100);
                        }
                        else
                        {
                            player.relativePositionY--;
                        }
                        break;
                    case 1:
                        player.positionX++;
                        if (player.relativePositionX + 1 >= renderWidth - border)
                        {
                            moveMap = true;
                            renderWidthStart++;
                            renderWidthEnd++;
                            await Task.Delay(100);
                        }
                        else
                        {
                            player.relativePositionX++;
                        }
                        break;
                    case 2:
                        player.positionY++;
                        if (player.relativePositionY + 1 >= renderHeight - border)
                        {
                            moveMap = true;
                            renderHeightStart++;
                            renderHeightEnd++;
                            await Task.Delay(100);
                        }
                        else
                        {
                            player.relativePositionY++;
                        }
                        break;
                    case 3:
                        player.positionX--;
                        if (player.relativePositionX - 1 < border)
                        {
                            moveMap = true;
                            renderWidthStart--;
                            renderWidthEnd--;
                            await Task.Delay(100);

                        }
                        else
                        {
                            player.relativePositionX--;
                        }
                        break;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
                if (!moveMap)
                {
                    for (int i = 0; i < 40; i++)
                    {
                        switch (dir)
                        {
                            case 0:
                                top--;
                                break;
                            case 1:
                                left++;
                                break;
                            case 2:
                                top++;
                                break;
                            case 3:
                                left--;
                                break;
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
                         await Task.Delay(2);
                        //Thread.Sleep(5);
                    }
                }
            }
        }

        public void generateMap()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Type type = Type.Grass;
                    string path = "images/grass.jpg";
                    switch (getPerlinNoise(x, y))
                    {
                        case 0:
                            type = Type.Rock;
                            path = "images/rock.png";
                            break;
                        case 3:
                            type = Type.Water;
                            path = "images/water.png";
                            break;
                        case 1:
                            type = Type.Forest;
                            path = "images/forest.png";
                            break;
                        case 2:
                            type = Type.Grass;
                            path = "images/grass.png";
                            break;
                    }
                    mapGrid.Add(
                            new Block
                            {
                                positionX = x,
                                positionY = y,
                                player = false,
                                Type = type,
                                ImagePath = path
                            });
                }
            }
        }

        public int getPerlinNoise(int x, int y)
        {
            float perlin = (float)Perlin.perlin(
                (float)(x - xOffset) / zoom,
                (float)(y - yOffset) / zoom, seed
                );
            float clampedPerlin = Math.Clamp(perlin, 0.0f, 1.0f);
            float scaledPerlin = clampedPerlin * 4;//pocet typov povrchu
            if (scaledPerlin == 4)
            {
                scaledPerlin = 3;
            }
            if(scaledPerlin > 1.8&& scaledPerlin<2.0f)
            {
                return 2;
            }
            return (int)Math.Floor(scaledPerlin);
        }
    }
}
