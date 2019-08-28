package ca.qc.johnabbott.cs406;

import java.util.ArrayList;
import java.util.List;

public class Trie implements Lexicon {

    //Internal class of the nodes
    private class Node {
        public char element;
        public ArrayList<Node> nexts;
        public boolean end;

        public Node(char elem){
            element = elem;
            end = false;
            nexts = new ArrayList<>();
        }
    }

    //fields
    private Node root;
    private List<String> suggestions;

    //creates an empty Tries instance
    public Trie(){
        root = new Node(' ');
    }

    //adds a word to the tree (calls a recursive version)
    @Override
    public void add(String word_) {
        char[] wordArray = word_.toCharArray();
        add(wordArray, root);
    }

    //recursive method to add each character of the word
    private void add(char[] word_, Node root_){
        boolean exists = false;
        //if no more characters to add
        if(word_.length==0){
            //set end flag
            root_.end = true;
        }
        else{
            //for each child
            for(Node path: root_.nexts){
                //if it's element is equal to the next element to add
                if(path.element==word_[0]){
                    //call recursively on that node
                    exists = true;
                    add(removeOne(word_), path);
                    break;
                }
            }
            //if no node of equal element found
            if(!exists){
                //create new node
                Node newPath = new Node(word_[0]);
                root_.nexts.add(newPath);
                //remove a letter from the word to add
                char[] newWord = removeOne(word_);
                add(newWord, newPath);
            }
        }
    }

    //searches the tree for a word (calls a recursive version)
    @Override
    public boolean contains(String word_) {
        char[] wordArray = word_.toCharArray();
        return contains(wordArray, root);
    }

    //recursive method to search for a word
    private boolean contains(char[] word_, Node root_){
        //if the end of word is found
        if(word_.length==0 && root_.end)
            return true;
        //dead end in search
        else if(word_.length==0)
            return false;
        //search for a path that continues further into the word
        for(Node path: root_.nexts){
            if(path.element==word_[0]){
                return contains(removeOne(word_), path);
            }
        }
        //no path found (word is'nt found)
        return false;
    }

    //completes the prefix entered (calls a recursive version)
    @Override
    public List<String> complete(String prefix_, int limit_) {
        char[] wordArray = prefix_.toCharArray();
        String builder = "";
        Node tmpRoot = root;
        boolean found;

        //reset suggestions
        suggestions = new ArrayList<>();

        //loop for each element in the prefix
        for(int i = 0; i < wordArray.length; i++){
            found = false;
            //loop through children of the temporary root
            for(Node path: tmpRoot.nexts){
                //search for a path that continues further into the word
                if(path.element == wordArray[i]){
                    builder += path.element;
                    tmpRoot=path;
                    found = true;
                    break;
                }
            }
            //if no progressive paths found return null
            if(!found)
                return null;
        }
        //call the recursive version starting at the end of the prefix
        return complete(builder, tmpRoot, limit_);
    }

    //recursive method to complete a word search
    private List<String> complete(String builder_, Node root_, int limit_) {
        //if search limit reached stop
        if(suggestions.size() == limit_)
            return suggestions;
        //if current root is an end to a word add the word
        if(root_.end){
            suggestions.add(builder_.toString());
        }
        //word search for each child
        for(Node path: root_.nexts){
            String newBuilder = addOne(builder_, path.element);
            complete(newBuilder, path, limit_);
        }
        //return all found options (until limit)
        return suggestions;
    }

    //Helpful to me :)
    //used to create a new array from another one (avoids references) and remove one
    private char[] removeOne(char[] arr) {
        char[] tmp = new char[arr.length-1];
        for(int i = 1; i<arr.length; i++){
            tmp[i-1]=arr[i];
        }
        return tmp;
    }

    //Helpful to me :)
    //used to create a new string from another one (avoids references) and adds an element
    private String addOne(String arr_, char elem_) {
        String tmp = "";
        for(int i = 0; i<arr_.length(); i++){
            tmp += arr_.charAt(i);
        }
        tmp += elem_;
        return tmp;
    }
}
