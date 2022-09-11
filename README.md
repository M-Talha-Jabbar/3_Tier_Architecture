# Redis
- Open source in-memory data structure store which can be used as a Caching System or full fledged Database or Message Broker.
![Capture](https://user-images.githubusercontent.com/76180043/189323592-41445b92-8db9-44de-b956-f6c51cd16970.PNG)
![Capture](https://user-images.githubusercontent.com/76180043/189374973-f9bd6035-287c-4359-90a7-9b0e8a1be83c.PNG)
- NoSQL Key/Value Store.
- Supports multiple Data Structures.
- Built-in Replication (Master-Slave Replication feature).
![1](https://user-images.githubusercontent.com/76180043/189301670-2ee66b2e-db35-41a9-9da5-7554794beed4.PNG)

# Reasons to Do Caching
- Reducing Network Calls: By putting commonly used data in Cache.
- Avoiding Recomputations: For example, calculating average price of a thing only once and then storing it in Cache. So everytime a user request for that average price, just directly return it from Cache without fetching the required data again from DB and then recomputing it and then sending it to user. <br><br>
These two are the key reasons we do caching which eventually <b>reduces the load on DB</b> and also <b>speed-up the responses</b>.

# Redis Datatypes
1) Strings
2) Lists
3) Sets
4) Sorted Sets
5) Hashes (similar to like JS object or JSON)

# Features of Redis
- Very Flexible.
- No Schemas & Column Names.
- Very Fast: Can perform around 110,000 SETs per second, and about 81,000 GETs per second.
- Rich Datatype Suport.
- Caching & Disk Persistence.
- Is single threaded - one action at a time.
- Pipelining: You can cluster multiple commands and send them at once. So it makes Redis even more faster.
- Redis is a multi-model database supporting variety of different database paradigms with add-on modules that you can opt into as needed. For example, use a RedisJSON module which is a real-time document store just like a document-oriented database like MongoDB, use RedisSearch to turn your database into full text search engine, etc.

# Basic Commands
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

# Redis Cache Functions with .Net 
1) SetAsync()
2) GetAsync()
3) SetStringAsync()
4) GetStringAsync()
5) RemoveAsync()
6) RefreshAsync()
