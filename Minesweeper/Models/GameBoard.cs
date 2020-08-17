using Minesweeper.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace Minesweeper.Models
{
    public class GameBoard
    {

        public int Width { get; set; } = 16;
        public int Height { get; set; } = 16;
        public int MineCount { get; set; } = 40;
        
        public List<Field> Fields { get; set; }

        public GameState GameState { get; set; }



        public void Initialize(int width, int heigth, int mineCount)
        {
            Width = width;
            Height = heigth;
            MineCount = mineCount;
            Fields = new List<Field>();

            int id = 1;
            for (var i = 1; i <= heigth; i++)
            {
                for (var j = 1; j <= width; j++)
                {
                    Fields.Add(new Field(id, j, i));
                    id++;
                }
            }
            GameState = GameState.NewGameStarted;
        }



        public void Reset()
        {
            Initialize(Width, Height, MineCount);
        }
    }
}
