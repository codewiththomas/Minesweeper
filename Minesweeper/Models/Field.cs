using Minesweeper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minesweeper.Models
{
    public class Field
    {
        public int Id { get; set; }
        public int X { get; set; }

        public int Y { get; set; }

        public bool IsMine { get; set; }

        //angrenzende Minen
        public int AdjacentMines { get; set; }

        public FieldState FieldState { get; set; }

        public bool IsRevealed { 
            get 
            {
                if (FieldState == FieldState.Revealed)
                    return true;
                else
                    return false;
            } 
        }

        public bool IsFlagged { get; }

        public Field(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }


        public void Flag()
        {
            if (FieldState == FieldState.Covered)
                FieldState = FieldState.Flagged;
        }


        public void Reveal()
        {
            FieldState = FieldState.Revealed;
        }
    }
}
