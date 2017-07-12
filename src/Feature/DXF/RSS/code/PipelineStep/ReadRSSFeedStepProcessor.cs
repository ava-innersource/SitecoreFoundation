using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SF.DXF.Feature.RSS
{
    [RequiredEndpointPlugins(typeof(RssSettings))]
    public class ReadRSSFeedStepProcessor : BaseReadDataStepProcessor
    {
        public ReadRSSFeedStepProcessor() 
        {
        }

        protected override void ReadData(
            Endpoint endpoint,
            PipelineStep pipelineStep,
            PipelineContext pipelineContext)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("Endpoint");
            }
            if (pipelineStep == null)
            {
                throw new ArgumentNullException("PipelineStep");
            }
            if (pipelineContext == null)
            {
                throw new ArgumentNullException("PipelineContext");
            }
            var logger = pipelineContext.PipelineBatchContext.Logger;
            
            //get the rss feed url from the plugin on the endpoint
            var settings = endpoint.GetRssSettings();
            if (settings == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(settings.FeedUrl))
            {
                logger.Error(
                    "No feed url is specified on the endpoint. " +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }
            
            var lines = new List<string[]>();
            
            //todo, fetch the RSS feed and then put into the context.
            try
            {
                var rssReader = XmlReader.Create(settings.FeedUrl);
                var feed = SyndicationFeed.Load(rssReader);


                //add the data that was read from the file to a plugin
                var dataSettings = new IterableDataSettings(GetFeedEnumerable(feed));
                logger.Info(
                    "{0} was loaded. (pipeline step: {1}, endpoint: {2})",
                     settings.FeedUrl, pipelineStep.Name, endpoint.Name);
                //
                //add the plugin to the pipeline context
                pipelineContext.Plugins.Add(dataSettings);
            }
            catch(Exception ex)
            {
                logger.Error(
                   "Unable to download and parse RSS feed" +
                    "(pipeline step: {0}, endpoint: {1}, feedUrl: {2}, error: {3})",
                    pipelineStep.Name, endpoint.Name, settings.FeedUrl, ex);
            }
        }

        private IEnumerable<FeedItem> GetFeedEnumerable(SyndicationFeed feed)
        {
            foreach(var item in feed.Items)
            {
                var feedItem = new FeedItem();
                feedItem.Title = item.Title.Text;
                feedItem.Summary = item.Summary.Text;
                feedItem.PublishDate = item.PublishDate.LocalDateTime;

                TextSyndicationContent textContent = item.Content as TextSyndicationContent;
                if (textContent != null)
                {
                    feedItem.Content = textContent.Text;
                }
                
                if (item.Links.Count > 0)
                {
                    feedItem.BaseUri = item.Links[0].GetAbsoluteUri().ToString();
                }

                yield return feedItem;
            }
        }
    }

    public class FeedItem
    {
        public string Title {get;set;}
        public string Summary {get;set;}
        public string Content {get;set;}
        public DateTime PublishDate {get;set;}

        public string BaseUri {get;set;}

    }
}
