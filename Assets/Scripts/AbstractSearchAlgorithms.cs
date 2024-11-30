using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSearchAlgorithms // template method gibi yazayım dedim de haybeye yazıyorum. Elimizde birkaç tane algoritma olunca yazsam çok daha iyi olacak.
                                                //kafana göre return veriyorum çünkü şimdi bunu yazarken ve büyük ihtimalle uymayacak 
{
    private HashSet<Vector3> tempHashSet = new HashSet<Vector3>();
    
    virtual public HashSet<Vector3> startAlgorithm(Grid input_grid, int deepening_count)
    {
        algorithm(input_grid, tempHashSet,  deepening_count);
        return tempHashSet;
    }

    virtual protected void algorithm(Grid input_grid, HashSet<Vector3> givenHashSet, int deepening_count)
    {
        baseCase();
        bodyPart();
        iterativePart();
    }

    virtual protected void baseCase(){}
    virtual protected void bodyPart(){}
    virtual protected void iterativePart(){}
}
