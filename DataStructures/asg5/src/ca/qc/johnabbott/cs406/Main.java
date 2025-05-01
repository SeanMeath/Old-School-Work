package ca.qc.johnabbott.cs406;

import ca.qc.johnabbott.cs406.collections.list.ArrayList;
import ca.qc.johnabbott.cs406.collections.list.LinkedList;
import ca.qc.johnabbott.cs406.collections.list.List;
import ca.qc.johnabbott.cs406.collections.map.NaiveMap;
import ca.qc.johnabbott.cs406.collections.map.HashMap;
import ca.qc.johnabbott.cs406.generator.Generator;
import ca.qc.johnabbott.cs406.generator.SentenceGenerator;
import ca.qc.johnabbott.cs406.generator.WordGenerator;
import ca.qc.johnabbott.cs406.profiler.Profiler;


import java.util.Map;
import java.util.Random;


public class Main {

    public static final Generator<String> STRING_GENERATOR;
    public static final Random RANDOM;
    public static final int SAMPLE_SIZE = 10000;

    static {
        RANDOM = new Random();
        STRING_GENERATOR = new SentenceGenerator(new WordGenerator("foo bar baz qux quux quuz corge grault garply waldo fred plugh xyzzy thud".split(" ")), 10);
    }

    public static void main(String[] args) throws InterruptedException {



        Profiler.getInstance().startSection("LinkedList");
        List<String> linkedList = listInitializeWithAppend(new LinkedList<>());
        linkedList = listInitializeWithAppendPos(new LinkedList<>());
        linkedList = listInitializeWithAppendMiddle(new LinkedList<>());
        linkedList = listInitializeWithAppendAlmostSize(new LinkedList<>());
        reverseListFrontToEnd(linkedList);
        reverseListEndToFront(linkedList);
        loopedListSearch(linkedList);
        traversalListSearch(linkedList);
        Profiler.getInstance().endSection();

        Profiler.getInstance().startSection("ArrayList");
        List<String> arrayList = listInitializeWithAppend(new ArrayList<>());
        arrayList = listInitializeWithAppendPos(new ArrayList<>());
        arrayList = listInitializeWithAppendMiddle(new ArrayList<>());
        arrayList = listInitializeWithAppendAlmostSize(new ArrayList<>());
        reverseListFrontToEnd(arrayList);
        reverseListEndToFront(arrayList);
        loopedListSearch(arrayList);
        traversalListSearch(arrayList);
        Profiler.getInstance().endSection();

        Profiler.getInstance().startSection("HashMap");
        ca.qc.johnabbott.cs406.collections.map.Map<Integer, String> hashMap = constructMap(new HashMap<>());
        searchMap(hashMap);
        Profiler.getInstance().endSection();

        Profiler.getInstance().startSection("NaiveMap");
        ca.qc.johnabbott.cs406.collections.map.Map<Integer, String> naiveMap = constructMap(new NaiveMap<>());
        searchMap(naiveMap);
        Profiler.getInstance().endSection();

        Report.printAllSections(Profiler.getInstance().getSections());
    }

    //Create a list using Add(x)
    private static List<String> listInitializeWithAppend(List<String> list) {
        for(int i = 0; i< SAMPLE_SIZE; i++)
            list.add(STRING_GENERATOR.generate(RANDOM));
        return list;
    }

    //Create a list adding to the end as you go
    private static List<String> listInitializeWithAppendPos(List<String> list) {
        for(int i = 0; i< SAMPLE_SIZE; i++)
            list.add(i, STRING_GENERATOR.generate(RANDOM));
        return list;
    }

    //Create a list adding to the middle as you go
    private static List<String> listInitializeWithAppendMiddle(List<String> list) {
        for(int i = 0; i< SAMPLE_SIZE; i++)
            list.add(i/2, STRING_GENERATOR.generate(RANDOM));
        return list;
    }

    //Create a list adding near the end of the list as you go
    private static List<String> listInitializeWithAppendAlmostSize(List<String> list) {
        int posToAdd;
        for(int i = 0; i< SAMPLE_SIZE; i++) {
            posToAdd = i - 2;
            if (posToAdd < 0) {
                posToAdd = 0;
            }
            list.add(posToAdd, STRING_GENERATOR.generate(RANDOM));
        }
        return list;
    }

    //Reverse the list removing the first elements and adding tehm to the end
    private static void reverseListFrontToEnd(List<String> list) {
        int size = list.size();
        for(int i = 0; i <= size - 1; i++){
            list.add(size - 1, list.remove(0));
        }
    }

    //Reverse the list removing the last elements and adding them to the start
    private static void reverseListEndToFront(List<String> list) {
        int size = list.size();
        for(int i = 0; i <= size - 1; i++){
            list.add(0, list.remove(list.size()-1));
        }
    }

    //Searches for the largest value in the list via loops
    private static void loopedListSearch(List<String> list) {
        int size = list.size();
        String max = null;
        String tmp;
        for(int i = 0; i < size - 1; i++){
            if(i==0){
                max = list.get(i);
            }
            else{
                tmp = list.get(i);
                if(tmp.compareTo(max) > 1){
                    max = tmp;
                }
            }
        }
    }

    //Searches for the largest value in the list via traversable calls
    private static void traversalListSearch(List<String> list) {
        String tmp;
        list.reset();
        String max = list.next();
        while(list.hasNext()){
            tmp = list.next();
            if(tmp.compareTo(max) > 1){
                max = tmp;
            }
        }
    }


    //Creates a map by adding as you go
    private static ca.qc.johnabbott.cs406.collections.map.Map<Integer, String> constructMap(ca.qc.johnabbott.cs406.collections.map.Map<Integer, String> map) {
        for(int i = 0; i< SAMPLE_SIZE; i++)
            map.put(i, STRING_GENERATOR.generate(RANDOM));
        return map;
    }

    //Check test searching (first half works, second half fails to find)
    private static void searchMap(ca.qc.johnabbott.cs406.collections.map.Map<Integer, String> map) {
        for(int i = 0; i< SAMPLE_SIZE*2; i++)
            map.containsKey(i);
    }
    
}
