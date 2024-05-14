# Dcxair Backend

In the DCXAir Backend, a clean architecture oriented to the problem domain was used, the main characteristics were:
1. Storage in SQLite database
2. Data structure optimization algorithms such as Dijkstra to return the lowest cost routes from an origin to a destination, one-way or round-trip
3. Unit testing with NUnit
4. Loading data from json through an endpoint
5. Loggers that allow any error produced in the application to be stored in the SQLite database