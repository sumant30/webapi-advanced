using CountingKs . Data;
using CountingKs . Data . Entities;
using CountingKs . Models;
using System;
using System . Collections . Generic;
using System . Linq;
using System . Web . Http;
using System . Web . Http . Routing;

namespace CountingKs . Controllers
	{
	[RoutePrefix ( "Nutrition/Foods" )]
	public class FoodsController : BaseApiController
		{

		int PageSize = 50;

		public FoodsController ( ICountingKsRepository repo )
			: base ( repo )
			{

			}

		[Route ( "{includeMeasures:bool?}" , Name = "Nutrition" )]
		public IHttpActionResult Get ( bool includeMeasures = true , int page = 0 )
			{
			IQueryable<Food> query;
			if ( includeMeasures )
				{
				query = TheRepository . GetAllFoodsWithMeasures ( );
				}
			else
				{
				query = TheRepository . GetAllFoods ( );
				}

			var  baseQuery = query . OrderBy ( f => f . Description );
			var totalCount = baseQuery . Count ( );
			var totalPages = Math . Ceiling ( ( double ) totalCount / PageSize );

			var urlHelper = new UrlHelper ( Request );
			var nextPage = page < totalPages-1 ?  urlHelper . Link ( "Nutrition" , new { page = page + 1 } ):string.Empty;
			var previousPage = page > 0 ? urlHelper . Link ( "Nutrition" , new { page = page - 1 } ):string.Empty;

			var results = baseQuery . Skip ( PageSize * page ) . Take ( PageSize ) . ToList ( ) . Select ( TheModelFactory . Create );
			return Ok ( new { TotalCount = totalCount , TotalPages = totalPages , PrevPageUrl = previousPage , NextPageUrl = nextPage , Results = results } );
			}

		[Route ( "{foodId:int}" )]
		public FoodModel Get ( int id )
			{
			return TheModelFactory . Create ( TheRepository . GetFood ( id ) );
			}
		}
	}
