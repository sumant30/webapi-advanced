﻿using System;
using System . Collections . Generic;
using System . Linq;
using System . Web;

namespace CountingKs . Models
	{
	public class FoodModel
		{
		public string Description { get; set; }
		public IEnumerable<MeasureModel> Measures  { get; set; }
		public string Url { get; set; }
		}
	}