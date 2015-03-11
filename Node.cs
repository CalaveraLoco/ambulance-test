using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambulance
{
    class Node
    {
        private int iD;
        public Node nearestHospital { get; set; }
        public int nearestHospitalDist;
        public Node nearestAmbulance { get; set; }
        public int nearestAmbulanceDist;
        public Node toHospital { get; set; }
        public Node toAmbulance { get; set; }
        private Dictionary<Node, int> adj;
        private int category;

        public int ID
        {
            get { return iD; }
        }

        public Node(int aid)
        {
            iD = aid;
            category = 0;
            adj = new Dictionary<Node,int>();
        }

        public Node(int aid, Node anode, int dist) : this(aid)
        {
            adj.Add(anode, dist);
            nearestHospitalDist = -1;
            nearestAmbulanceDist = -1;
        }

        public void ConnectTo(Node anode, int dist)
        {
            if (!adj.ContainsKey(anode))
                adj.Add(anode, dist);
            return;
        }

        public bool IsHospital() { return (category == 2) ? true : false; }
        public bool IsAmbulance() { return (category == 1) ? true : false; }

        public void DeclareHospital() { category = 2; return; }
        public void DeclareAmbulance() { category = 1; nearestAmbulance = this; nearestAmbulanceDist = 0; return; }

        public int GetAdjNum()
        {
            return adj.Count;
        }

        public Dictionary<Node,int> GetAdjecent()
        {
            return adj;
        }
        
    }
}
