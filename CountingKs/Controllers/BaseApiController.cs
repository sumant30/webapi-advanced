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
	public class BaseApiController : ApiController
		{
		ICountingKsRepository _repo;

		IModelFactory _modelFactory;

		public BaseApiController ( ICountingKsRepository repo )
			{
			_repo = repo;

			}
		public ICountingKsRepository TheRepository
			{
			get
				{
				return _repo;
				}

			}
		public IModelFactory TheModelFactory
			{
			get
				{
				if ( _modelFactory == null )
					{
					_modelFactory = new ModelFactory ( this . Request,TheRepository );
					}
				return _modelFactory;

				}
			}
		}
	}
