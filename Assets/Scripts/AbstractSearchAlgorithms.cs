using System.Collections.Generic;
using UnityEngine;

public abstract  class AbstractSearchAlgorithms // template method gibi yazayım dedim de haybeye yazıyorum. Elimizde birkaç tane algoritma olunca yazsam çok daha iyi olacak.
                                                //kafana göre return veriyorum çünkü şimdi bunu yazarken ve büyük ihtimalle uymayacak 
{
    virtual public HashSet<Vector3> startAlgorithm(Grid grid, int deepening_count)
    {
        HashSet<Vector3> tempHashSet = new HashSet<Vector3>();
        algorithm(grid, tempHashSet,  deepening_count);
        return tempHashSet;
    }

    virtual protected void algorithm(Grid grid, HashSet<Vector3> givenHashSet, int deepening_count)
    {
        baseCase();
        bodyPart();
        iterativePart();
    }

    virtual protected Vector3 baseCase() {return Vector3.zero;}
    virtual protected void bodyPart(){}
    virtual protected void iterativePart(){}
}
