using Minesweeper.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
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

        public Stopwatch Stopwatch { get; set; }



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



        public List<Field> GetNeighborFields(int x, int y)
        {
            var currentField = Fields.Where(field => field.X == x && field.Y == y);

            return GetFieldWithNeighbors(x, y).Except(currentField).ToList();
        }



        public List<Field> GetFieldWithNeighbors(int x, int y)
        {
            var nearbyFields = Fields.Where(field =>
                field.X >= (x - 1)
                && field.X <= (x + 1)
                && field.Y >= (y - 1)
                && field.Y <= (y + 1));
            return nearbyFields.ToList();
        }



        public void FirstClick(int x, int y)
        {
            Random random = new Random();

            var clickedFieldWithNeighbors = GetFieldWithNeighbors(x, y);

            var availableFieldsForMines = Fields
                .Except(clickedFieldWithNeighbors)
                .OrderBy(param => random.Next());

            var fieldsWithPlacedMines = availableFieldsForMines
                .Take(MineCount)
                .ToList()
                .Select(p => new { p.X, p.Y });

            foreach (var mine in fieldsWithPlacedMines)
            {
                Fields.Single(field =>
                    field.X == mine.X
                    && field.Y == mine.Y).IsMine = true;
            }

            foreach (var safeField in Fields.Where(field => !field.IsMine))
            {
                var nearbyFields = GetNeighborFields(safeField.X, safeField.Y);
                safeField.AdjacentMines = nearbyFields.Count(param => param.IsMine);
            }

            GameState = GameState.InProgress;

            Stopwatch.StartNew();
        }



        public void Click(int x, int y)
        {
            if (GameState == GameState.NewGameStarted)
            {
                FirstClick(x, y);
            }
            RevealField(x, y);
        }



        public void RevealField(int x, int y)
        {
            var selectedField = Fields.First(field => field.X == x && field.Y == y);

            selectedField.Reveal();

            if (selectedField.IsMine)
            {
                GameState = GameState.Lost;
                RevealAllMines();
                return;
            }

            if (selectedField.AdjacentMines == 0)
            {
                RevealZeroes(x, y);
            }

            CompletionCheck();
        }



        private void RevealAllMines()
        {
            Fields
                .Where(field => field.IsMine)
                .ToList()
                .ForEach(field => field.FieldState = FieldState.Revealed);
        }



        public void RevealZeroes(int x, int y)
        {
            var neighborFields = GetNeighborFields(x, y).Where(field => !field.IsRevealed);

            foreach (var neighborField in neighborFields)
            {
                neighborField.FieldState = FieldState.Revealed;

                if (neighborField.AdjacentMines == 0)
                {
                    RevealZeroes(neighborField.X, neighborField.Y);
                }
            }
        }



        private void CompletionCheck()
        {
            var coveredFields = Fields.Where(field => !field.IsRevealed).Select(field => field.Id);

            var mineFields = Fields.Where(field => field.IsMine).Select(field => field.Id);

            if (!coveredFields.Except(mineFields).Any())
            {
                GameState = GameState.Won;
                Stopwatch.Stop();
            }
        }



        public void FlagField(int x, int y)
        {
            var flaggedField = Fields.Where(field => field.X == x && field.Y == y).First();

            flaggedField.Flag();
        }
    }
}
