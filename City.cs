using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambulance
{
    class City
    {
        private Node[] nodes;
        private int[] hospitals;
        private int hospitalindex;
        private int[] ambulances;
        private int ambulanceindex;

        public City(int anodeNum, int hospitalnum, int ambulancenum)
        {
            nodes = new Node[anodeNum];
            hospitals = new int[hospitalnum];
            hospitalindex = 0;
            ambulances = new int[ambulancenum];
            ambulanceindex = 0;
        }

        //Coincidently the Id of a Node is also it's place in the node list
        public void AddRoad(int x, int y, int dist)
        {
            if (nodes[x] == null && nodes[y] == null)
            {
                nodes[x] = new Node(x);
                nodes[y] = new Node(y, nodes[x], dist);
                nodes[x].ConnectTo(nodes[y], dist);
            }
            else if( nodes[x] == null || nodes[y] == null)
            {
                if(nodes[x] != null)
                {
                    nodes[y] = new Node(y, nodes[x], dist);
                    nodes[x].ConnectTo(nodes[y], dist);
                }
                else
                {
                    nodes[x] = new Node(x, nodes[y], dist);
                    nodes[y].ConnectTo(nodes[x], dist);
                }
            }
            else
            {
                nodes[x].ConnectTo(nodes[y], dist);
                nodes[y].ConnectTo(nodes[x], dist);
            }
            return;
        }

        public void AddHospital(int x)
        {
            nodes[x].DeclareHospital();
            nodes[x].nearestHospital = nodes[x];
            nodes[x].nearestHospitalDist = 0;
            hospitals[hospitalindex] = x;
            hospitalindex++;
            return;
        }

        public void AddAmbulance (int x)
        {
            nodes[x].DeclareAmbulance();            
            ambulances[ambulanceindex] = x;
            ambulanceindex++;
            return;
        }

        public void DivideByHospitals()
        {
            List<Node[]> searchNodes = new List<Node[]>();
            for (int i = 0; i < hospitals.Length; i++)
            {
                searchNodes.Add(new Node[1]);
                searchNodes[i][0] = nodes[hospitals[i]];
            }

            bool notfinished;
            do
            {
                notfinished = false;
                for ( int i=0; i<hospitals.Length; i++)
                {
                    if (searchNodes[i].Length == 0)
                        continue;

                    Node currentHospital = nodes[hospitals[i]];
                    Node currentSearchNode;
                    int newSearchNodesCount = 0;

                    for ( int j = 0; j < searchNodes[i].Length; j++) //Just to know how big array we need later on
                    {
                        newSearchNodesCount += searchNodes[i][j].GetAdjNum(); //upper limit only
                    }
                    
                    Node[] newSearchNodes = new Node[newSearchNodesCount];
                    int newSearchNodesCounter = 0;

                    for ( int j = 0; j < searchNodes[i].Length; j++)
                    {
                        currentSearchNode = searchNodes[i][j];
                        foreach (KeyValuePair<Node, int> entry in currentSearchNode.GetAdjecent())
                        {
                            if (entry.Key == currentSearchNode.toHospital) // no backtracking
                                continue;
                            if (entry.Key.nearestHospital == null || entry.Key.nearestHospitalDist > (currentSearchNode.nearestHospitalDist + entry.Value) || entry.Key.nearestHospitalDist == -1)
                            {
                                entry.Key.nearestHospital = currentHospital;
                                entry.Key.nearestHospitalDist = currentSearchNode.nearestHospitalDist + entry.Value;
                                entry.Key.toHospital = currentSearchNode;
                                newSearchNodes[newSearchNodesCounter] = entry.Key;
                                newSearchNodesCounter++;
                            }
                        }
                    }

                    //the resulting array may contain fewer elements but we need the actual size
                    searchNodes[i] = new Node[newSearchNodesCounter];
                    if (newSearchNodesCounter != 0)
                    {
                        Array.Copy(newSearchNodes, searchNodes[i], newSearchNodesCounter);
                        notfinished = true;
                    }
                }
            } while (notfinished);
            return;
        }

        public void DivideByAmbulance()
        {
            List<Node[]> searchNodes = new List<Node[]>();
            for (int i = 0; i < ambulances.Length; i++)
            {
                searchNodes.Add(new Node[1]);
                searchNodes[i][0] = nodes[ambulances[i]];
            }

            bool notfinished;
            do
            {
                notfinished = false;
                for (int i = 0; i < ambulances.Length; i++)
                {
                    if (searchNodes[i].Length == 0)
                        continue;

                    Node currentAmbulance = nodes[ambulances[i]];
                    Node currentSearchNode;
                    int newSearchNodesCount = 0;

                    for (int j = 0; j < searchNodes[i].Length; j++) //Just to know how big array we need later on
                    {
                        newSearchNodesCount += searchNodes[i][j].GetAdjNum(); //upper limit only
                    }

                    Node[] newSearchNodes = new Node[newSearchNodesCount];
                    int newSearchNodesCounter = 0;

                    for (int j = 0; j < searchNodes[i].Length; j++)
                    {
                        currentSearchNode = searchNodes[i][j];
                        foreach (KeyValuePair<Node, int> entry in currentSearchNode.GetAdjecent())
                        {
                            if (entry.Key == currentSearchNode.toAmbulance) // no backtracking
                                continue;
                            if (entry.Key.nearestAmbulance == null || entry.Key.nearestAmbulanceDist > (currentSearchNode.nearestAmbulanceDist + entry.Value) || entry.Key.nearestAmbulanceDist == -1)
                            {
                                entry.Key.nearestAmbulance = currentAmbulance;
                                entry.Key.nearestAmbulanceDist = currentSearchNode.nearestAmbulanceDist + entry.Value;
                                entry.Key.toAmbulance = currentSearchNode;
                                newSearchNodes[newSearchNodesCounter] = entry.Key;
                                newSearchNodesCounter += 1;
                            }
                        }
                    }

                    //the resulting array may contain fewer elements but we need the actual size
                    searchNodes[i] = new Node[newSearchNodesCounter];
                    if (newSearchNodesCounter != 0)
                    {
                        Array.Copy(newSearchNodes, searchNodes[i], newSearchNodesCounter);
                        notfinished = true;
                    }
                }
            } while (notfinished);
            return;
        }

        public List<int> SearchNearestAmbulance(int x)
        {
            int ditance = nodes[x].nearestAmbulanceDist;
            List<int> route = new List<int>();
            RouteToAmbulancePlan(nodes[x], route);
            return route;
        }

        private List<int> RouteToAmbulancePlan(Node anode, List<int> alist)
        {
            if (anode.IsAmbulance())
            {
                alist.Add(anode.ID);
                return alist;
            }
            else
            {
                RouteToAmbulancePlan(anode.toAmbulance, alist);
                alist.Add(anode.ID);
                return alist;
            }
        }

        public List<int> SearchNearestHospital(int x)
        {
            int ditance = nodes[x].nearestAmbulanceDist;
            List<int> route = new List<int>();
            RouteToHospitalPlan(nodes[x], route);
            return route;
        }

        private List<int> RouteToHospitalPlan(Node anode, List<int> alist)
        {
            if (anode.IsHospital())
            {
                alist.Add(anode.ID);
                return alist;
            }
            else
            {
                alist.Add(anode.ID);
                RouteToHospitalPlan(anode.toHospital, alist);
                return alist;
            }
        }
        
    }
}
