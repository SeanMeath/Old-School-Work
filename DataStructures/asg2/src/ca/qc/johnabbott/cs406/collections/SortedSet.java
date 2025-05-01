package ca.qc.johnabbott.cs406.collections;

import java.util.Arrays;

public class SortedSet<T extends Comparable<T>> implements Set<T>, Traversable<T>{

    private static final int DEFAULT_CAPACITY = 100;

    private T[] elements;
    private int size;

    //Traversable Variables
    //Position holds the current position of the traverse
    //TraverseFlag holds whether changes have been made during a traverse
    private int position = 0;
    private boolean traverseFlag = false;

    public SortedSet() {
        this(DEFAULT_CAPACITY);
    }

    public SortedSet(int capacity) {
        this.size = 0;
        this.elements = (T[]) new Comparable[capacity];
    }

    @Override
    public boolean contains(T elem) {
        // elements is sorted, so we can binary search for the element.
        return Arrays.binarySearch(elements, 0, size, elem) >= 0;
    }

    //Checks whether the sortedSet contains all the elements in the given set
    @Override
    public boolean containsAll(Set<T> rhs) {
        //Holds the variable to look for
        T checkVar;
        //Reset the position in the traverse
        rhs.reset();
        //Loops through all the variable in the rhs set
        while(rhs.hasNext()){
            //Get next variable to look for
            checkVar = rhs.next();
            //If it's not found
            if(Arrays.binarySearch(elements, 0, size, checkVar) < 0) return false;
        }
        return true;
    }

    //Adds an element to the sortedSet
    //returns whether the element was already in there or not
    //Throws error if it's full
    //Adds to size if you added something
    //Sets traverse flag if traverse in progress
    @Override
    public boolean add(T elem) {
        //Swap Variable
        T temp;
        //If Full
        if(size() == elements.length) throw new FullSetException();
        //If element is already added
        if(contains(elem)) return false;
        //Loop through the elements
        for(int i = 0; i < size(); i++){
            //If the element is smaller than the other one swap
            if(elements[i].compareTo(elem) > 0){
                temp = elements[i];
                elements[i] = elem;
                elem = temp;
            }
        }
        //If traverse has started, set flag
        if(position != 0) traverseFlag = true;
        //Add the new last element and increase size
        elements[size()] = elem;
        size++;
        return true;
    }

    //Removes the specified element from the sorted set
    //Returns whether or not you found the element
    //If found removes the element and move everything over to the left
    //Decrease size if you removed something
    //Sets traverse flag if traverse in progress
    @Override
    public boolean remove(T elem) {
        //Get the index of the element to remove or a negative if not found
        int foundIndex = Arrays.binarySearch(elements, 0, size, elem);
        //If not found
        if(foundIndex < 0) return false;
        //Loop from the element onwards moving everything forward (Overwriting the one to be deleted)
        for(int i = foundIndex; i < size() - 1; i++) elements[i] = elements[i + 1];
        //If the traverse has started, set Flag
        if(position != 0) traverseFlag = true;
        //Lower size
        size--;
        return true;
    }

    //Gets the amount of elements in the sortedSet
    @Override
    public int size() {
        return size;
    }

    //Checks if the sortedSet is empty
    @Override
    public boolean isEmpty() {
        return (size() == 0);
    }

    //Gets the smallest element in the sortedSet (first element)
    //Throws error if the sortedSet is empty
    public T min() {
        //If empty return error
        if(isEmpty()) throw new EmptySetException();
        //return the first element (sorted set)
        return elements[0];
    }

    //Gets the largest element in the sortedSet (last element)
    //Throws error if the sortedSet is empty
    public T max() {
        //If empty return error
        if(isEmpty()) throw new EmptySetException();
        //return the first element (sorted set)
        return elements[size()-1];
    }

    //Creates a subset of the current sortedSet
    //Uses the two variable given to find the first index and the last index to use
    //Throws error if the first is larger than the last
    public SortedSet<T> subset(T first, T last) {
        //If first is bigger then last send error
        if(first.compareTo(last) > 0)
            throw new IllegalArgumentException("Cannot have a first item being bigger than the last item");
        //Get the supposed position of first and last (negative if its currently not in there)
        int sp = Arrays.binarySearch(elements, 0, size, first);
        int ep = Arrays.binarySearch(elements, 0, size, last);
        //If its's smaller than 0 add 1 to place it in the right position
        if(sp < 0) sp++;
        if(ep < 0) ep++;
        //Make sure it's positive
        int startPos = Math.abs(sp);
        int endPos = Math.abs(ep);
        //Get length of the subset
        int length = endPos-startPos;
        //Initialize subset
        SortedSet<T> subset = new SortedSet<>(length);
        //Loop through startPos to Endpos adding the elements i'th position each time
        for(int i = startPos; i < endPos; i++) subset.add(elements[i]);
        //Returns the subset
        return  subset;
    }

    //Checks if the sortedSet is full
    @Override
    public boolean isFull() {
        return (size() == elements.length);
    }

    //Outputs the contents of the sortedSet
    @Override
    public String toString() {
        //Initialise a stringBuilder
        StringBuilder sb = new StringBuilder();
        //Add starting braces
        sb.append('{');
        //Prepare a bool for commas
        boolean first = true;
        //Loop through each of the elements
        for(T x : this.elements) {
            //If it's an empty array close brace and return
            if(x == null){
                sb.append('}');
                return sb.toString();
            }
            //If it's the first element don't add a comma
            else if(first)
                first = false;
            //Add comma
            else
                sb.append(", ");
            //Add the element
            sb.append(x);
        }
        //Close brace and return
        sb.append('}');
        return sb.toString();
    }

    //Resets the traversal
    @Override
    public void reset() {
        //Reset the position of the traverse as well as it's flag
        position = 0;
        traverseFlag = false;
    }

    //Gets the next element of the sortedSet
    //Throws errors if called and the end of a traversal
    @Override
    public T next() {
        //If called at the end of a traversal send error
        if(!hasNext() || traverseFlag) throw new TraversalException();
        //Return the next element and get the next element
        return elements[position++];
    }

    //Returns whether or not we have reached the end of the traversal
    @Override
    public boolean hasNext() {
        //Returns if there are more elements to come (Reached end or no)
        return (position < size());
    }
}