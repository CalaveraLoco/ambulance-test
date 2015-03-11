# ambulance-test

Just a little algorithm and console application written in C# to solve a graph traversal problem quickly and efficiently.
The task: We have hospitals and stations with ambulance cars, not necessarily at the same location.
The city's roadmap is given by a series of edges connecting 2 junctions by a given distance.
The junctions (later on Nodes) are numbered from 0-n without vacancy.
The hospitals and stations are special nodes declared after building the map.
Then two almost identical method is used to divide the city to areas dominated by the closest Ambulance and Hospital.
The division is a breadth first search.
In the case of Hospitals, each hospital are taking turns and spreading their influences.
When a hospital looks at a node and it's uncontested it takes it. If it's contested,
it only takes it if it's closer to the new challenger.
By taking turns we can eliminate some futile work of the early contenders.
And last but not least, each node has a "parent" which is the last node that took it over, which must also be the closest to the Hospital.

An alert is solved by a simple recursive call, following each nodes "closest-to-hospital" neighbour.
The order of recursive calls and value assignment is just there so the solution makes sense from the drivers point of view
(from station to accident, from accident to hospital).

Hope it helps someone.
