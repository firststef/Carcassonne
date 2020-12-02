using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using GameComponents;
using GameStructures;

					
public class Program {


  public static T ParseEnum<T>(string value) {
    return (T) Enum.Parse(typeof(T), value, true);
  }


  public static void run() {
    System.Console.WriteLine("da");
    var componentManager = new ComponentManager();
    var structureManager = new StructureManager();
    var tileComps = componentManager.ParseJson();
    foreach (var tileComp in tileComps) {
      // System.Console.WriteLine(tileComp.ToString());
    }
    // // // testing rotation
    // var tile1 = new GameStructures.Tile(tileComps[0]);
    // var str = "";
    // str = tile1.TileComponent.PrintMatrix();
    // System.Console.WriteLine(str);
    // tile1.RotateTile(1);
    // str = tile1.TileComponent.PrintMatrix();
    // System.Console.WriteLine(str);
    // var tile2 = new GameStructures.Tile(tileComps[0]);
    // tile2.RotateTile(4);
    // str = tile2.TileComponent.PrintMatrix();
    // System.Console.WriteLine(str);

    // // // testing neighbors
    // var tile3 = new GameStructures.Tile(tileComps[0]);
    // var tile4 = new GameStructures.Tile(tileComps[0]);
    // tile4.RotateTile(1);
    // var x = tile4.CanBeNeighbors(tile3, 3);
    // System.Console.WriteLine(x);
    // x= tile3.CanBeNeighbors(tile3, 1);
    // System.Console.WriteLine(x);

    // // // testing meeple creation
    // Meeple m1 = new Meeple(MeepleColor.Red);
    // Meeple m2 = new Meeple(MeepleColor.Yellow);
    // System.Console.WriteLine(m1.ToString());
    // System.Console.WriteLine(m2.ToString());

    // // testing structure creation and joining
    GameStructure g1 = new City();
    GameStructure g2 = new City();
    var g3 = new City();
    System.Console.WriteLine(g3.GetType()); // is City
    var tile5 = new Tile(tileComps[0]);
    var tile6 = new Tile(tileComps[0]);

    g1.ComponentTiles.Add(tile5);
    g3.ComponentTiles.Add(tile6);
    g3.MeepleList.Add(new Meeple(MeepleColor.Blue));

    System.Console.WriteLine(g1.PrintTileMatrices());
    System.Console.WriteLine(g3.PrintTileMatrices());

    g3.joinStructures(g1);
    
    System.Console.WriteLine(g3.PrintTileMatrices());
    System.Console.WriteLine(g3.ToString());
  }
  
	
	public static void Main() {
    Program.run();
	}
}