using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using ShopEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace RidoShop.Server
{
    public class TriggeredEventRepository
    {
        DocumentClient client;

        const string dbname = "MyShopDB";
        const string collectionname = "shopEvents";

        static Boolean initialized = false;

        public TriggeredEventRepository(Uri endPointUrl, string serviceKey)
        {
            client = new DocumentClient(endPointUrl, serviceKey);

        }

        private async Task Initialize()
        {
            if (!TriggeredEventRepository.initialized)
            {
                await CreateDatabaseIfNotExists(dbname);
                await CreateDocumentCollectionIfNotExists(dbname, collectionname);
                TriggeredEventRepository.initialized = true;
            }
        }

        public async Task<string> Add(TriggeredEvent te)
        {
            await Initialize();
            var res = await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(dbname, collectionname), te);
            return res.Resource.Id;
        }

        public async Task<TriggeredEvent> Get(string id)
        {
            await Initialize();
            var res = await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(dbname, collectionname, id));
            var json = res.Resource.ToString();
            return TriggeredEvent.FromJson(json);
        }

        public async Task<IList<TriggeredEvent>> GetAll()
        {
            await Initialize();

            var queryOptions = new FeedOptions { MaxItemCount = 100 };
            var q = this.client.CreateDocumentQuery<TriggeredEvent>(
                UriFactory.CreateDocumentCollectionUri(dbname, collectionname), queryOptions);
            //TODO Execute the query async
            
            var result = new List<TriggeredEvent>();            
            foreach (var item in q)
            {
                result.Add(item);
            }
            return result;            
        }

        private async Task<ResourceResponse<Document>> CreateDocumentCollectionIfNotExists(
                string databaseName, string collectionName, TriggeredEvent te)
        {
            try
            {
                return await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(
                    databaseName, collectionName, ""));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    return await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), te);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateDatabaseIfNotExists(string databaseName)
        {
            // Check to verify a database with the id=FamilyDB does not exist
            try
            {
                await this.client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
            }
            catch (DocumentClientException de)
            {
                // If the database does not exist, create a new database
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDatabaseAsync(new Database { Id = databaseName });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<ResourceResponse<DocumentCollection>> CreateDocumentCollectionIfNotExists(string databaseName, string collectionName)
        {
            try
            {
                return await this.client.ReadDocumentCollectionAsync(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));

            }
            catch (DocumentClientException de)
            {
                // If the document collection does not exist, create a new collection
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionName;

                    // Configure collections for maximum query flexibility including string range queries.
                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                    // Here we create a collection with 400 RU/s.
                    return await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseName),
                        collectionInfo,
                        new RequestOptions { OfferThroughput = 400 });

                }
                else
                {
                    throw;
                }
            }
        }
    }
}