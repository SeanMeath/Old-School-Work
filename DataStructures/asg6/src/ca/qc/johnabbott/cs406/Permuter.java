package ca.qc.johnabbott.cs406;

import java.util.*;

//Used to create permutations of a given list of elements and a given k
public class Permuter<T> implements Sequences<T, List<T>> {

    //Base Fields
    private int k;
    private List<T> elements;

    //Holds the permutations Being Built
    private Set<List<T>> permutation;

    //Constructor
    public Permuter(int k, List<T> elements) {
        this.k = k;
        this.elements = elements;
    }

    //Generates all possible permutations
    @Override
    public Set<List<T>> generateAll() {
        //Reset permutations and create empty builder
        permutation = new LinkedHashSet<>();
        List<T> builder = new ArrayList<>();

        //Generates permutations
        generate(builder, elements, -1);
        return permutation;
    }

    //Generates some permutations
    @Override
    public Set<List<T>> generateSome(int limit) {
        //Reset permutations and create empty builder
        permutation = new LinkedHashSet<>();
        List<T> builder = new ArrayList<>();

        //Generates permutations until the given limit
        generate(builder, elements, limit);
        return permutation;
    }

    //Recursive function that generates the builder from elements and adds them to the permutations
    private void generate(List<T> builder, List<T> elems, int limit) {
        //If builder is complete OR no elements to choose
        if (builder.size() == k || elems.size() == 0) {
            //add finalized permutation and end current instance
            permutation.add(builder);
            return;
        }

        //Temporary variable to hold elements
        T elem;

        //For each element
        for (int i = 0; i < elems.size(); i++) {
            elem = elems.get(i);
            //Call recursively with one extra element in builder and one less in elements
            generate(addOne(builder, elem), removeOne(elems, elem), limit);
            //If limit is reached
            if(limit != -1 && permutation.size() >= limit){
                //End current instance
                return;
            }
        }
    }

    //Adds one element to builder (used to avoid references)
    private List<T> addOne(List<T> builder, T elem) {
        //Create the temporary variable (Builder Kind-of)
        List<T> tmp = new ArrayList<>(builder);
        //Add the element to the new builder
        tmp.add(elem);
        return tmp;
    }

    //Removes one element from elements (used to avoid references)
    private List<T> removeOne(List<T> elements, T elem) {
        //Create the temporary variable (Elements Kind-of)
        List<T> tmp = new ArrayList<>(elements);
        //Remove the element from the new elements
        tmp.remove(elem);
        return tmp;
    }
}


