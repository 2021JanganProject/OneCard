using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenedCard : MonoBehaviour
{
    [SerializeField] private CardManager CM;

    private int layerOrder;
    private void Awake()
    {
        layerOrder = 1;
        CM = FindObjectOfType<CardManager>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "MyCardEnlarge")
        {
            if(collision.transform.GetComponent<PlayCard>().IsDrop == true)
            {
                collision.transform.GetComponent<PlayCard>().IsDrop = false;
                collision.transform.GetComponent<PlayCard>().IsEnlarge = false;
                    if(CM != null)
                    {
                        int index = collision.transform.GetComponent<SpriteRenderer>().sortingOrder - 1;
                        CM.RemoveCard(index);
                        CM.OpenedCardDeck.Add(collision.gameObject);
                        collision.gameObject.transform.parent = this.transform;
                        collision.transform.position = transform.position;
                        collision.transform.rotation = transform.rotation;
                        collision.transform.localScale = transform.localScale;
                        collision.transform.GetComponent<SpriteRenderer>().sortingOrder = layerOrder++;
                        collision.transform.GetComponent<SpriteRenderer>().sortingLayerName = "CardOnField";
                        CM.EnlargeMyCardHand();
                        CM.SetCardOrderFuck();
                        
                    }
                    
                }
                
            }
        }

    }

