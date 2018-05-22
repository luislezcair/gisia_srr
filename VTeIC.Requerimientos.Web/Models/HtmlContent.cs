using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace busquedaSolrnet.Models
{
    public class HtmlContent
    {
    [SolrField("absoluteuri")]
    public string AbsoluteUri { get; set; }
    [SolrUniqueKey("id")]
    public long DiscoveryID { get; set; }
    [SolrField("score")]
    public double Score { get; set; }
    [SolrField("title")]
    public string Title { get; set; }
    [SolrField("extension")]
    public string Extension { get; set; }
    [SolrField("publishdate")]
    public string PublishDate { get; set; }
    [SolrField("text")]
    public string Text { get; set; }
    [SolrField("idRuta")]
    public string IdRuta { get; set; }
    
}

}