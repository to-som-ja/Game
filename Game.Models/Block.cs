using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Models
{
	public class Block
	{
		public int positionX { get; set; }
		public int positionY { get; set; }
		public bool player { get; set; }
		public Type Type { get; set; }
		public string ImagePath { get; set; }
	}
}
