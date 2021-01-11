using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LibCarcassonne.GameComponents;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameLogic;
using LibCarcassonne.ArrayAccessMethods;

namespace LibCarcassonne
{
    namespace GameLogic
    {


        public class GameRunner
        {
            public GameBoard GameBoard { get; set; }
            public List<Tile> UnplayedTiles { get; set; }
            public PlayerManager PlayerManager { get; set;  }
            public AI AI { get; set; }


            public GameRunner(GameRunner another)
            {
                this.GameBoard = another.GameBoard.Clone();
                this.AI = another.AI;
                this.PlayerManager = another.PlayerManager;
                this. UnplayedTiles = another.UnplayedTiles.Select(item => (Tile)item.Clone()).ToList();
            }

            public GameRunner(List<TileComponent> tileComponents, int numberOfPlayers, int difficutly = 3)
            {
                var structureManager = new StructureManager(); // initializing structure manager to set min id's for structures to 10
                this.PlayerManager = new PlayerManager(numberOfPlayers);

                this.AI = new AI(difficulty: difficutly, gameRunner: this, meepleColor: EnumParse<MeepleColor>.IntToEnum(numberOfPlayers)); 
                // initializes AI player, as the last player with the last color, and speciffied difficulty

                System.Console.WriteLine("GameRunner start");
                this.GameBoard = new GameBoard();
                this.UnplayedTiles = new List<Tile>();

                var tileComponentsArray = tileComponents.ToArray();
                var shuffler = new Utils<TileComponent>();
                var randomWithSeed = new Random(42);
                shuffler.Shuffle(randomWithSeed, tileComponentsArray);
                tileComponents = tileComponentsArray.ToList();

                //tileComponents = tileComponents.OrderBy(i => Guid.NewGuid()).ToList(); // random is not the same for each session
                foreach (var tileComponent in tileComponents)
                {
                    this.UnplayedTiles.Add(new Tile(GameBoard, tileComponent, (-1, -1)));
                }
            }


            /**
            * Tile tile => tile to be placed for which to get possible free position
            * returns a list of possible coordinates for current tile, along with coresponding rotations 
            * or null if tile is unplaceble
            */
            public List<Tuple<(int, int), List<int>>> GetFreePositionsForTile(Tile tile)
            {
                return this.GameBoard.GetFreePositionsForTile(tile);
            }


            /**
            * returns first tile able to be played. removes it from unplayed tiles
            */
            public Tile GetCurrentRoundTile()
            {
                foreach (var tile in this.UnplayedTiles)
                {
                    if (this.GetFreePositionsForTile(tile) != null)
                    {
                        this.UnplayedTiles.Remove(tile);
                        return tile;
                    }
                }

                //TODO: de a fi atent atunci cand se apeleaza functia asta; cand returneaza null => game end
                return null;
            }


            /**
             * distributes points on game end
             */
            public void TriggerEndGame()
            {
                System.Console.WriteLine("\t\tGame has ended\t\t\n");
                foreach(var gameStructure in this.GameBoard.GameStructures)
                {
                    gameStructure.DistributePoints();
                }
            }


            public void AddTileInPosition(Tile tile, (int, int) position)
            {
                this.GameBoard.PlaceTileInPosition(tile, position);
            }


            /**
             * adds tile in position and rotation.
             * returns placed tile
             */
            public List<int> AddTileInPositionAndRotation(Tile tile, (int, int) position, int rotation)
            {
                var clonedTile = tile.Clone();
                clonedTile.RotateTile(rotation);
                this.AddTileInPosition(clonedTile, position);
                return clonedTile.GetPossiblePositionsForMeeple();
            }


            /**
             * places meeple on current tile in tile component type with StructureId = meeplePositionToPlace
             */
            public void PlaceMeeple(Meeple meeple, int meeplePositionToPlace)
            {
                this.GameBoard.PlaceMeeple(this.GetLastPlacedTile(), meeple, meeplePositionToPlace);
            }


            /**
             * method called at the end of each turn; it's purpose is to commit changes made this turn and most importantly, 
             * to check is any new structure is finished, to update finished and unfinished structures, eventually to add points to players and retake meeples
             * 
             * returns the list of meeples to be raised
             */
            public List<Meeple> CommitChanges()
            {
                return this.GameBoard.UpdateUnfinishedStructures();
            }


            /**
             * returns the last placed tile
             */
            public Tile GetLastPlacedTile()
            {
                return this.GameBoard.PlacedTiles[this.GameBoard.PlacedTiles.Count - 1];
            }


            public GameRunner Clone()
            {
                return new GameRunner(this);
            }


            public void SimulatePlay(Tuple<(int, int), int> possibleStateToJumpInto)
            {
                this.GameBoard.FreePositions.Remove(possibleStateToJumpInto.Item1);
                this.GameBoard.TileMatrix[possibleStateToJumpInto.Item1.Item1, possibleStateToJumpInto.Item1.Item2] = new Tile(this.UnplayedTiles[0]);
                this.GameBoard.UpdateFreePositions(possibleStateToJumpInto.Item1);
            }



        }
    }
}