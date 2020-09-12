using System;
using System.Collections.Generic;

namespace LicentaPuzzlePirates.Games.GameResources
{
    public enum DirectionEnum
    {
        Up,
        Left,
        Down,
        Right
    }


    public class PatchTile
    {
        public DirectionEnum[] directions;


        public PatchTile(params DirectionEnum[] directions)
        {
            this.directions = directions;
        }


        public void UpdateDirections()
        {
            for (int i = 0; i < directions.Length; i++)
            {
                directions[i] = (DirectionEnum)(((int)(directions[i]) + 1) % Enum.GetValues(typeof(DirectionEnum)).Length);
            }
        }


        public static PatchTile CreateRandomSewShape()
        {
            List<DirectionEnum> directions = new List<DirectionEnum>();
            for (int i = 0; i < Enum.GetValues(typeof(DirectionEnum)).Length; i++)
            {
                directions.Add((DirectionEnum)Enum.GetValues(typeof(DirectionEnum)).GetValue(i));
            }

            int toRemove = Enum.GetValues(typeof(DirectionEnum)).Length - GamesEngine.random.Next(2, 5);

            for (int i = 0; i < toRemove; i++)
            {
                directions.RemoveAt(GamesEngine.random.Next(directions.Count));
            }

            return new PatchTile(directions.ToArray());
        }
    }
}
