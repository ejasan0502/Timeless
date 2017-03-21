using UnityEngine;
using System.Collections;

public class Item {

    public string name;
    public string id;
    public bool stackable;
    public ItemType itemType;
    public float weight;
    public string iconPath;

    private Sprite icon = null;
    public Sprite Icon {
        get {
            if ( icon == null ){
                icon = (Sprite) Resources.Load(iconPath);
            }
            return icon;
        }
    }

}
