using CountingKs . Data;
using CountingKs . Filters;
using CountingKs . Models;
using CountingKs . Services;
using System;
using System . Collections . Generic;
using System . Linq;
using System . Web . Http;

namespace CountingKs.Controllers
{

	[RoutePrefix("User/Diaries")]
	//[CountingKsAuthorization]
	public class DiariesController : BaseApiController
    {
		ICountingKsIdentityService _identityService;
		public DiariesController ( ICountingKsRepository repo,ICountingKsIdentityService identityService)
			: base ( repo )
			{
			_identityService = identityService;
			}

		[Route ("")]
		public IEnumerable<DiaryModel> Get ( )
			{
				 var user = _identityService.CurrentUser;
				 var results = TheRepository . GetDiaries ( user )
											. OrderByDescending ( x => x . CurrentDate )
											. Take ( 10 )
											. ToList ( )
											. Select ( TheModelFactory . Create );
				 return results;
			}

		[Route ( "{diaryId:DateTime}" , Name = "Diary" )]
		public IHttpActionResult Get ( DateTime diaryId )
			{
			var user = _identityService . CurrentUser;
			var result = TheRepository . GetDiary ( user , diaryId );
			if ( result == null ) 
				{
				return NotFound ( );
				}
			return Ok ( TheModelFactory . Create ( result ) );
			}
    }
}
