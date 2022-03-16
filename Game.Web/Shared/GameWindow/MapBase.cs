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
        public int renderWidth = 30;//30
        public int renderHeight = 20;//20
        public int renderWidthStart;
        public int renderWidthEnd;
        public int renderHeightStart;
        public int renderHeightEnd;
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
            rnd = new Random(2);
            player = new Player(mapWidth / 2, mapHeight / 2, "images/player.png");
            player.relativePositionX = renderWidth / 2;
            player.relativePositionY = renderHeight / 2;
            seed = rnd.NextDouble();
            xOffset = rnd.Next(1, 10000) - 10000;
            yOffset = rnd.Next(1, 10000) - 10000;
            top = player.relativePositionY * 40 + 3;
            left = player.relativePositionX * 40 + 3;
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
        public void refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
        public bool canMove(int dir)
        {
            Block block;
            bool canWalkOnBlock= false;
            bool inRange=false;
            switch (dir)
            {
                case 0:
                    block = mapGrid[mapFunction(player.positionX, player.positionY - 1)];
                    inRange = player.positionY - 1 >= 0;
                    break;
                case 1:
                    block = mapGrid[mapFunction(player.positionX+1, player.positionY)];
                    inRange = player.positionX + 1 < mapWidth;
                    break;
                case 2:
                    block = mapGrid[mapFunction(player.positionX, player.positionY + 1)];
                    inRange = player.positionY + 1 < mapHeight;
                    break;
                case 3:
                    block = mapGrid[mapFunction(player.positionX-1, player.positionY)];
                    inRange = player.positionX - 1 >= 0;
                    break;
                    default:
                    block = null;
                    break;
            }
            canWalkOnBlock =block!=null && block.Type == Type.Grass || block.Type == Type.ChoppedTrees;
          return inRange && canWalkOnBlock;
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
                        player.direction = Direction.North;
                        player.positionY--;
                        if (top - 40 < border*40+3)
                        {
                            moveMap = true;
                            renderHeightStart--;
                            renderHeightEnd--;
                            await Task.Delay(200);
                        }
                        else
                        {
                            player.relativePositionY--;
                        }
                        break;
                    case 1:
                        player.positionX++;
                        player.direction = Direction.East;
                        if (left + 40 >= renderWidth*40-border*40)
                        {
                            moveMap = true;
                            renderWidthStart++;
                            renderWidthEnd++;
                            await Task.Delay(200);
                        }
                        else
                        {
                            player.relativePositionX++;
                        }
                        break;
                    case 2:
                        player.positionY++;
                        player.direction = Direction.South;
                        if (top + 40 >= renderHeight*40 - border*40)
                        {
                            moveMap = true;
                            renderHeightStart++;
                            renderHeightEnd++;
                            await Task.Delay(200);
                        }
                        else
                        {
                            player.relativePositionY++;
                        }
                        break;
                    case 3:
                        player.positionX--;
                        player.direction = Direction.West;
                        if (left -40 < border*40+3)
                        {
                            moveMap = true;
                            renderWidthStart--;
                            renderWidthEnd--;
                            await Task.Delay(200);

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
            if (scaledPerlin > 1.8 && scaledPerlin < 2.0f)
            {
                return 2;
            }
            return (int)Math.Floor(scaledPerlin);
        }
    }
}
