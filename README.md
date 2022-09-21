# Redis
<b>Note: In this project we have used Redis as Cache.</b>

- Open source in-memory data structure store which can be used as a Caching System or full fledged Database or Message Broker.
![Capture](https://user-images.githubusercontent.com/76180043/189323592-41445b92-8db9-44de-b956-f6c51cd16970.PNG)
![Capture](https://user-images.githubusercontent.com/76180043/189374973-f9bd6035-287c-4359-90a7-9b0e8a1be83c.PNG)
- NoSQL Key/Value Store.
- Supports multiple Data Structures.
- Built-in Replication (Master-Slave Replication feature).
![1](https://user-images.githubusercontent.com/76180043/189301670-2ee66b2e-db35-41a9-9da5-7554794beed4.PNG)

### Reasons to Do Caching
- Reducing Network Calls: By putting commonly used data in Cache.
- Avoiding Recomputations: For example, calculating average price of a thing only once and then storing it in Cache. So everytime a user request for that average price, just directly return it from Cache without fetching the required data again from DB and then recomputing it and then sending it to user. <br><br>
These two are the key reasons we do caching which eventually <b>reduces the load on DB</b> and also <b>speed-up the responses</b>.

### Redis Datatypes
1) Strings
2) Lists
3) Sets
4) Sorted Sets
5) Hashes (similar to like JS object or JSON)

### Features of Redis
- Very Flexible.
- No Schemas & Column Names.
- Very Fast: Can perform around 110,000 SETs per second, and about 81,000 GETs per second.
- Rich Datatype Suport.
- Caching & Disk Persistence.
- Is single threaded - one action at a time.
- Pipelining: You can cluster multiple commands and send them at once. So it makes Redis even more faster.
- Redis is a multi-model database supporting variety of different database paradigms with add-on modules that you can opt into as needed. For example, use a RedisJSON module which is a real-time document store just like a document-oriented database like MongoDB, use RedisSearch to turn your database into full text search engine, etc.

### Basic Commands
- SET &lt;Key&gt; &lt;Value&gt;
- GET &lt;Key&gt;
-INCR &lt;Key&gt;
-DECR &lt;Key&gt;
- EXISTS &lt;Key&gt;
- DEL &lt;Key&gt;
- FLUSHALL (clears everything on redis server)
- EXPIRE &lt;Key&gt; <time-in-secs> (expire a particular key after a certain time)
- TTL &lt;Key&gt; (return amount of seconds remaining before the key get expired)
- CLEAR (clearing out the console)
- SETEX &lt;Key&gt; &lt;time-in-secs&gt; &lt;Value&gt; (setting expiration time along with setting the value)
- PERSIST &lt;Key&gt; (if you want to take away the expiration and persist the key) (now if you will execute TTL command, it will return -1, means that the key now will not expire at all)
- MSET &lt;Key1&gt; &lt;Value1&gt; ... &lt;KeyN&gt; &lt;ValueN&gt;
- APPEND &lt;Key&gt; &lt;appended-value&gt;
- RENAME &lt;Key&gt; &lt;new-key-name&gt;
- DUMP &lt;Key&gt; (returns a serialized version of the value stored at the specified key)

### IDistributedCache interface in .Net
This is the interface you need, to access the distributed cache objects. IDistributedCache Interface provides you with the following methods to perform actions on the actual cache:
1) Set, SetAsync() - Accepts a string key and value and sets it to the cache server. These methods add an item as byte \[\](array) to the cache using a key. 
2) Get, GetAsync() - Gets the value from the cache server based on the string key. These methods accept a key and retrieve a cached item as a byte \[\](array). 
3) SetString, SetStringAsync()
4) GetString, GetStringAsync()
5) Remove, RemoveAsync()
6) Refresh, RefreshAsync() - Refreshes Sliding Expiration Time.

We have used <b>Microsoft.Extensions.Caching.StackExchangeRedis</b> implementation for IDistributed interface.

### Eager vs Lazy Approach for refreshing the data in Cache
![Capture](https://user-images.githubusercontent.com/76180043/189633119-09b0b7ff-ef2e-42c1-bb6f-3cae3d060f54.PNG)

We have used <b>Lazy Approach.</b>

# Pagination - Why is it Important?
Imagine you have an endpoint in your API that could potentially return millions of records with a single request. Let’s say there are 100s of users that are going to exploit this endpoint by requesting all the data in a single go at the same time. This would nearly kill your server and lead to several issues including security. An ideal API endpoint would allow it’s consumers to get only a specific number of records in one go. In this way, we are not giving load to our Database Server, the CPU/server on which the API is hosted, the network bandwidth, the client also (b/c if client have to deal with huge amount of data than this can lead to much longer load times, high memory consumption on the front-end application and so on). This is a highly crucial feature for any API, especially the public APIs.

<b>Note:</b> Using IQuerable&lt;T&gt; will give you these advantages/benefits, if you use IEnumerable&lt;T&gt; then you are still giving load to the Database Server, the CPU/server on which the API is hosted, or the network bandwidth b/c in IEnumerable&lt;T&gt; all the data is fetched from the database and then the CPU/server on which the API is hosted will apply filter. So if we have 100s users that will hit this endpoint than we are bringing all data 100s times from the Database Server on the CPU/server on which the API is hosted. So thats why use IQuerable&lt;T&gt; where the filter logic is executed on the Database Server.

Paging or Pagination in a method in which you get paged response. This means that you request with a page number and page size, and the ASP.NET Core WebApi returns exactly what you asked for, nothing more.

By implementing Pagination in your APIs, your Front end Developers would have a really comfortable time in building UIs that do not lag. Such APIs are good for integration by other consumers (MVC, React.js Applications) as the data already comes paginated.

It results in a system where the client is not overworked while processing the huge set of data thrown at it and the server is not overworked while processing the huge dataset to be passed down to the client. The page size is kept at optimal so that the server can process quickly and the client can receive it without stressing the channel. <b>Pagination is more of a design choice than a functionality</b>.
