package ca.qc.johnabbott.cs406;

import java.util.ArrayList;
import java.util.List;

//Tests the two classes (Permuter, Combiner)
public class Main {

    public static void main(String[] args) {
        //Populate Elements
        List<Integer> elems = new ArrayList<>();
        for(int i = 1; i < 5; i++){
            elems.add(i);
        }

        //Create and test permuter
        Permuter<Integer> per = new Permuter<>(3, elems);
        System.out.println(per.generateAll());
        System.out.println(per.generateSome(3));

        //Create and test combiner
        Combiner<Integer> com = new Combiner<>(3, elems);
        System.out.println(com.generateAll());
        System.out.println(com.generateSome(3));
    }
}