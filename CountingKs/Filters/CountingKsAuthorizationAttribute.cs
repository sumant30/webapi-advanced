using System;
using System . Collections . Generic;
using System . Linq;
using System . Net;
using System . Web;
using System . Web . Http . Controllers;
using System . Web . Http . Filters;
using System . Net . Http;
using System . Text;
using WebMatrix . WebData;
using System . Security . Principal;
using System . Threading;

namespace CountingKs . Filters
	{
	public class CountingKsAuthorizationAttribute : AuthorizationFilterAttribute
		{
		public override void OnAuthorization ( HttpActionContext actionContext )
			{
			if ( Thread . CurrentPrincipal . Identity . IsAuthenticated )
				{
				return;
				}

			//We handle basic authentication based on header which is present with request
			var   authHeader = actionContext . Request . Headers . Authorization;
			if ( authHeader != null )
				{
				//Check if the header has scheme equals basic && paramater is not null or empty 
				if ( authHeader . Scheme . Equals ( "basic" , StringComparison . OrdinalIgnoreCase ) && !string . IsNullOrWhiteSpace ( authHeader . Parameter ) )
					{
					//Pull credential from paramter
					//Raw string of credentails  in base 64 format
					var rawCredentials = authHeader . Parameter;

					//As username might be foreign characters and password may contain symbols
					var encoding = 	Encoding . GetEncoding ( "iso-8859-1" );

					var credentials = encoding . GetString ( Convert . FromBase64String ( rawCredentials ) );
					var split = credentials . Split ( ':' );
					var username = split[ 0 ];
					var password = split[ 1 ];

					//Intialize web security
					if ( !WebSecurity . Initialized )
						{
						WebSecurity . InitializeDatabaseConnection ( "DefaultConnection" , "UserProfile" , "UserId" , "UserName" , autoCreateTables: true );
						}

					if ( WebSecurity . Login ( username , password ) )
						{
						//As we are using username of the current user we are going to create principla here
						var principal = new GenericPrincipal ( new GenericIdentity ( username ) , null );
						//Set the principal
						Thread . CurrentPrincipal = principal;
						return;
						}
					}
				}
			HandleUnauthorized ( actionContext );
			}

		void HandleUnauthorized ( HttpActionContext actionContext )
			{
			actionContext . Response = actionContext . Request . CreateResponse ( HttpStatusCode . Unauthorized );
			actionContext . Response . Headers . Add ( "WWW-Authenticate" , "Basic Scheme='CountingKs' location=''" );
			}
		}
	}