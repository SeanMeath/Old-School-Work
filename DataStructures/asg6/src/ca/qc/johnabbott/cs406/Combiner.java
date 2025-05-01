package ca.qc.johnabbott.cs406;

import java.util.*;

//Used to create combinations of a given list of elements and a given k
public class Combiner<T> implements Sequences<T, Set<T>> {

    //Base Fields
    private int k;
    private List<T> elements;

    //Holds the combinations Being Built
    private Set<Set<T>> combination;

    //Constructor
    public Combiner(int k, List<T> elements) {
        this.k = k;
        this.elements = elements;
    }

    //Generates all possible combinations
    @Override
    public Set<Set<T>> generateAll() {
        //Reset combinations and create empty builder
        combination = new LinkedHashSet<>();
        Set<T> builder = new LinkedHashSet<>();

        //Generates combinations
        generate(builder, elements, -1);
        return combination;
    }

    //Generates some combinations
    @Override
    public Set<Set<T>> generateSome(int limit) {
        //Reset combinations and create empty builder
        combination = new LinkedHashSet<>();
        Set<T> builder = new LinkedHashSet<>();

        //Generates combinations until the given limit
        generate(builder, elements, limit);
        return combination;
    }

    //Recursive function that generates the builder from elements and adds them to the combinations
    private void generate(Set<T> builder, List<T> elems, int limit) {
        //If builder is complete OR no elements to choose
        if (builder.size() == k || elems.size() == 0) {
            //add finalized combination and end current instance
            combination.add(builder);
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
            if(limit != -1 && combination.size() >= limit){
                //End current instance
                return;
            }
        }
    }

    //Adds one element to builder (used to avoid references)
    private Set<T> addOne(Set<T> builder, T elem) {
        //Create the temporary variable (Builder Kind-of)
        Set<T> tmp = new LinkedHashSet<>(builder);
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
