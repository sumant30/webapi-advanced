using CountingKs . Data;
using CountingKs . Data . Entities;
using CountingKs . Models;
using System;
using System . Collections . Generic;
using System . Linq;
using System . Net;
using System . Net . Http;
using System . Text;
using System . Web . Http;

namespace CountingKs . Controllers
	{
	public class TokenController : BaseApiController
		{
		public TokenController ( ICountingKsRepository repo )
			: base ( repo )
			{

			}
		public IHttpActionResult Post ( [FromBody] TokenRequestModel model )
			{
			try
				{
				//Get the user
				var user = TheRepository . GetApiUsers ( ) . Where ( x => x . AppId == model . ApiKey ) . FirstOrDefault ( );
				if ( user != null )
					{
					var secret = user . Secret;
					//Simplistic version which cannot be used in real scenarios
					var key = Convert . FromBase64String ( secret );
					var provider = new System . Security . Cryptography . HMACSHA256 ( key );
					//Provide hash from API Key(Not secure)
					var hash = provider . ComputeHash ( Encoding . UTF8 . GetBytes ( user . AppId ) );
					var signature = Convert . ToBase64String ( hash );
					if ( signature == model . Signature )
						{
						var rawToken = string . Concat ( user . AppId + DateTime . UtcNow . ToString ( "d" ) );
						var rawTokenBytes =   Encoding . UTF8 . GetBytes ( rawToken );
						var token = provider . ComputeHash ( rawTokenBytes );
						var authtoken = new AuthToken ( )
						{
							Token = Convert . ToBase64String ( token ) ,
							Expiration = DateTime . UtcNow . AddDays ( 7 ) ,
							ApiUser = user
						};
						TheRepository . Insert ( authtoken );
						TheRepository . SaveAll ( );
						return Created ( "" , TheModelFactory . Create ( authtoken ) );
						}
					}
				}
			catch ( Exception )
				{
				return BadRequest ( );
				}
			return BadRequest ( );
			}

		}
	}
