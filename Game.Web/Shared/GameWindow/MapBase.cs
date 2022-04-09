using Accord.Math;
using AVXPerlinNoise;
using Dapper;
using Game.Models;
using Game.Web.Models;
using Game.Web.Shared.GameWindow.Enemies;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using Type = Game.Models.Type;

namespace Game.Web.Pages
{
    
    public class MapBase : ComponentBase, INotifyPropertyChanged
    {   
        [Inject]
        public IJSRuntime JS {get; set;}
        [Inject]
        public IDataAcces _data { get; set; }
        [Inject]
        public IConfiguration _config { get; set; }
        [Parameter]
        public int seed { get; set; }
        [Parameter]
        public int gameId { get; set; }
        [Parameter]
        public int dificulty { get; set; }

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
        public int top;
        public int left;
        public PerlinNoise noise = new PerlinNoise();
        public float zoom = 9.0f;
        public int xOffset ; //<- +>
        public int yOffset; //v- +^
        public event PropertyChangedEventHandler PropertyChanged;
        public List<IEnemy> enemies = new List<IEnemy>();
        public int spawnRate = 50;


        protected async override void OnInitialized()
        {
            
            if (gameId!=0)
            {
                GameModel game = null;
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default")))
                {
                    string sql = $"select * from Games WHERE Id = (@id);";
                    connection.Open();
                    var rows = connection.Query<GameModel>(sql, new {id = gameId });
                    game= rows.AsList()[0];
                }
                if (game.Seed == 0)
                {
                    Random seedGenerator = new Random();
                    game.Seed = seedGenerator.Next();
                }
                if (game.PositionX == 0)
                {
                    player = new Player(mapWidth / 2, mapHeight / 2, "Images/player.png");
                }
                else
                {
                    player = new Player(game.PositionX, game.PositionY, "Images/player.png");
                    player.level = game.Level;
                    player.experience = game.Experience;
                }
                rnd = new Random(game.Seed);
                seed = game.Seed;
                save();
            }
            else
            {
                if (seed == 0)
                {
                    rnd = new Random();
                }
                else
                {
                    rnd = new Random(seed);
                }
                player = new Player(mapWidth / 2, mapHeight / 2, "Images/player.png");
            }
            xOffset = rnd.Next(1, 10000) - 10000;
            yOffset = rnd.Next(1, 10000) - 10000;
            generateMap();
            while (mapGrid[mapFunction(player.positionX, player.positionY)].Type != Type.Grass)
            {
                player.positionX++;
            }
            PropertyChanged += (o, e) => StateHasChanged();
            StateHasChanged();
        }
        public bool save()
        {
            if (gameId != 0)
            {
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default")))
                {
                    string sql = $"UPDATE Games SET Seed = (@seed_), PositionX = (@positionX), PositionY = (@positionY), Level=(@level) , Experience = (@experience)  WHERE Id = (@id) ";
                    connection.Open();
                    connection.Execute(sql, new { seed_ = seed, positionX = player.positionX, positionY = player.positionY, level = player.level, experience = player.experience, id = gameId });
                }
                return true;
            }
            else
            {
                return false;
            }           
        }
        public async Task  setDimensions()
        {

            renderWidth = (int)(await JS.InvokeAsync<int>("getWidthWindow") - 500) / 40;
            if (renderWidth < 10)
            {
                renderWidth = 10;
            }
            renderHeight = (int)(await JS.InvokeAsync<int>("getHeightWindow")) / 40;
            if (renderHeight < 5)
            {
                renderHeight = 5;
            }
            if (renderWidth % 2 == 0)
            {
                renderWidthEnd = player.positionX + renderWidth / 2;
            }
            else
            {
                renderWidthEnd = player.positionX + renderWidth / 2 + 1;
            }
            if (renderHeight % 2 == 0)
            {
                renderHeightEnd = player.positionY + renderHeight / 2;
            }
            else
            {
                renderHeightEnd = player.positionY + renderHeight / 2 + 1;
            }
            renderHeightStart = player.positionY - renderHeight / 2;
            renderWidthStart = player.positionX - renderWidth / 2;
            left = 3 + 40 * (player.positionX - renderWidthStart);
            top = 3 + 40 * (player.positionY - renderHeightStart);
            renderWidth = renderWidth;
            renderHeight = renderHeight;
            for (int i = renderHeightStart; i < renderHeightEnd; i++)
            {
                if (i != player.positionY)
                    spawnEnemy(false, i);
            }
            base.StateHasChanged();
            chechEnemies();
            base.StateHasChanged();
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
            bool canWalkOnBlock = false;
            bool inRange = false;
            switch (dir)
            {
                case 0:
                    block = mapGrid[mapFunction(player.positionX, player.positionY - 1)];
                    inRange = player.positionY - 1 >= 0;
                    break;
                case 1:
                    block = mapGrid[mapFunction(player.positionX + 1, player.positionY)];
                    inRange = player.positionX + 1 < mapWidth;
                    break;
                case 2:
                    block = mapGrid[mapFunction(player.positionX, player.positionY + 1)];
                    inRange = player.positionY + 1 < mapHeight;
                    break;
                case 3:
                    block = mapGrid[mapFunction(player.positionX - 1, player.positionY)];
                    inRange = player.positionX - 1 >= 0;
                    break;
                default:
                    block = null;
                    break;
            }
            if(block != null)
            canWalkOnBlock = block != null && block.Type == Type.Grass || block.Type == Type.ChoppedTrees;
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
                        if (top - 40 < border * 40 + 3)
                        {
                            moveMap = true;
                            renderHeightStart--;
                            renderHeightEnd--;
                            despawnEnemy(false, renderHeightEnd);
                            spawnEnemy(false, renderHeightStart);
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
                        if (left + 40 >= renderWidth * 40 - border * 40)
                        {
                            moveMap = true;
                            despawnEnemy(true, renderWidthStart);
                            spawnEnemy(true, renderWidthEnd);
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
                        if (top + 40 >= renderHeight * 40 - border * 40)
                        {
                            moveMap = true;
                            despawnEnemy(false, renderHeightStart);
                            spawnEnemy(false, renderHeightEnd);
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
                        if (left - 40 < border * 40 + 3)
                        {
                            moveMap = true;
                            renderWidthStart--;
                            renderWidthEnd--;
                            despawnEnemy(true, renderWidthEnd);
                            spawnEnemy(true, renderWidthStart);
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

        public void spawnEnemy(bool row, int position)
        {


            if (rnd.Next(0, 100) < spawnRate)
            {
                int positionY = 0;
                int positionX = 0;
                Block block;
                List<Block> candidatesForSpawn = new List<Block>();
                if (row)
                {
                    for (int i = renderHeightStart; i < renderHeightEnd; i++)
                    {
                        block = mapGrid[mapFunction(position, i)];
                        if (block.Type == Type.Grass || block.Type == Type.ChoppedTrees)
                        {
                            candidatesForSpawn.Add(block);
                        }
                    }

                    positionX = position;
                    if (candidatesForSpawn.Count != 0)
                        positionY = candidatesForSpawn[rnd.Next(0, candidatesForSpawn.Count)].positionY;
                }
                else
                {
                    for (int i = renderWidthStart; i < renderWidthEnd; i++)
                    {
                        block = mapGrid[mapFunction(i, position)];
                        if (block.Type == Type.Grass || block.Type == Type.ChoppedTrees)
                        {
                            candidatesForSpawn.Add(block);
                        }
                    }
                    positionY = position;
                    if (candidatesForSpawn.Count != 0)
                        positionX = candidatesForSpawn[rnd.Next(0, candidatesForSpawn.Count)].positionX;
                }
                int level = (int)Math.Sqrt(Math.Abs(mapWidth / 2 - positionX) + Math.Abs(mapHeight / 2 - positionY));
                EnemyParent enemy = null;
                switch (rnd.Next(0, 5))
                //switch(0)
                {
                    case 0:
                        enemy = new EnemyVeigar(positionX, positionY, level);
                        break;
                    case 1:
                        enemy = new EnemyMorde(positionX, positionY, level);
                        break;
                    case 2:
                        enemy = new EnemyFiddle(positionX, positionY, level);
                        break;
                    case 3:
                        enemy = new EnemyBrand(positionX, positionY, level);
                        break;
                    case 4:
                        enemy = new EnemyChogath(positionX, positionY, level);
                        break;
                }
                if (candidatesForSpawn.Count != 0)
                {
                    enemies.Add((IEnemy)enemy);
                }
            }
        }
        public void despawnEnemy(bool row, int position)
        {
            Stack<int> removeEnemies = new Stack<int>();
            if (row)
            {//stlpec y
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (((EnemyParent)enemies[i]).positionX == position)
                    {
                        removeEnemies.Push(i);
                    }
                }
            }
            else
            {//riadok x
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (((EnemyParent)enemies[i]).positionY == position)
                    {
                        removeEnemies.Push(i);
                    }
                }
            }

            foreach (int index in removeEnemies)
            {
                enemies.RemoveAt(index);
            }
        }
        public void chechEnemies()
        {
            Stack<int> removeEnemies = new Stack<int>();
            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyParent enemy = (EnemyParent)enemies[i];
                if (enemy.positionY >= renderHeightEnd || enemy.positionX >= renderWidthEnd || enemy.positionX < renderWidthStart || enemy.positionY < renderHeightStart)
                {
                    removeEnemies.Push(i);
                }
            }
            foreach (int index in removeEnemies)
            {
                enemies.RemoveAt(index);
            }
        }
    }
}
