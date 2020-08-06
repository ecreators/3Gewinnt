using System;
using System.Collections;
using UnityEngine;

public class ZelleComponent : MonoBehaviour
{
    public event Action<ZellInhaltComponent> ContentReachedCellEvent;

    public (int column, int row) Index { get; set; }

    public ZellInhaltComponent Inhalt
    {
        get
        {
            foreach (object child in this.transform)
            {
                if (child is GameObject go)
                {
                    return go.GetComponent<ZellInhaltComponent>();
                }
            }
            return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ZellInhaltComponent content))
        {
            content.Cell = this;
            StartCoroutine(WaitForStopMovement(content));
        }
    }

    private IEnumerator WaitForStopMovement(ZellInhaltComponent content)
    {
        yield return new WaitUntil(() => content.NotMoving);

        if (content.Cell == this)
        {
            ContentReachedCellEvent?.Invoke(content);
        }
    }
}
