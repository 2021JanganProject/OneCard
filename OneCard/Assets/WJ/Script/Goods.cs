using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Category
{
    GOLD,
    CRYSTAL,
    HEART
}
[CreateAssetMenu(fileName = "Goods", menuName = "Scriptable Object/Goods")]
public class Goods : ScriptableObject
{
    [SerializeField] private Category category = Category.GOLD;
    [SerializeField] private int amount;
    [SerializeField] private int price;
    [SerializeField] private Sprite image;
    public Category Category { get => category; }
    public int Amount { get => amount; }
    public int Price { get => price; }
    public Sprite Image { get => image; }
}
