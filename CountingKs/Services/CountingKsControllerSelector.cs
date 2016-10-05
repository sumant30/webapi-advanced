using System;
using System . Collections . Generic;
using System . Linq;
using System . Net . Http;
using System . Web;
using System . Web . Http;
using System . Web . Http . Controllers;
using System . Web . Http . Dispatcher;

namespace CountingKs . Services
	{
	public class CountingKsControllerSelector : DefaultHttpControllerSelector
		{
		HttpConfiguration _config;
		public CountingKsControllerSelector ( HttpConfiguration config )
			: base ( config )
			{
			_config = config;
			}

		public override HttpControllerDescriptor SelectController ( HttpRequestMessage request )
			{
			//Get a dictionary of all controllers
			var controllers = GetControllerMapping ( );

			//Get route data - this will give us all the dat regarding the information passed in url
			var routeData = request . GetRouteData ( );

			//Get controller names
			var controllerName = ( string ) routeData . Values[ "controller" ];

			HttpControllerDescriptor descriptor;

			//Find the controller name
			if ( controllers . TryGetValue ( controllerName , out descriptor ) )
				{

				var version="2";

				var newName = string . Concat ( controllerName , "v" , version );

				HttpControllerDescriptor versionedDescriptor;
				if ( controllers . TryGetValue ( controllerName , out versionedDescriptor ) )
				{
				return versionedDescriptor;
			    }

				//Default implementation
				return descriptor;
				}
			return null;
			}
		}
	}