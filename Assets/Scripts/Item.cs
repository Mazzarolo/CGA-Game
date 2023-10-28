using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aux_Classes
{
    public class Item
    {
        public int itemID;
        public string itemName;
        public float itemPrice;

        public bool owned;

        public Item(int id, string name, float price)
        {
            itemID = id;
            itemName = name;
            itemPrice = price;
            owned = false; 
        }
    }
}
