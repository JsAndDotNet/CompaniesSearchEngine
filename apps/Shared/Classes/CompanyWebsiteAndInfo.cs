using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Classes
{
	public class CompanyAndWebsiteInfo : AzEntityBase
	{
		public CompanyAndWebsiteInfo(string indices, string stockCode, string companyName, string companyWebsite, string metaDescription)
		{
			Indices = indices;
			StockCode = stockCode;
			YahooCode = (stockCode + ".L").Replace("..", "");
			CompanyName = companyName;
			CompanyWebsite = companyWebsite;
		}

		public string Indices { get; set; }

		public string StockCode { get; set; }

		public string YahooCode { get; set; }

		public string CompanyName { get; set; }

		// The bit of data we're looking for
		public string CompanyWebsite { get; set; }

		/// <summary>
		/// The meta description from the companies webpage.
		/// </summary>
		public string Description { get; set; }

	}
}
