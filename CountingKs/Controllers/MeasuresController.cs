using CountingKs . Data;
using CountingKs . Models;
using System;
using System . Collections . Generic;
using System . Linq;
using System . Net;
using System . Net . Http;
using System . Web . Http;

namespace CountingKs . Controllers
	{
	[RoutePrefix ( "Nutrition/Foods/{foodId:int}/Measures/{id:int?}" )]
	public class MeasuresController : BaseApiController
		{


		public MeasuresController ( ICountingKsRepository repo )
			: base ( repo )
			{

			}
		[Route ( "" , Name = "Measures" )]
		public IEnumerable<MeasureModel> Get ( int foodId )
			{
			var results = TheRepository . GetFood ( foodId ) . Measures . ToList ( ) . Select ( TheModelFactory . Create );
			return results;
			}

		[Route ( "" )]
		public IHttpActionResult Get ( int foodid , int id )
			{
			var results = TheRepository . GetMeasure ( id );

			if ( results . Food . Id == foodid )
				{
				return Ok(TheModelFactory . Create ( results ));
				}
			return NotFound ( );
			}
		}
	}
