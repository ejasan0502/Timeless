using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// Manages craftable items in given object
[RequireComponent(typeof(Inventory))]
public class CraftManager : MonoBehaviour {
    
    public int maxQueue = 5;

    public class CraftItem {
        public Recipe recipe;
        public int amount;

        public CraftItem(Recipe recipe, int amount){
            this.recipe = recipe;
            this.amount = amount;
        }
    }
    public List<CraftItem> crafting { get; private set; }
    public List<Recipe> recipes { get; private set; }
    
    private Inventory inventory;

    private bool isCrafting = false;
    private float currentIncrement = 0;

    void Awake(){
        inventory = GetComponent<Inventory>();

        crafting = new List<CraftItem>();
        recipes = new List<Recipe>();

        FillRecipeList();
    }
    
    // Fill recipe list, For testing purposes, fill list with block recipes
    private void FillRecipeList(){
        recipes = RecipeManager.instance.blockRecipes;
    }
    // Look for recipe in crafting queue
    private CraftItem GetCraftItem(Recipe recipe){
        return crafting.Where<CraftItem>(ci => ci.recipe == recipe).FirstOrDefault();
    }
    // Coroutine for crafting
    private IEnumerator Craft(){
        while ( isCrafting ){
            if ( crafting.Count > 0 ){
                yield return new WaitForSeconds(1f);
                // Progress timer for current crafting queue
                currentIncrement += 1;
                if ( currentIncrement >= crafting[0].recipe.craftTime ){
                    Debug.Log("Crafted " + crafting[0].recipe.productId);
                    inventory.AddItem(crafting[0].recipe.productId,1);
                    currentIncrement = 0;
                    crafting[0].amount -= 1;
                    if ( crafting[0].amount < 1 ){
                        crafting.RemoveAt(0);
                    }
                }
            } else {
                isCrafting = false;
            }
        }
    }

    // Add recipe to crafting queue
    public void AddCraftItem(int index, int amount){
        Recipe recipe = recipes[index];
        CraftItem ci = GetCraftItem(recipe);
        if ( ci != null ){
            Debug.Log("Increment recipe " + index);
            ci.amount += amount;
        } else {
            if ( crafting.Count < maxQueue ){
                Debug.Log("Crafting recipe " + index);
                crafting.Add(new CraftItem(recipe,amount));
                if ( !isCrafting ){
                    isCrafting = true;
                    StartCoroutine(Craft());
                }
            } else {
                Debug.Log("Too many recipes in crafting queue!");
            }
        }
    }
    // Clear all crafting queues
    public void ClearAll(){
        crafting = new List<CraftItem>();
        isCrafting = false;
        StopCoroutine(Craft());
    }
    // Clear crafting queue based on index
    public void Clear(int index){
        if ( index < crafting.Count ){
            crafting.RemoveAt(index);
            if ( index == 0 ){
                StopCoroutine(Craft());
                StartCoroutine(Craft());
            }
        }
    }
    // Checks if item can be crafted, if so, consume resources/materials from inventory
    public bool CanCraft(int index){
        Recipe recipe = recipes[index];
        foreach (KeyValuePair<string,int> item in recipe.itemsRequired){
            if ( !inventory.HasItem(item.Key,item.Value) ){
                return false;
            }
        }

        // Remove items from inventory
        foreach (KeyValuePair<string,int> item in recipe.itemsRequired){
            inventory.RemoveItem(item.Key,item.Value);
        }

        return true;
    }

}
