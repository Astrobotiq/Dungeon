using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    private List<Grid> foundPath;
    
    public class Node { //Vector3 olarak tuttuğumuz gridler üzerinden gcost ya da h cost gibi işlemler yapabilmek için buna dönüşüyor
        public Vector3 Position;
        public float GCost; // Cost from start to this node
        public float HCost; // Heuristic cost to the target
        public float FCost => GCost + HCost; // Total cost
        public Node Parent; // En sonda path bulurken kullanamk için parent tutuyor

        public Node(Vector3 position) {
            Position = position;
        }
    }

    public List<Grid> startAlgorithm(Grid startGrid, Grid targetGrid) {
        return AlgorithmV4(startGrid, targetGrid);
    }
    
    public List<Grid> AlgorithmV4(Grid startGrid, Grid targetGrid) {
        List<Node> openGridNodes = new List<Node>(); //açılacak nodeları tutar
        List<Node> closedGridNodes = new List<Node>(); // açılmış nodeları tutar

        Node startNode = new Node(startGrid.gameObject.transform.position);
        Node targetNode = new Node(targetGrid.gameObject.transform.position);
        startNode.GCost = 0;
        startNode.HCost = calculateHCost(startNode, targetNode);
        openGridNodes.Add(startNode);

        Node lowest = new Node(new Vector3(100, 100, 100));
        lowest.GCost = 999;
        lowest.HCost = 999;
        
        while (true) {
            foreach (Node node in openGridNodes) {
                if (node.FCost < lowest.FCost || (node.FCost == lowest.FCost && node.HCost < lowest.HCost)) {
                    lowest = node;
                }
            }

            Node currentNode = lowest;
            openGridNodes.Remove(currentNode);
            closedGridNodes.Add(currentNode);

            if (currentNode.Position == targetNode.Position) {
                return returnedPath(currentNode); // currentNode'dan geriye doğru ekleye ekleye list yapacak metoda gönderir
            }

            List<Vector3> nearNodesVec3 = calculateNearNodes(currentNode.Position);
            List<Node> temp = turnNeighborsIntoNodes(nearNodesVec3, currentNode);
            foreach (Node neighborNode in temp) { // Her bir dönen komşu node için işlemlerin yapıldığı döngü
                
                bool isAlreadyInClosed = false;
                foreach (Node closedNode in closedGridNodes) { // Zaten eskiden açılmış mı diye bakıyoruz
                    if (closedNode.Position == neighborNode.Position) {
                        isAlreadyInClosed = true;
                    }
                }
                
                // Eğer eskiden açılmış ya da üstü dolu ise atlayıp geçiyoruz
                if (GridManager.Instance.getGridFromLocation(neighborNode.Position).GetComponent<Grid>().GridObject != null 
                    || isAlreadyInClosed) { continue;}
                
                bool isAlreadyInOpen = false;
                Node foundNodeWithSameLocaiton = null;
                foreach (Node openNode in openGridNodes) { // Açılacak nodeların içinde hali hazırda var mı diye bakıyoruz
                    if (openNode.Position == neighborNode.Position) {
                        isAlreadyInOpen = true;
                        // Aynı locatinda olup daha düşük movement costu olanı tercih ederiz ondan burada referans olarak tuttuk diğer bulduğumuzu da
                        foundNodeWithSameLocaiton = openNode;  
                    }
                }
                
                // Aynı konumda olup daha düşük costa sahip olan varsa ya da hali hazırda açılmış değilse
                // Bu kısmın ilk if partı konusunda ben de anlamadım ama attığın videodaki adam yapıyor diye yapmaya çalıştım
                if (neighborNode.GCost < foundNodeWithSameLocaiton.GCost || !isAlreadyInOpen) { 
                    neighborNode.HCost = calculateHCost(neighborNode, targetNode);
                    neighborNode.Parent = currentNode;
                    if (isAlreadyInOpen == false) {
                        openGridNodes.Add(neighborNode);
                    }
                }
            }
        }
    }

    public float calculateHCost(Node input_node, Node target_node) { // heuristic costunu hesaplamak için tamamen
        return Vector3.Distance(input_node.Position, target_node.Position);
    }
    
    private List<Vector3> calculateNearNodes(Vector3 coordinates) { // Direkt Algorithmdeki method
        Vector3 location = coordinates;
        List<Vector3> nearNodes = new List<Vector3>();
        

        if(location.z==0){ // y=0 ise
            if(location.x==0){ //hem x hem y = 0 ise
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
            }
            else if(location.x==7){ //hem x hem de y 7 ise
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
            }
            else{
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
            }
        }
        else if(location.z==7){ // y=7 ise
            if(location.x==7){ //hem x hem de y 7 ise
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            }
            else if(location.x==0){ //hem x=0 y 7 ise
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            }
            else{
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            }
        }
        else if(location.x==0){ // x=0 ise
            /*if(location.z==7){ // x=0 y=7 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //sol
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //aşşağı
            }*/
            //else{
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            //}
        }
        else if (location.x==7){ // x=7 ise
            /*if(location.z==0){ // hem x=7 y=0 ise
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //sağ
                nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //yukarı
            }*/
            //else{
                nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
                nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
                nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
            //}
        }
        else{
            nearNodes.Add(new Vector3(location.x+1,location.y,location.z)); //sağ
            nearNodes.Add(new Vector3(location.x-1,location.y,location.z)); //sol
            nearNodes.Add(new Vector3(location.x,location.y,location.z+1)); //yukarı
            nearNodes.Add(new Vector3(location.x,location.y,location.z-1)); //aşşağı
        }
        return nearNodes;

    }
    
    private List<Node> turnNeighborsIntoNodes(List<Vector3> input_neighbors, Node parentNode) //Bizim Vec3 değerleri node'a dönüştürür
    {
        List<Node> returnedNodes = new List<Node>(); // Node olarak döndüreceğimiz objeleri tutar
        
        foreach (Vector3 location in input_neighbors) { // normalde Vec3 olan komşuları node'a dönüştürür, parent ekler, gcost arttırır ve üstteki list'e ekler
            Node temp = new Node(location);
            temp.Parent = parentNode;
            temp.GCost += 10;
            returnedNodes.Add(temp);
        }

        return returnedNodes;
    }

    private List<Grid> returnedPath(Node node) { // geriye doğru parent baka baka path'i liste ekler ve döndürür
        GridManager gridManager = GridManager.Instance;

        List<Grid> gridPath = new List<Grid>();
        Node currentNode = node;

        while (currentNode != null)
        {
            gridPath.Add(gridManager.getGridFromLocation(currentNode.Position));
            currentNode = currentNode.Parent;
        }

        gridPath.Reverse();
        return gridPath;
    }
}

