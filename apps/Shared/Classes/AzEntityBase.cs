using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Classes
{


	public abstract class AzEntityBase: TableEntity
	{
		public AzEntityBase()
		{

		}

		public AzEntityBase(string partitionKey, string rowKey)
		{
			// Table storage specifics
			PartitionKey = partitionKey;
			RowKey = rowKey;
		}




		// NOTE: These properties are only required for CosmosDB
		//		 TableStorage just needs the partitionkey/row
		//[JsonProperty(PropertyName = "id")]
		//public string Id { get; set; }

		//public string EventType { get; set; }

		//public string EventSubType { get; set; }
	}
}
