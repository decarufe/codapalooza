using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Codapalooza.Models
{
	public class SubscriptionService
	{
		private int _maximumNumberOfInscriptions;
		private CodapaloozaEntities _db;

		public SubscriptionService()
		{
			int.TryParse(ConfigurationManager.AppSettings["maximumNumberOfInscriptions"], out _maximumNumberOfInscriptions);
      _db = new CodapaloozaEntities();
    }

		public int RemainingPlaces()
		{
			var numberOfInscribedParticipants = _db.Participants.Count();
			return _maximumNumberOfInscriptions - numberOfInscribedParticipants;
		}

		public static SubscriptionService Current
		{
			get { return new SubscriptionService();}
		}
	}
}