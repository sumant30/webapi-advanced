using CountingKs . Data;
using CountingKs . Data . Entities;
using System;
using System . Linq;
using System . Net . Http;
using System . Web . Http . Routing;

namespace CountingKs . Models
	{

	public interface IModelFactory
		{
		FoodModel Create ( Food food );

		MeasureModel Create ( Measure measure );

		DiaryModel Create ( Diary arg );

		DiaryEntryModel Create ( DiaryEntry arg );

		DiaryEntry Parse ( DiaryEntryModel model );

		DiarySummaryModel CreateSummary ( Diary diary );

		AuthTokenModel Create ( AuthToken authtoken );

		MeasureV2Model CreateV2 ( Measure measure );
		}

	public class ModelFactory : IModelFactory
		{
		UrlHelper _urlHepler;

		ICountingKsRepository _repo;

		public ModelFactory ( HttpRequestMessage request , ICountingKsRepository repo )
			{
			_urlHepler = new UrlHelper ( request );
			_repo = repo;
			}

		public FoodModel Create ( Food food )
			{
			return new FoodModel ( )
			{
				Url = _urlHepler . Link ( "Nutrition" , new { foodId = food . Id } ) ,
				Description = food . Description ,
				Measures = food . Measures . Select ( Create )
			};
			}

		public MeasureModel Create ( Measure measure )
			{
			return new MeasureModel ( )
			{
				Url = _urlHepler . Link ( "Measures" , new { foodId = measure . Food . Id , Id = measure . Id } ) ,
				Calories = Math . Round ( measure . Calories ) ,
				Description = measure . Description
			};
			}


		public DiaryModel Create ( Diary diary )
			{
			return new DiaryModel ( )
			{
				CurrentDate = diary . CurrentDate ,
				Url = _urlHepler . Link ( "Diary" , new { diaryId = diary . CurrentDate . ToString ( "yyyy-MM-dd" ) } ) ,
				Entries = diary . Entries . Select ( Create )
			};
			}


		public DiaryEntryModel Create ( DiaryEntry diaryEntry )
			{
			return new DiaryEntryModel ( )
			{
				FoodDescription = diaryEntry . FoodItem . Description ,
				MeasureDescription = diaryEntry . Measure . Description ,
				MeasureUrl = _urlHepler . Link ( "Measures" , new { foodId = diaryEntry . Measure . Food . Id , Id = diaryEntry . Measure . Id } ) ,
				Quantity = diaryEntry . Quantity ,
				Url = _urlHepler . Link ( "DiaryEntry" , new { diaryId = diaryEntry . Diary . CurrentDate . ToString ( "yyyy-MM-dd" ) , Id = diaryEntry . Id } )
			};

			}


		public DiaryEntry Parse ( DiaryEntryModel model )
			{
			try
				{
				DiaryEntry entity = new DiaryEntry ( );
				if ( model . Quantity != default ( double ) )
					{
					entity . Quantity = model . Quantity;
					}
				if ( !string . IsNullOrWhiteSpace ( model . MeasureUrl ) )
					{
					var uri =  new Uri ( model . MeasureUrl );
					var mearureId = int . Parse ( uri . Segments . Last ( ) );
					var measure = _repo . GetMeasure ( mearureId );
					entity . Measure = measure;
					entity . FoodItem = measure . Food;
					}
				return entity;
				}
			catch
				{
				return null;
				}
			}


		public DiarySummaryModel CreateSummary ( Diary diary )
			{
			return new DiarySummaryModel ( )
			{
				DiaryDate = diary . CurrentDate ,
				TotalCalories = Math . Round ( diary . Entries . Sum ( x => x . Measure . Calories * x . Quantity ) )
			};
			}


		public AuthTokenModel Create ( AuthToken authtoken )
			{
			return new AuthTokenModel ( )
			{
				Token = authtoken . Token ,
				Expiration = authtoken . Expiration
			};
			}


		public MeasureV2Model  CreateV2 ( Measure measure )
			{
			return new MeasureV2Model ( )
			{
				Url = _urlHepler . Link ( "Measures" , new { foodId = measure . Food . Id , Id = measure . Id } ) ,
				Calories = Math . Round ( measure . Calories ) ,
				Description = measure . Description ,
				Carbohydrates = measure . Carbohydrates ,
				Cholestrol = measure . Cholestrol ,
				Fiber = measure . Fiber ,
				Iron = measure . Iron ,
				Protein = measure . Protein ,
				SaturatedFat = measure . SaturatedFat ,
				Sodium = measure . Sodium ,
				Sugar = measure . Sugar ,
				TotalFat = measure . TotalFat

			};
			}
		}
	}