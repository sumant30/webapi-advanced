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
	//[RoutePrefix ( "Nutrition/Foods/{foodId:int}/Measures/{id:int?}" )]
	public class MeasuresV2Controller : BaseApiController
		{


		public MeasuresV2Controller ( ICountingKsRepository repo )
			: base ( repo )
			{

			}
		//[Route ( "" )]
		public IEnumerable<MeasureV2Model> Get ( int foodId )
			{
			var results = TheRepository . GetFood ( foodId ) . Measures . ToList ( ) . Select ( TheModelFactory . CreateV2 );
			return results;
			}

		//[Route ( "" )]
		public IHttpActionResult Get ( int foodid , int id )
			{
			var results = TheRepository . GetMeasure ( id );

			if ( results . Food . Id == foodid )
				{
				return Ok ( TheModelFactory . CreateV2 ( results ) );
				}
			return NotFound ( );
			}
		}
	}
