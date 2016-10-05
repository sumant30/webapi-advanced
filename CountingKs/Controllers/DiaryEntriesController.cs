using CountingKs . Data;
using CountingKs . Models;
using CountingKs . Services;
using System;
using System . Collections . Generic;
using System . Linq;
using System . Net;
using System . Net . Http;
using System . Web . Http;

namespace CountingKs . Controllers
	{
	[RoutePrefix ( "User/DiaryEntries/{diaryId:DateTime}/entries/{id:int?}" )]
	public class DiaryEntriesController : BaseApiController
		{
		ICountingKsIdentityService _identityService;

		public DiaryEntriesController ( ICountingKsRepository repo , ICountingKsIdentityService identityService )
			: base ( repo )
			{
			_identityService = identityService;
			}

		[Route ( "" , Name = "DiaryEntry" )]
		public IEnumerable<DiaryEntryModel> Get ( DateTime diaryId )
			{
			var user = _identityService . CurrentUser;
			var results = TheRepository . GetDiaryEntries ( user , diaryId . Date )
										. ToList ( )
										. Select ( TheModelFactory . Create );
			return results;
			}

		[Route ( "" )]
		public IHttpActionResult Get ( DateTime diaryId , int id )
			{
			var user = _identityService . CurrentUser;
			var result = TheRepository . GetDiaryEntry ( user , diaryId . Date , id );
			if ( result == null )
				{
				return NotFound ( );
				}
			return Ok ( TheModelFactory . Create ( result ) );
			}

		[Route ( "" )]
		public IHttpActionResult Post ( DateTime diaryId , [FromBody]DiaryEntryModel model )
			{
			var diaryEntity  = TheModelFactory . Parse ( model );
			if ( diaryEntity == null )
				{
				return BadRequest ( "Could not read entry in body." );
				}
			else
				{
				var diary = TheRepository . GetDiary ( _identityService . CurrentUser , diaryId );
				if ( diary == null )
					{
					return BadRequest ( "Diary could not be found." );
					}
				if ( diary . Entries . Any ( x => x . Measure . Id == diaryEntity . Measure . Id ) )
					{
					return BadRequest ( "Diary entry already exists." );
					}
				else
					{
					diary . Entries . Add ( diaryEntity );
					TheRepository . SaveAll ( );
					return Created ( Request . RequestUri + diary . Id . ToString ( ) , TheModelFactory . Create ( diaryEntity ) );
					}

				}
			}

		[Route ( "" )]
		public IHttpActionResult Delete ( DateTime diaryId , int id )
			{
			try
				{
				if ( TheRepository . GetDiaryEntries ( _identityService . CurrentUser , diaryId ) . Any ( x => x . Id == id ) )
					{
					TheRepository . DeleteDiaryEntry ( id );
					TheRepository . SaveAll ( );
					return Ok ( );
					}
				else
					{
					return NotFound ( );
					}
				}
			catch ( Exception )
				{
				return BadRequest ( );
				}
			}

		[HttpPut]
		[HttpPatch]
		[Route ( "" )]
		public IHttpActionResult UpdateDiaryEntry ( DateTime diaryId , int id , [FromBody]DiaryEntryModel model )
			{
			try
				{
				if ( TheRepository . GetDiaryEntry ( _identityService . CurrentUser , diaryId . Date , id ) != null )
					{
					var diaryEntryEntity = TheModelFactory . Parse ( model );
					var originalEntry = TheRepository . GetDiaryEntry ( _identityService . CurrentUser , diaryId . Date , id );
					if ( diaryEntryEntity . Quantity != originalEntry . Quantity )
						{
						originalEntry . Quantity = model . Quantity;
						TheRepository . SaveAll ( );
						return Ok ( );
						}
					else
						{
						return BadRequest ( );
						}
					}
				else
					{
					return NotFound ( );
					}
				}
			catch ( Exception )
				{
				return BadRequest ( );
				}
			
			}
		}
	}
