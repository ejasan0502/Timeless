using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class RecipeManager {

    public List<Recipe> blockRecipes { get; private set; }

    private static RecipeManager _instance;
    public static RecipeManager instance {
        get {
            if ( _instance == null )
                _instance = new RecipeManager();

            return _instance;
        }
    }

    public RecipeManager(){
        blockRecipes = new List<Recipe>();

        Initialize();
    }

    public Recipe GetBlock(string id){
        return blockRecipes.Where<Recipe>(r => r.productId == id).FirstOrDefault();
    }

    // For testing purposes, manually fill lists
    private void Initialize(){
        blockRecipes = new List<Recipe>(){
            new Recipe("block-0", 5f, new List<string>(){"resource-0"}, new List<int>(){10})
        };
    }
}
