using System.Collections.Generic;

namespace catalogue.Database.Queries
{
	public class ReleaseDetailsQuery : IQuery
	{
		private readonly KeyValuePair<string, object> _id;

		public ReleaseDetailsQuery(KeyValuePair<string, object> id) {
			_id = id;
		}

		public IDictionary<string, object> Parameters {
			get {
				return new Dictionary<string, object> { { "@Id", _id.Value } };
			}
		}

		public string Command {
			get {
				return "SELECT DISTINCT [product_productid] "
				       + ",[product_productname]"
				       + ",[product_productversionname]"
				       + ",[product_productsortname]"
				       + ",[product_labelid]"
				       + ",[product_artistid]"
				       + ",[product_producttypeid]"
				       + ",[product_productyear]"
				       + ",[product_dateadded]"
				       + ",[product_imageid]"
				       + ",[product_releasedate]"
				       + ",[product_publisherline]"
				       + ",[product_copyrightline]"
				       + ",[product_labelline]"
				       + ",[product_upccode]"
				       + ",[product_parentaladvisory]"
				       + ",[product_producturl]"
				       + ",[product_labelgroupid]"
				       + ",[product_ishidden]"
				       + ",[product_isexclusive]"
				       + ",[product_productstatusid]"
					   + ",[masterartist_masterartistid]"
					   + ",[masterartist_masterartistname]"
					   + ",[masterartist_masterartistsortname]"
					   + ",[masterartist_masterartisturl]"
					   + " FROM vw_ProductSelect WHERE product_productid=@Id";
			}
		}
	}
}