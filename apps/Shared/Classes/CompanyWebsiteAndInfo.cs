using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Classes
{
	public class CompanyAndWebsiteInfo : AzEntityBase
	{
        public CompanyAndWebsiteInfo()
        {
			// For retrieval in storage, need parameterless ctor
        }

		public CompanyAndWebsiteInfo(string indices, string stockCode, string companyName, string companyWebsite, string metaDescription)
		{
			Indices = indices;
			StockCode = stockCode;
			YahooCode = (stockCode + ".L").Replace("..", "");
			CompanyName = companyName;
			CompanyWebsite = companyWebsite;
		}

		public void SetKey()
        {
			if(Indices.ToLower().Contains("ftse"))
            {
				this.PartitionKey = "pk_co_ftse";
			}
            else
            {
				this.PartitionKey = "pk_co_unk";
			}

			// e.g. FTSE_BP
			this.RowKey = this.Indices.Replace(".", "") + "_" + this.StockCode.Replace(".", "");
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
