using System.Collections.Generic;
using UnityEngine;

public class AttackPreview : MonoBehaviour
{
     [Header("Prefabs")]
    [SerializeField] private GameObject RotateMarkPrefab;
    [SerializeField] private GameObject FourSidedZPlusMarkPrefab;
    [SerializeField] private GameObject FourSidedZMinusMarkPrefab;
    [SerializeField] private GameObject FourSidedXPlusMarkPrefab;
    [SerializeField] private GameObject FourSidedXMinusMarkPrefab;

    private List<GameObject> activePreviews = new List<GameObject>();

    public void PreviewRotateable(Vector3 position)
    {
        GameObject go = Instantiate(RotateMarkPrefab, new Vector3(position.x, 0.5f, position.z), Quaternion.identity);
        activePreviews.Add(go);
    }

    public void PreviewFourSidedPushable(Vector3 position)
    {
        for (int i = -1; i <= 1; i++)
        {
            if (i == 0) continue;

            var zGrid = GridManager.Instance.getGridFromLocation(new Vector3(position.x, 0, position.z + i));
            if (zGrid != null)
            {
                GameObject zMark = Instantiate(i == -1 ? FourSidedZMinusMarkPrefab : FourSidedZPlusMarkPrefab,
                    new Vector3(position.x, 0.5f, position.z + i), Quaternion.identity);
                activePreviews.Add(zMark);
            }

            var xGrid = GridManager.Instance.getGridFromLocation(new Vector3(position.x + i, 0, position.z));
            if (xGrid != null)
            {
                GameObject xMark = Instantiate(i == -1 ? FourSidedXMinusMarkPrefab : FourSidedXPlusMarkPrefab,
                    new Vector3(position.x + i, 0.5f, position.z), Quaternion.identity);
                activePreviews.Add(xMark);
            }
        }
    }

    public void PreviewOneSidedPushable(Vector3 throwerPos, Vector3 targetPos)
    {
        Vector3 direction = targetPos - throwerPos;

        if (direction.x > 0)
        {
            activePreviews.Add(Instantiate(FourSidedXPlusMarkPrefab, new Vector3(targetPos.x, 0.5f, targetPos.z), Quaternion.identity));
        }
        if (direction.x < 0)
        {
            activePreviews.Add(Instantiate(FourSidedXMinusMarkPrefab, new Vector3(targetPos.x, 0.5f, targetPos.z), Quaternion.identity));
        }
        if (direction.z > 0)
        {
            activePreviews.Add(Instantiate(FourSidedZPlusMarkPrefab, new Vector3(targetPos.x, 0.5f, targetPos.z), Quaternion.identity));
        }
        if (direction.z < 0)
        {
            activePreviews.Add(Instantiate(FourSidedZMinusMarkPrefab, new Vector3(targetPos.x, 0.5f, targetPos.z), Quaternion.identity));
        }
    }

    public void ClosePreviews()
    {
        foreach (var preview in activePreviews)
        {
            if (preview != null)
            {
                Destroy(preview);
            }
        }

        activePreviews.Clear();
    }
}
