using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameComponents;
using LibCarcassonne.ArrayAccessMethods;

namespace LibCarcassonne
{
    namespace GameStructures
    {


        public enum MeepleColor
        {
            Red = 0,
            Blue = 1,
            Green = 2,
            Black = 3,
            Yellow = 4
        }


        public class Meeple
        {
            public MeepleColor MeepleColor { get; set; }
            public static int MeepleMaxId { get; set; }
            public int MeepleId { get; set; }
            public Tile MeeplePlacement { get; set; }
            public int PlacementId { get; set; }
            public int MeeplePoints { get; set; }
            public Player Owner { get; set; }


            public Meeple(MeepleColor meepleColor, Player owner)
            {
                this.MeepleId = Meeple.MeepleMaxId++;
                this.MeepleColor = meepleColor;
                this.MeeplePlacement = null;
                this.PlacementId = -1;
                this.MeeplePoints = 0;
                this.Owner = owner;
            }


            public void PlaceMeeple(Tile meeplePlacement, int placementId)
            {
                this.MeeplePlacement = meeplePlacement;
                this.PlacementId = placementId;
                this.MeeplePoints = 0;
            }

            
            /**
             * raises meeple from ground and returns meeple points
             */
            public void RaiseMeeple()
            {
                this.PlacementId = -1;
                this.MeeplePlacement = null;
                this.Owner.PlayerPoints += this.MeeplePoints;
                this.MeeplePoints = 0;
            }


            /**
             * returns False if meeple is placed (placement Id is not -1), True otherwise
             */
            public bool IsMeepleFree()
            {
                return (-1 == this.PlacementId);
            }


            public override string ToString()
            {
                return $"{{\nMeeple: meeple_{this.MeepleId}\nMeepleColor: {this.MeepleColor.ToString()}\nMeeplePlacementId: {this.PlacementId}\n}}";
            }


        }
    }
}