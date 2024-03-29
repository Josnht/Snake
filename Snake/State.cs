﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class State
    {
        public int Rows { get; }
        public int Cols { get; }
        public Map[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int score { get; private set; }
        public bool gameover { get; private set; }
        private readonly LinkedList<Direction> dirChange = new LinkedList<Direction>();
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();
        private readonly Random random = new Random();

        public State(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new Map[rows, cols];
            Dir = Direction.Right;

            AddSnake();
            Addfood();
        }

        private void AddSnake()
            {
            int r = Rows / 2;

            for (int c=1; c<=3; c++)
            {
                Grid[r, c] = Snake.Map.Snake;
                snakePositions.AddFirst(new Position(r, c));
            }

            }
        private IEnumerable<Position> EmptyPosition()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c=0; c< Cols; c++)
                {
                    if (Grid[r,c] == Snake.Map.Empty)
                    {
                        yield return new Position(r, c);
                    }
                }
            }
        }
        private void Addfood()
        {
            List<Position> empty = new List<Position>(EmptyPosition());
            if (empty.Count == 0)
            {
                return;
            }
            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = Snake.Map.Food;
        }
        public Position HeadPosition()
        {
            return snakePositions.First.Value;
        }
        public Position TailPosition()
        {
            return snakePositions.Last.Value;
        }
        public IEnumerable<Position> SnakePosition()
        {
            return snakePositions;
        }
        public void AddHead(Position pos)
        {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = Snake.Map.Snake;
        }
        public void RemoveTail()
        {
            Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = Snake.Map.Empty;
            snakePositions.RemoveLast();
        }
        private Direction GetLastDirection()
        {
            if (dirChange.Count == 0)
            {
                return Dir;
            }return dirChange.Last.Value;
        }
        private bool CanChangeDir(Direction newDir)
        {
            if (dirChange.Count == 2)
            {
                return false;
            }
            Direction lastDir = GetLastDirection();
            return newDir != lastDir && newDir != lastDir.Oposite(); 
        }
        public void ChangeDirection (Direction dir)
        {
            if (CanChangeDir(dir))
            {
                dirChange.AddLast(dir);
            }
        }
        private bool OutsideMap(Position pos)
        {
            return pos.Row <0 || pos.Row >=Rows || pos.Col <0|| pos.Col >=Cols;
        }
        private Map Willhit (Position headpos)
        {
            if (OutsideMap(headpos))
            {
                return Snake.Map.Outside;
            }
            if (headpos == TailPosition())
            {
                return Snake.Map.Empty;
            }
            return Grid[headpos.Row, headpos.Col];
        }
        public void Move ()
        {
            if (dirChange.Count >0)
            {
                Dir = dirChange.First.Value;
                dirChange.RemoveFirst();
            }
            Position headpos = HeadPosition().Translate(Dir);
            Map hit = Willhit(headpos);
            if (hit == Snake.Map.Outside || hit == Snake.Map.Snake)
            {
                gameover = true;
            }else if (hit == Snake.Map.Empty)
            {
                RemoveTail();
                AddHead(headpos);
            }
            else if (hit == Snake.Map.Food)
            {
                AddHead(headpos);
                score++;
                Addfood();
            }
        }
    }
}
