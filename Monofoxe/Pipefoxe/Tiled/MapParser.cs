﻿using System;
using System.Xml;
using Monofoxe.Tiled.MapStructure;

namespace Pipefoxe.Tiled
{
	static class MapParser
	{
		public static TiledMap Parse(XmlDocument xml)
		{
			Logger.Init("map.log");	
			Logger.Log("foxes");

			var map = new TiledMap();
			var mapXml = xml["map"];
			var mapAttributes = xml["map"].Attributes;

			if (mapAttributes["infinite"].Value == "1")
			{
				throw new Exception("Infinite maps are not supported yet!");
			}

			// Properties.
			map.Width = int.Parse(mapAttributes["width"].Value);
			map.Height = int.Parse(mapAttributes["height"].Value);
			map.TileWidth = int.Parse(mapAttributes["tilewidth"].Value);
			map.TileHeight = int.Parse(mapAttributes["tileheight"].Value);
			
			Enum.TryParse(mapAttributes["renderorder"].Value.Replace("-", ""), true, out map.RenderOrder);
			Enum.TryParse(mapAttributes["orientation"].Value, true, out map.Orientation);

			if (mapAttributes["staggeraxis"] != null)
			{
				Enum.TryParse(mapAttributes["staggeraxis"].Value, true, out map.StaggerAxis);
			}
			if (mapAttributes["staggerindex"] != null)
			{
				Enum.TryParse(mapAttributes["staggerindex"].Value, true, out map.StaggerIndex);
			}

			map.HexSideLength = XmlHelper.GetXmlIntSafe(mapXml, "hexsidelength");
			// Properties.
			

			// Tilesets and layers.
			map.Tilesets = TilesetParser.Parse(mapXml.SelectNodes("tileset"));
			LayerParser.Parse(mapXml, map);
			// Tilesets and layers.

			return map;
		}
	}
}
