using CountingKs . Data;
using CountingKs . Services;
using System;
using System . Collections . Generic;
using System . Linq;
using System . Net;
using System . Net . Http;
using System . Web . Http;

namespace CountingKs . Controllers
	{
	[RoutePrefix ( "User/Diaries/{diaryId:DateTime}/Summary" )]
	public class DiarySummaryController : BaseApiController
		{
		ICountingKsIdentityService _identityService;

		public DiarySummaryController ( ICountingKsRepository repo , ICountingKsIdentityService identityService )
			: base ( repo )
			{
			_identityService = identityService;
			}
		[Route("")]
		public IHttpActionResult Get ( DateTime diaryId ) 
			{
			try
				{
				var diary = TheRepository . GetDiary ( _identityService . CurrentUser , diaryId );
				if ( diary != null )
					{
					return Ok(TheModelFactory . CreateSummary ( diary ));
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
